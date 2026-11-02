using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SpotifyNet.Services.Abstractions;

public interface IAuthorizationRepository
{
    Task<string> GetUserAuthorizeUri(
        IEnumerable<string> scopes,
        CancellationToken cancellationToken = default
    );

    Task<AccessTokenMetadata> GetNewAccessToken(
        string code,
        string state,
        CancellationToken cancellationToken = default
    );

    Task<AccessTokenMetadata> GetAccessToken(CancellationToken cancellationToken = default);

    /// <summary>The stored token as is, without refreshing it; null before the first sign-in.</summary>
    Task<AccessTokenMetadata?> GetStoredAccessToken(CancellationToken cancellationToken = default);
}
