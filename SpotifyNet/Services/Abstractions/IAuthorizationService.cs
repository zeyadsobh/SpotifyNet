using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SpotifyNet.Clients.Abstractions;

namespace SpotifyNet.Services.Abstractions;

public interface IAuthorizationService
{
    Task<string> GetAccessToken(
        IEnumerable<string> scopes,
        CancellationToken cancellationToken = default
    );

    /// <summary>A provider that checks the token has <paramref name="scopes"/> on every request.</summary>
    AccessTokenProvider GetAccessToken(params string[] scopes) =>
        cancellationToken => GetAccessToken(scopes, cancellationToken);
}
