using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SpotifyNet.Datastructures.Spotify;
using SpotifyNet.Datastructures.Spotify.Artists;
using SpotifyNet.Datastructures.Spotify.Tracks;
using SpotifyNet.Datastructures.Spotify.Users;

namespace SpotifyNet.Services.Abstractions.WebAPI;

public interface IUsersService
{
    Task<User> GetCurrentUserProfile(CancellationToken cancellationToken = default);

    /// <typeparam name="T"><see cref="Artist"/> or <see cref="Track"/>.</typeparam>
    /// <param name="timeRange">Spotify defaults to <see cref="TimeRange.medium_term"/>.</param>
    IAsyncEnumerable<T> GetTopItems<T>(
        TimeRange? timeRange = null,
        int offset = 0,
        CancellationToken cancellationToken = default
    )
        where T : ITopItem;

    /// <param name="after">The artist ID to start after.</param>
    IAsyncEnumerable<Artist> GetFollowedArtists(
        string? after = null,
        CancellationToken cancellationToken = default
    );
}
