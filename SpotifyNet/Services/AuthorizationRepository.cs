using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Runtime.Versioning;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using SpotifyNet.Clients.Abstractions;
using SpotifyNet.Datastructures.Spotify.Authorization;
using SpotifyNet.Services.Abstractions;

namespace SpotifyNet.Services;

/// <summary>
/// The <see cref="IAuthorizationRepository"/> that keeps the access token, and the sign-in under
/// way, as JSON files, refreshing the token when it's expiring. On Unix only the owner can read
/// the files.
/// </summary>
/// <param name="authorizationClient">
/// Exchanges authorization codes and refresh tokens for access tokens.
/// </param>
/// <param name="directory">Where to keep the token files; the user's local app data by default.</param>
public sealed class AuthorizationRepository(
    IAuthorizationClient authorizationClient,
    string? directory = null
) : IAuthorizationRepository
{
    private readonly string _authorizationMetadataPath = Path.Combine(
        directory ?? DefaultDirectory,
        "AuthorizationMetadata.json"
    );

    private readonly string _accessTokenPath = Path.Combine(
        directory ?? DefaultDirectory,
        "AccessToken.json"
    );

    // Serializes reading, refreshing and saving the token, which is cached once read.
    private readonly SemaphoreSlim _tokenLock = new(1, 1);

    private AccessTokenMetadata? _token;

    // Set while the cached token is newer than its file, as writing it failed.
    private bool _unsaved;

    // DoNotVerify: otherwise a missing folder (e.g. ~/.local/share on a fresh Linux box) gives "",
    // and the files would land in the working directory. Write creates the folder.
    private static string DefaultDirectory =>
        Path.Combine(
            Environment.GetFolderPath(
                Environment.SpecialFolder.LocalApplicationData,
                Environment.SpecialFolderOption.DoNotVerify
            ),
            "SpotifyNet"
        );

    /// <inheritdoc />
    public async Task<string> GetUserAuthorizeUri(
        IEnumerable<string> scopes,
        CancellationToken cancellationToken
    )
    {
        ImmutableArray<string> requestedScopes = [.. scopes];

        var authorization = authorizationClient.GetUserAuthorization(requestedScopes);

        var metadata = new UserAuthorizationMetadata
        {
            AuthorizationScopes = requestedScopes,
            CodeVerifier = authorization.CodeVerifier,
            State = authorization.State,
        };

        await Write(_authorizationMetadataPath, metadata, cancellationToken);

        return authorization.AuthorizationUri;
    }

    /// <inheritdoc />
    public async Task<AccessTokenMetadata> GetNewAccessToken(
        string code,
        string state,
        CancellationToken cancellationToken
    )
    {
        var authorization = await Read<UserAuthorizationMetadata>(
            _authorizationMetadataPath,
            cancellationToken
        );

        if (state != authorization.State)
        {
            throw new AuthorizationException(
                "The authorization callback's state doesn't match the request's."
            );
        }

        // Not cancellable from here on: Spotify spends the code as soon as it answers.
        var token = await authorizationClient.GetUserAccessToken(
            code,
            authorization.CodeVerifier,
            CancellationToken.None
        );

        var refreshToken =
            token.RefreshToken
            ?? throw new AuthorizationException("Spotify didn't return a refresh token.");

        await _tokenLock.WaitAsync(CancellationToken.None);
        try
        {
            return await Save(token, refreshToken, authorization.AuthorizationScopes);
        }
        finally
        {
            _tokenLock.Release();
        }
    }

    /// <inheritdoc />
    public async Task<AccessTokenMetadata> GetAccessToken(CancellationToken cancellationToken)
    {
        await _tokenLock.WaitAsync(cancellationToken);
        try
        {
            var stored =
                await Load(cancellationToken)
                ?? throw new AuthorizationException(
                    "There's no access token yet; sign in with ITokenAcquirerService first."
                );

            if (!stored.IsExpiring)
            {
                return stored;
            }

            // Not cancellable: Spotify may rotate the refresh token, so dropping its answer would
            // strand the session.
            var token = await authorizationClient.RefreshUserAccessToken(
                stored.RefreshToken,
                CancellationToken.None
            );

            // Spotify leaves the refresh token out when the current one stays valid.
            return await Save(
                token,
                token.RefreshToken ?? stored.RefreshToken,
                stored.AuthorizationScopes
            );
        }
        finally
        {
            _tokenLock.Release();
        }
    }

    /// <inheritdoc />
    public async Task<AccessTokenMetadata?> GetStoredAccessToken(
        CancellationToken cancellationToken
    )
    {
        await _tokenLock.WaitAsync(cancellationToken);
        try
        {
            return await Load(cancellationToken);
        }
        finally
        {
            _tokenLock.Release();
        }
    }

    // The cached token, read from its file the first time; null before the first sign-in.
    private async Task<AccessTokenMetadata?> Load(CancellationToken cancellationToken)
    {
        if (_token is null)
        {
            _token = File.Exists(_accessTokenPath)
                ? await Read<AccessTokenMetadata>(_accessTokenPath, cancellationToken)
                : null;
        }
        else if (_unsaved)
        {
            await RetryFailedWrite(_token);
        }

        return _token;
    }

    // Not cancellable, and cached before it's written: a token Spotify has issued must be kept,
    // even if saving it fails. A failed write is reported, then retried by later calls.
    private async Task<AccessTokenMetadata> Save(
        AccessToken token,
        string refreshToken,
        ImmutableArray<string> requestedScopes
    )
    {
        // An omitted scope means the requested ones were granted; an empty one grants none.
        var grantedScopes = token.Scope?.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        _token = new AccessTokenMetadata
        {
            AuthorizationScopes = grantedScopes is null ? requestedScopes : [.. grantedScopes],
            Token = token.Token,
            ExpiresAtUtc = DateTimeOffset.UtcNow.AddSeconds(token.ExpiresIn),
            RefreshToken = refreshToken,
        };

        _unsaved = true;
        await Write(_accessTokenPath, _token, CancellationToken.None);
        _unsaved = false;

        return _token;
    }

    // Quietly, as the failure was reported when it happened.
    private async Task RetryFailedWrite(AccessTokenMetadata token)
    {
        try
        {
            await Write(_accessTokenPath, token, CancellationToken.None);
            _unsaved = false;
        }
        catch (Exception e) when (e is IOException or UnauthorizedAccessException) { }
    }

    private static async Task<T> Read<T>(string path, CancellationToken cancellationToken)
    {
        await using var stream = File.OpenRead(path);

        if (!OperatingSystem.IsWindows())
        {
            MakePrivate(stream);
        }

        var item = await JsonSerializer.DeserializeAsync<T>(
            stream,
            cancellationToken: cancellationToken
        );

        return item ?? throw new JsonException($"{path} holds null.");
    }

    // Files an earlier version wrote can be readable by others. Best effort: a file that isn't
    // ours can't be changed, but can still be read.
    [UnsupportedOSPlatform("windows")]
    private static void MakePrivate(FileStream stream)
    {
        try
        {
            File.SetUnixFileMode(
                stream.SafeFileHandle,
                UnixFileMode.UserRead | UnixFileMode.UserWrite
            );
        }
        catch (Exception e) when (e is IOException or UnauthorizedAccessException) { }
    }

    // Writes a temporary file and swaps it in, so the file is never left half-written. The files
    // hold the tokens, so on Unix only the owner can read them, as local app data ensures on
    // Windows.
    private static async Task Write<T>(string path, T item, CancellationToken cancellationToken)
    {
        var directory = Path.GetDirectoryName(path)!;
        var temporaryPath = path + ".tmp";
        var options = new FileStreamOptions
        {
            Mode = FileMode.Create,
            Access = FileAccess.Write,
            Share = FileShare.None,
        };

        if (OperatingSystem.IsWindows())
        {
            Directory.CreateDirectory(directory);
        }
        else
        {
            Directory.CreateDirectory(
                directory,
                UnixFileMode.UserRead | UnixFileMode.UserWrite | UnixFileMode.UserExecute
            );

            // Created afresh, as a file left over from a failed write would keep its mode.
            File.Delete(temporaryPath);
            options.UnixCreateMode = UnixFileMode.UserRead | UnixFileMode.UserWrite;
        }

        await using (var stream = new FileStream(temporaryPath, options))
        {
            await JsonSerializer.SerializeAsync(stream, item, cancellationToken: cancellationToken);
        }

        File.Move(temporaryPath, path, overwrite: true);
    }
}
