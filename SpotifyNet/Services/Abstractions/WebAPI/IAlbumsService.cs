using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SpotifyNet.Datastructures.Spotify.Albums;
using SpotifyNet.Datastructures.Spotify.Tracks;

namespace SpotifyNet.Services.Abstractions.WebAPI;

public interface IAlbumsService
{
    Task<Album> GetAlbum(string albumId, CancellationToken cancellationToken = default);
    IAsyncEnumerable<SimplifiedTrack> GetTracks(
        string albumId,
        int offset = 0,
        CancellationToken cancellationToken = default
    );
    IAsyncEnumerable<SavedAlbum> GetUserSavedAlbums(
        int offset = 0,
        CancellationToken cancellationToken = default
    );
}
