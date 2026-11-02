using System.Threading;
using System.Threading.Tasks;

namespace SpotifyNet.Clients.Abstractions;

/// <summary>Gets a valid access token. Called before every request, so long enumerations keep working as tokens refresh.</summary>
public delegate Task<string> AccessTokenProvider(CancellationToken cancellationToken);
