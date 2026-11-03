using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SpotifyNet.Clients.Abstractions;
using SpotifyNet.Datastructures.Spotify.Authorization;

namespace SpotifyNet.Services.Abstractions.WebAPI;

/// <summary>Saving tracks, albums, episodes, shows, audiobooks, users and playlists by URI.</summary>
/// <remarks>
/// Bad arguments throw <see cref="ArgumentNullException"/>, <see cref="ArgumentException"/> or
/// <see cref="ArgumentOutOfRangeException"/> before any request is sent, except as noted on
/// <see cref="CheckSavedItems"/>. Requests throw <see cref="AuthorizationException"/> if there's no
/// access token yet or it lacks a required scope, and <see cref="WebAPIException"/> if Spotify
/// answers with an error; <see cref="CheckSavedItems"/> does so while it's enumerated.
/// </remarks>
public interface ILibraryService
{
    /// <summary>Saves items to the current user's library.</summary>
    /// <param name="uris">1 to 40 URIs.</param>
    /// <param name="cancellationToken">A token to cancel the request.</param>
    /// <remarks>
    /// Requires <see cref="AuthorizationScope.UserLibraryModify"/>,
    /// <see cref="AuthorizationScope.UserFollowModify"/> and
    /// <see cref="AuthorizationScope.PlaylistModifyPublic"/>.
    /// </remarks>
    Task SaveItems(IEnumerable<string> uris, CancellationToken cancellationToken = default);

    /// <summary>Removes items from the current user's library.</summary>
    /// <param name="uris">1 to 40 URIs.</param>
    /// <param name="cancellationToken">A token to cancel the request.</param>
    /// <remarks>
    /// Requires <see cref="AuthorizationScope.UserLibraryModify"/>,
    /// <see cref="AuthorizationScope.UserFollowModify"/> and
    /// <see cref="AuthorizationScope.PlaylistModifyPublic"/>.
    /// </remarks>
    Task RemoveItems(IEnumerable<string> uris, CancellationToken cancellationToken = default);

    /// <summary>Checks whether items are saved in the current user's library.</summary>
    /// <param name="uris">Any number of URIs; artists are accepted too. Checked 40 at a time.</param>
    /// <param name="cancellationToken">A token to cancel the request.</param>
    /// <returns>Whether each URI is saved, in input order.</returns>
    /// <remarks>
    /// Requires <see cref="AuthorizationScope.UserLibraryRead"/>,
    /// <see cref="AuthorizationScope.UserFollowRead"/> and
    /// <see cref="AuthorizationScope.PlaylistReadPrivate"/>. Arguments are checked while
    /// enumerating, a chunk at a time, so a <see langword="null"/> URI only surfaces after the
    /// chunks before it have been requested.
    /// </remarks>
    IAsyncEnumerable<bool> CheckSavedItems(
        IEnumerable<string> uris,
        CancellationToken cancellationToken = default
    );
}
