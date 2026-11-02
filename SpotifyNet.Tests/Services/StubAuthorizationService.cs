using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SpotifyNet.Services.Abstractions;

namespace SpotifyNet.Tests.Services;

/// <summary>Grants every scope, recording the scopes each token is asked for.</summary>
internal sealed class StubAuthorizationService : IAuthorizationService
{
    public List<string[]> Requested { get; } = [];

    public Task<string> GetAccessToken(
        IEnumerable<string> scopes,
        CancellationToken cancellationToken
    )
    {
        Requested.Add([.. scopes]);

        return Task.FromResult("token");
    }
}
