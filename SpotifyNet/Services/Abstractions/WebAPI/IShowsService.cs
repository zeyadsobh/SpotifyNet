using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SpotifyNet.Datastructures.Spotify.Episodes;
using SpotifyNet.Datastructures.Spotify.Shows;

namespace SpotifyNet.Services.Abstractions.WebAPI;

public interface IShowsService
{
    Task<Show> GetShow(string showId, CancellationToken cancellationToken = default);

    IAsyncEnumerable<SimplifiedEpisode> GetEpisodes(
        string showId,
        int offset = 0,
        CancellationToken cancellationToken = default
    );

    IAsyncEnumerable<SavedShow> GetUserSavedShows(
        int offset = 0,
        CancellationToken cancellationToken = default
    );
}
