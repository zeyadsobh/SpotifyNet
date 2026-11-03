using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SpotifyNet.Clients.Abstractions;
using SpotifyNet.Datastructures.Spotify.Authorization;
using SpotifyNet.Datastructures.Spotify.Episodes;

namespace SpotifyNet.Services.Abstractions.WebAPI;

/// <summary>Podcast episodes and the current user's saved episodes.</summary>
/// <remarks>
/// Bad arguments throw <see cref="ArgumentNullException"/>, <see cref="ArgumentException"/> or
/// <see cref="ArgumentOutOfRangeException"/> before any request is sent. Requests throw
/// <see cref="AuthorizationException"/> if there's no access token yet or it lacks a required
/// scope, and <see cref="WebAPIException"/> if Spotify answers with an error; paged results do so
/// while they're enumerated.
/// </remarks>
public interface IEpisodesService
{
    /// <summary>Gets an episode's catalog information.</summary>
    /// <remarks>Requires <see cref="AuthorizationScope.UserReadPlaybackPosition"/>.</remarks>
    Task<Episode> GetEpisode(string episodeId, CancellationToken cancellationToken = default);

    /// <summary>Gets the episodes saved in the current user's library, a page at a time.</summary>
    /// <remarks>
    /// Requires <see cref="AuthorizationScope.UserLibraryRead"/> and
    /// <see cref="AuthorizationScope.UserReadPlaybackPosition"/>.
    /// </remarks>
    IAsyncEnumerable<SavedEpisode> GetUserSavedEpisodes(
        int offset = 0,
        CancellationToken cancellationToken = default
    );
}
