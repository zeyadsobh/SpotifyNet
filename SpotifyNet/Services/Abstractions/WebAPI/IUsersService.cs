using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SpotifyNet.Clients.Abstractions;
using SpotifyNet.Datastructures.Spotify;
using SpotifyNet.Datastructures.Spotify.Artists;
using SpotifyNet.Datastructures.Spotify.Authorization;
using SpotifyNet.Datastructures.Spotify.Tracks;
using SpotifyNet.Datastructures.Spotify.Users;

namespace SpotifyNet.Services.Abstractions.WebAPI;

/// <summary>The current user's profile, top items and followed artists.</summary>
/// <remarks>
/// Bad arguments throw <see cref="ArgumentNullException"/>, <see cref="ArgumentException"/> or
/// <see cref="ArgumentOutOfRangeException"/> before any request is sent. Requests throw
/// <see cref="AuthorizationException"/> if there's no access token yet or it lacks a required
/// scope, and <see cref="WebAPIException"/> if Spotify answers with an error; paged results do so
/// while they're enumerated.
/// </remarks>
public interface IUsersService
{
    /// <summary>Gets the current user's profile.</summary>
    /// <remarks>
    /// Requires <see cref="AuthorizationScope.UserReadPrivate"/> and
    /// <see cref="AuthorizationScope.UserReadEmail"/>.
    /// </remarks>
    Task<User> GetCurrentUserProfile(CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the current user's top artists or tracks by calculated affinity, a page at a time.
    /// </summary>
    /// <typeparam name="T"><see cref="Artist"/> or <see cref="Track"/>.</typeparam>
    /// <param name="timeRange">Spotify defaults to <see cref="TimeRange.medium_term"/>.</param>
    /// <param name="offset">The index of the first item to return.</param>
    /// <param name="cancellationToken">A token to cancel the request.</param>
    /// <remarks>Requires <see cref="AuthorizationScope.UserTopRead"/>.</remarks>
    IAsyncEnumerable<T> GetTopItems<T>(
        TimeRange? timeRange = null,
        int offset = 0,
        CancellationToken cancellationToken = default
    )
        where T : ITopItem;

    /// <summary>Gets the artists the current user follows, a page at a time.</summary>
    /// <param name="after">The artist ID to start after.</param>
    /// <param name="cancellationToken">A token to cancel the request.</param>
    /// <remarks>Requires <see cref="AuthorizationScope.UserFollowRead"/>.</remarks>
    IAsyncEnumerable<Artist> GetFollowedArtists(
        string? after = null,
        CancellationToken cancellationToken = default
    );
}
