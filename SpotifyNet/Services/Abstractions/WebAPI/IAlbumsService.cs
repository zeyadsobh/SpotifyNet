using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SpotifyNet.Clients.Abstractions;
using SpotifyNet.Datastructures.Spotify.Albums;
using SpotifyNet.Datastructures.Spotify.Authorization;
using SpotifyNet.Datastructures.Spotify.Tracks;

namespace SpotifyNet.Services.Abstractions.WebAPI;

/// <summary>Albums, their tracks and the current user's saved albums.</summary>
/// <remarks>
/// Bad arguments throw <see cref="ArgumentNullException"/>, <see cref="ArgumentException"/> or
/// <see cref="ArgumentOutOfRangeException"/> before any request is sent. Requests throw
/// <see cref="AuthorizationException"/> if there's no access token yet or it lacks a required
/// scope, and <see cref="WebAPIException"/> if Spotify answers with an error; paged results do so
/// while they're enumerated.
/// </remarks>
public interface IAlbumsService
{
    /// <summary>Gets an album's catalog information.</summary>
    Task<Album> GetAlbum(string albumId, CancellationToken cancellationToken = default);

    /// <summary>Gets an album's tracks, a page at a time.</summary>
    IAsyncEnumerable<SimplifiedTrack> GetTracks(
        string albumId,
        int offset = 0,
        CancellationToken cancellationToken = default
    );

    /// <summary>Gets the albums saved in the current user's library, a page at a time.</summary>
    /// <remarks>Requires <see cref="AuthorizationScope.UserLibraryRead"/>.</remarks>
    IAsyncEnumerable<SavedAlbum> GetUserSavedAlbums(
        int offset = 0,
        CancellationToken cancellationToken = default
    );
}
