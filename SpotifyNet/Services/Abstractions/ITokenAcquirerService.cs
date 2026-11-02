using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SpotifyNet.Services.Abstractions;

public interface ITokenAcquirerService
{
    /// <summary>
    /// Signs in through the browser unless the stored token has <paramref name="scopes"/>. Scopes
    /// granted earlier are asked for again, so they're kept; <paramref name="forceGenerate"/>
    /// signs in afresh with just <paramref name="scopes"/>.
    /// </summary>
    Task EnsureTokenExists(
        IEnumerable<string> scopes,
        bool forceGenerate = false,
        CancellationToken cancellationToken = default
    );
}
