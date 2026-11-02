using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SpotifyNet.Services.Abstractions.WebAPI;

/// <summary>Saving tracks, albums, episodes, shows, audiobooks, users and playlists by URI.</summary>
public interface ILibraryService
{
    /// <param name="uris">1 to 40 URIs.</param>
    Task SaveItems(IEnumerable<string> uris, CancellationToken cancellationToken = default);

    /// <param name="uris">1 to 40 URIs.</param>
    Task RemoveItems(IEnumerable<string> uris, CancellationToken cancellationToken = default);

    /// <param name="uris">Any number of URIs; artists are accepted too. Checked 40 at a time.</param>
    /// <returns>Whether each URI is saved, in input order.</returns>
    IAsyncEnumerable<bool> CheckSavedItems(
        IEnumerable<string> uris,
        CancellationToken cancellationToken = default
    );
}
