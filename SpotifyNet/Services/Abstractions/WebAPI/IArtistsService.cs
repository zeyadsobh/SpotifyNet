using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SpotifyNet.Datastructures.Spotify.Albums;
using SpotifyNet.Datastructures.Spotify.Artists;

namespace SpotifyNet.Services.Abstractions.WebAPI;

public interface IArtistsService
{
    Task<Artist> GetArtist(string artistId, CancellationToken cancellationToken = default);

    /// <param name="includeGroups">Only these groups; all of them if <see langword="null"/>.</param>
    IAsyncEnumerable<SimplifiedAlbum> GetAlbums(
        string artistId,
        IEnumerable<AlbumGroup>? includeGroups = null,
        int offset = 0,
        CancellationToken cancellationToken = default
    );
}
