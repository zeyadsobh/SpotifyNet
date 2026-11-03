using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Extensions.Options;
using SpotifyNet.Clients.Abstractions;
using SpotifyNet.Clients.Options;
using SpotifyNet.Datastructures.Spotify.Authorization;

namespace SpotifyNet.Clients;

/// <summary>
/// The <see cref="IAuthorizationClient"/> for Spotify's accounts service, set up by
/// <see cref="AuthorizationClientOptions"/>.
/// </summary>
/// <remarks>
/// Creating it throws <see cref="ArgumentException"/> if the options have no client ID, or a
/// redirect URI that isn't an absolute URI.
/// </remarks>
public sealed class AuthorizationClient(
    IOptions<AuthorizationClientOptions> options,
    HttpClient httpClient
) : IAuthorizationClient
{
    private const string AuthorizeEndpoint = "https://accounts.spotify.com/authorize";
    private const string TokenEndpoint = "https://accounts.spotify.com/api/token";

    private readonly AuthorizationClientOptions _options = Validate(options.Value);

    /// <inheritdoc />
    public UserAuthorization GetUserAuthorization(IEnumerable<string> scopes)
    {
        ArgumentNullException.ThrowIfNull(scopes);

        var scopesCollection = scopes as ICollection<string> ?? scopes.ToList();
        var invalidScopes = scopesCollection.Except(AuthorizationScope.ValidScopes).ToList();

        if (invalidScopes.Count != 0)
        {
            throw new ArgumentException(
                $"Invalid authorization scopes `{string.Join(", ", invalidScopes)}`.",
                nameof(scopes)
            );
        }

        var (verifier, challenge) = PKCE.GetCodeVerifierAndChallenge(verifierLength: 64);
        var state = Guid.NewGuid().ToString();

        var parameters = new NameValueCollection
        {
            ["client_id"] = _options.AppClientId,
            ["response_type"] = "code",
            ["redirect_uri"] = _options.AppRedirectUri,
            ["state"] = state,
            ["scope"] = string.Join(' ', scopesCollection),
            ["show_dialog"] = "false",
            ["code_challenge_method"] = "S256",
            ["code_challenge"] = challenge,
        };

        var builder = new UriBuilder(AuthorizeEndpoint);
        var queryParameters = HttpUtility.ParseQueryString(builder.Query);
        queryParameters.Add(parameters);
        builder.Query = queryParameters.ToString();

        return new UserAuthorization
        {
            AuthorizationUri = builder.Uri.AbsoluteUri,
            CodeVerifier = verifier,
            State = state,
        };
    }

    /// <inheritdoc />
    public Task<AccessToken> GetUserAccessToken(
        string code,
        string codeVerifier,
        CancellationToken cancellationToken
    )
    {
        var payload = new Dictionary<string, string>
        {
            ["grant_type"] = "authorization_code",
            ["code"] = code,
            ["redirect_uri"] = _options.AppRedirectUri,
            ["client_id"] = _options.AppClientId,
            ["code_verifier"] = codeVerifier,
        };

        return PostForAccessToken(payload, cancellationToken);
    }

    /// <inheritdoc />
    public Task<AccessToken> RefreshUserAccessToken(
        string refreshToken,
        CancellationToken cancellationToken
    )
    {
        var payload = new Dictionary<string, string>
        {
            ["grant_type"] = "refresh_token",
            ["refresh_token"] = refreshToken,
            ["client_id"] = _options.AppClientId,
        };

        return PostForAccessToken(payload, cancellationToken);
    }

    // Failures throw WebAPIException, as for API calls.
    private async Task<AccessToken> PostForAccessToken(
        IReadOnlyDictionary<string, string> payload,
        CancellationToken cancellationToken
    )
    {
        using var content = new FormUrlEncodedContent(payload);

        using var response = await httpClient.PostAsync(TokenEndpoint, content, cancellationToken);
        var body = await response.Content.ReadAsStringAsync(cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            throw new WebAPIException(
                TokenEndpoint,
                response.StatusCode,
                OAuthErrorMessage(body),
                body
            );
        }

        try
        {
            return JsonSerializer.Deserialize<AccessToken>(body, JsonSerializerOptions.Web)
                ?? throw new JsonException("The token response is null.");
        }
        catch (JsonException e)
        {
            // The body may hold live tokens, so keep it out of the exception.
            throw new WebAPIException(
                TokenEndpoint,
                response.StatusCode,
                "(withheld: it may hold tokens)",
                e
            );
        }
    }

    // OAuth errors look like {"error":"invalid_grant","error_description":"Refresh token revoked"}.
    private static string? OAuthErrorMessage(string body)
    {
        try
        {
            var error = JsonNode.Parse(body);

            return (string?)error?["error_description"] ?? (string?)error?["error"];
        }
        catch (Exception e) when (e is JsonException or InvalidOperationException)
        {
            return null;
        }
    }

    // Binding configuration doesn't enforce `required`, and bad settings would only surface
    // mid-sign-in, e.g. as an authorize page that never redirects back.
    private static AuthorizationClientOptions Validate(AuthorizationClientOptions options)
    {
        if (string.IsNullOrWhiteSpace(options.AppClientId))
        {
            throw new ArgumentException(
                $"{nameof(options.AppClientId)} isn't set.",
                nameof(options)
            );
        }

        // Apps may use custom schemes, but not files: Unix reads "/callback" as file:///callback.
        if (
            !Uri.TryCreate(options.AppRedirectUri, UriKind.Absolute, out var redirectUri)
            || redirectUri.IsFile
        )
        {
            throw new ArgumentException(
                $"{nameof(options.AppRedirectUri)} isn't an absolute URI.",
                nameof(options)
            );
        }

        return options;
    }
}
