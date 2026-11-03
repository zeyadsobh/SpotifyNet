using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SpotifyNet.Datastructures.Spotify.Authorization;

namespace SpotifyNet.Clients.Abstractions;

/// <summary>
/// Spotify's accounts service: the PKCE authorization flow and its token requests.
/// </summary>
/// <remarks>
/// Token requests throw <see cref="WebAPIException"/> if Spotify refuses them (say, for a used
/// code or a revoked refresh token) or answers with something that can't be read.
/// </remarks>
public interface IAuthorizationClient
{
    /// <summary>Builds the authorize URI, with its PKCE verifier and state, without a request.</summary>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="scopes"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// A scope isn't one of <see cref="AuthorizationScope.ValidScopes"/>.
    /// </exception>
    UserAuthorization GetUserAuthorization(IEnumerable<string> scopes);

    /// <summary>
    /// Exchanges the authorization code from the redirect for an access token, proving it with the
    /// verifier from <see cref="GetUserAuthorization"/>.
    /// </summary>
    Task<AccessToken> GetUserAccessToken(
        string code,
        string codeVerifier,
        CancellationToken cancellationToken = default
    );

    /// <summary>Exchanges a refresh token for a new access token.</summary>
    Task<AccessToken> RefreshUserAccessToken(
        string refreshToken,
        CancellationToken cancellationToken = default
    );
}
