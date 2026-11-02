using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Versioning;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using SpotifyNet.Clients.Abstractions;
using SpotifyNet.Datastructures.Spotify.Authorization;
using SpotifyNet.Services;
using SpotifyNet.Services.Abstractions;
using TUnit.Core.Enums;

namespace SpotifyNet.Tests.Services;

public sealed class AuthorizationRepositoryTests : IDisposable
{
    private readonly string _directory = Directory.CreateTempSubdirectory().FullName;

    [Test]
    public async Task RefreshesAnExpiringTokenOnceForConcurrentCalls()
    {
        var client = new RefreshingClient();
        var repository = await RepositoryWithExpiringToken(client);

        var tokens = await Task.WhenAll(
            Enumerable.Range(0, 8).Select(_ => repository.GetAccessToken())
        );
        var reloaded = await Repository(client).GetAccessToken();

        await Assert.That(client.Refreshes).IsEqualTo(1);
        await Assert.That(tokens.All(t => t.Token == "new")).IsTrue();
        await Assert.That(reloaded.Token).IsEqualTo("new");
        // The refresh response left the refresh token out, so the current one is kept.
        await Assert.That(reloaded.RefreshToken).IsEqualTo("refresh");
    }

    [Test]
    public async Task KeepsARefreshItsCallerCancels()
    {
        using var cancellation = new CancellationTokenSource();
        var client = new RefreshingClient
        {
            DuringRefresh = cancellation.Cancel,
            NewRefreshToken = "rotated",
        };
        var repository = await RepositoryWithExpiringToken(client);

        await repository.GetAccessToken(cancellation.Token);
        var reloaded = await Repository(client).GetAccessToken();

        // Spotify has already spent the old refresh token, so the rotated one must be saved.
        await Assert.That(reloaded.RefreshToken).IsEqualTo("rotated");
    }

    [Test]
    public async Task KeepsARefreshItCouldNotSave()
    {
        // A folder in the way of the temporary file makes the write fail.
        var blocker = Path.Combine(_directory, "AccessToken.json.tmp");
        var client = new RefreshingClient
        {
            DuringRefresh = () => Directory.CreateDirectory(blocker),
        };
        var repository = await RepositoryWithExpiringToken(client);

        await Assert.That(() => (Task)repository.GetAccessToken()).Throws<Exception>();
        var token = await repository.GetAccessToken();
        Directory.Delete(blocker);
        await repository.GetAccessToken();
        var reloaded = await Repository(client).GetAccessToken();

        await Assert.That(token.Token).IsEqualTo("new");
        await Assert.That(client.Refreshes).IsEqualTo(1);
        // The failed write is retried once it can succeed.
        await Assert.That(reloaded.Token).IsEqualTo("new");
    }

    [Test]
    public async Task ThrowsAuthorizationExceptionBeforeSignIn() =>
        await Assert
            .That(() => (Task)Repository(new RefreshingClient()).GetAccessToken())
            .Throws<AuthorizationException>();

    [Test]
    public async Task ThrowsOnACallbackWhoseStateDiffersFromTheStoredRequests()
    {
        var repository = Repository(new RefreshingClient());
        await repository.GetUserAuthorizeUri(["user-read-private"]);

        await Assert
            .That(() => (Task)repository.GetNewAccessToken("code", "another state"))
            .Throws<AuthorizationException>();
    }

    [Test]
    [Arguments(null, new[] { "user-read-private" })]
    [Arguments("", new string[0])]
    [Arguments("user-top-read", new[] { "user-top-read" })]
    public async Task StoresTheScopesARefreshGrants(string? scope, string[] expected)
    {
        var repository = await RepositoryWithExpiringToken(
            new RefreshingClient { NewScope = scope }
        );

        var token = await repository.GetAccessToken();

        await Assert.That(token.AuthorizationScopes).IsEquivalentTo(expected);
    }

    [Test]
    [ExcludeOn(OS.Windows)]
    [UnsupportedOSPlatform("windows")]
    public async Task MakesAStoredTokenFileOwnerOnlyWhenLoadingIt()
    {
        var path = Path.Combine(_directory, "AccessToken.json");
        await File.WriteAllTextAsync(
            path,
            JsonSerializer.Serialize(
                new AccessTokenMetadata
                {
                    AuthorizationScopes = [],
                    Token = "token",
                    ExpiresAtUtc = DateTimeOffset.MaxValue,
                    RefreshToken = "refresh",
                }
            )
        );
        File.SetUnixFileMode(path, WorldReadable);

        await Repository(new RefreshingClient()).GetAccessToken();

        await Assert.That(File.GetUnixFileMode(path)).IsEqualTo(OwnerOnly);
    }

    [Test]
    [ExcludeOn(OS.Windows)]
    [UnsupportedOSPlatform("windows")]
    public async Task WritesTokenFilesOwnerOnlyDespiteALeftoverTemporaryFile()
    {
        var repository = await RepositoryWithExpiringToken(new RefreshingClient());
        var leftover = Path.Combine(_directory, "AccessToken.json.tmp");
        await File.WriteAllTextAsync(leftover, "");
        File.SetUnixFileMode(leftover, WorldReadable);

        await repository.GetAccessToken();

        await Assert
            .That(File.GetUnixFileMode(Path.Combine(_directory, "AccessToken.json")))
            .IsEqualTo(OwnerOnly);
    }

    public void Dispose() => Directory.Delete(_directory, recursive: true);

    [UnsupportedOSPlatform("windows")]
    private const UnixFileMode OwnerOnly = UnixFileMode.UserRead | UnixFileMode.UserWrite;

    [UnsupportedOSPlatform("windows")]
    private const UnixFileMode WorldReadable =
        OwnerOnly | UnixFileMode.GroupRead | UnixFileMode.OtherRead;

    private IAuthorizationRepository Repository(IAuthorizationClient client) =>
        new AuthorizationRepository(client, _directory);

    private async Task<IAuthorizationRepository> RepositoryWithExpiringToken(
        IAuthorizationClient client
    )
    {
        var expiring = new AccessTokenMetadata
        {
            AuthorizationScopes = ["user-read-private"],
            Token = "old",
            ExpiresAtUtc = DateTimeOffset.UtcNow,
            RefreshToken = "refresh",
        };
        await File.WriteAllTextAsync(
            Path.Combine(_directory, "AccessToken.json"),
            JsonSerializer.Serialize(expiring)
        );

        return Repository(client);
    }

    private sealed class RefreshingClient : IAuthorizationClient
    {
        private int _refreshes;

        public int Refreshes => _refreshes;

        public Action? DuringRefresh { get; init; }

        public string? NewRefreshToken { get; init; }

        public string? NewScope { get; init; }

        public UserAuthorization GetUserAuthorization(IEnumerable<string> scopes) =>
            new()
            {
                AuthorizationUri = "https://accounts.spotify.com/authorize",
                CodeVerifier = "verifier",
                State = "state",
            };

        public Task<AccessToken> GetUserAccessToken(
            string code,
            string codeVerifier,
            CancellationToken cancellationToken
        ) => throw new NotSupportedException();

        public async Task<AccessToken> RefreshUserAccessToken(
            string refreshToken,
            CancellationToken cancellationToken
        )
        {
            Interlocked.Increment(ref _refreshes);
            DuringRefresh?.Invoke();
            await Task.Delay(50, cancellationToken);

            return new AccessToken
            {
                Token = "new",
                TokenType = "Bearer",
                ExpiresIn = 3600,
                RefreshToken = NewRefreshToken,
                Scope = NewScope,
            };
        }
    }
}
