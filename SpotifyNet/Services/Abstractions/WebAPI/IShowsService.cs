using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SpotifyNet.Clients.Abstractions;
using SpotifyNet.Datastructures.Spotify.Authorization;
using SpotifyNet.Datastructures.Spotify.Episodes;
using SpotifyNet.Datastructures.Spotify.Shows;

namespace SpotifyNet.Services.Abstractions.WebAPI;

/// <summary>Podcast shows, their episodes and the current user's saved shows.</summary>
/// <remarks>
/// Bad arguments throw <see cref="ArgumentNullException"/>, <see cref="ArgumentException"/> or
/// <see cref="ArgumentOutOfRangeException"/> before any request is sent. Requests throw
/// <see cref="AuthorizationException"/> if there's no access token yet or it lacks a required
/// scope, and <see cref="WebAPIException"/> if Spotify answers with an error; paged results do so
/// while they're enumerated.
/// </remarks>
public interface IShowsService
{
    /// <summary>Gets a show's catalog information.</summary>
    /// <remarks>Requires <see cref="AuthorizationScope.UserReadPlaybackPosition"/>.</remarks>
    Task<Show> GetShow(string showId, CancellationToken cancellationToken = default);

    /// <summary>Gets a show's episodes, a page at a time.</summary>
    /// <remarks>Requires <see cref="AuthorizationScope.UserReadPlaybackPosition"/>.</remarks>
    IAsyncEnumerable<SimplifiedEpisode> GetEpisodes(
        string showId,
        int offset = 0,
        CancellationToken cancellationToken = default
    );

    /// <summary>Gets the shows saved in the current user's library, a page at a time.</summary>
    /// <remarks>Requires <see cref="AuthorizationScope.UserLibraryRead"/>.</remarks>
    IAsyncEnumerable<SavedShow> GetUserSavedShows(
        int offset = 0,
        CancellationToken cancellationToken = default
    );
}
