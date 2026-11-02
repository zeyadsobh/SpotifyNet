using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SpotifyNet.Datastructures.Spotify.Authorization;

namespace SpotifyNet.Clients.Abstractions;

public interface IAuthorizationClient
{
    /// <summary>Builds the authorize URI, with its PKCE verifier and state, without a request.</summary>
    UserAuthorization GetUserAuthorization(IEnumerable<string> scopes);

    Task<AccessToken> GetUserAccessToken(
        string code,
        string codeVerifier,
        CancellationToken cancellationToken = default
    );

    Task<AccessToken> RefreshUserAccessToken(
        string refreshToken,
        CancellationToken cancellationToken = default
    );
}
