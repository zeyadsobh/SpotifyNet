using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SpotifyNet.Clients.Abstractions;

namespace SpotifyNet.Services.Abstractions;

/// <summary>Keeps the access token, and the sign-in under way, between runs.</summary>
/// <remarks>
/// Requests to Spotify's accounts service throw <see cref="WebAPIException"/> if Spotify refuses
/// them.
/// </remarks>
public interface IAuthorizationRepository
{
    /// <summary>Starts a sign-in, saving its PKCE verifier and state until it finishes.</summary>
    /// <returns>
    /// The URI to open in a browser, asking the user to authorize <paramref name="scopes"/>.
    /// </returns>
    Task<string> GetUserAuthorizeUri(
        IEnumerable<string> scopes,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Finishes the sign-in: exchanges the authorization <paramref name="code"/> from the redirect
    /// for an access token, and stores it.
    /// </summary>
    /// <exception cref="AuthorizationException">
    /// <paramref name="state"/> isn't the one the sign-in started with, or Spotify returned no
    /// refresh token.
    /// </exception>
    Task<AccessTokenMetadata> GetNewAccessToken(
        string code,
        string state,
        CancellationToken cancellationToken = default
    );

    /// <summary>Gets the stored access token, refreshing it first if it's expiring.</summary>
    /// <exception cref="AuthorizationException">There's no access token yet.</exception>
    Task<AccessTokenMetadata> GetAccessToken(CancellationToken cancellationToken = default);

    /// <summary>The stored token as is, without refreshing it; null before the first sign-in.</summary>
    Task<AccessTokenMetadata?> GetStoredAccessToken(CancellationToken cancellationToken = default);
}
