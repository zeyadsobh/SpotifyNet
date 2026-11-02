using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SpotifyNet.Datastructures.Spotify.Tracks;

namespace SpotifyNet.Services.Abstractions.WebAPI;

public interface ITracksService
{
    Task<Track> GetTrack(string trackId, CancellationToken cancellationToken = default);

    IAsyncEnumerable<SavedTrack> GetUserSavedTracks(
        int offset = 0,
        CancellationToken cancellationToken = default
    );
}
