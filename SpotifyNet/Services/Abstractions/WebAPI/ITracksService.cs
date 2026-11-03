using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SpotifyNet.Clients.Abstractions;
using SpotifyNet.Datastructures.Spotify.Authorization;
using SpotifyNet.Datastructures.Spotify.Tracks;

namespace SpotifyNet.Services.Abstractions.WebAPI;

/// <summary>Tracks and the current user's saved tracks.</summary>
/// <remarks>
/// Bad arguments throw <see cref="ArgumentNullException"/>, <see cref="ArgumentException"/> or
/// <see cref="ArgumentOutOfRangeException"/> before any request is sent. Requests throw
/// <see cref="AuthorizationException"/> if there's no access token yet or it lacks a required
/// scope, and <see cref="WebAPIException"/> if Spotify answers with an error; paged results do so
/// while they're enumerated.
/// </remarks>
public interface ITracksService
{
    /// <summary>Gets a track's catalog information.</summary>
    Task<Track> GetTrack(string trackId, CancellationToken cancellationToken = default);

    /// <summary>Gets the tracks saved in the current user's library, a page at a time.</summary>
    /// <remarks>Requires <see cref="AuthorizationScope.UserLibraryRead"/>.</remarks>
    IAsyncEnumerable<SavedTrack> GetUserSavedTracks(
        int offset = 0,
        CancellationToken cancellationToken = default
    );
}
