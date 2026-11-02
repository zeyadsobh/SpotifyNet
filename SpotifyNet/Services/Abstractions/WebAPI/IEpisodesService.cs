using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SpotifyNet.Datastructures.Spotify.Episodes;

namespace SpotifyNet.Services.Abstractions.WebAPI;

public interface IEpisodesService
{
    Task<Episode> GetEpisode(string episodeId, CancellationToken cancellationToken = default);

    IAsyncEnumerable<SavedEpisode> GetUserSavedEpisodes(
        int offset = 0,
        CancellationToken cancellationToken = default
    );
}
