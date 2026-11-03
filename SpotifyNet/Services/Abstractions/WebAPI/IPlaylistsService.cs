using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SpotifyNet.Clients.Abstractions;
using SpotifyNet.Datastructures.Spotify;
using SpotifyNet.Datastructures.Spotify.Authorization;
using SpotifyNet.Datastructures.Spotify.Playlists;

namespace SpotifyNet.Services.Abstractions.WebAPI;

/// <summary>Playlists, their items and cover images, and the current user's playlists.</summary>
/// <remarks>
/// Bad arguments throw <see cref="ArgumentNullException"/>, <see cref="ArgumentException"/> or
/// <see cref="ArgumentOutOfRangeException"/> before any request is sent. Requests throw
/// <see cref="AuthorizationException"/> if there's no access token yet or it lacks a required
/// scope, and <see cref="WebAPIException"/> if Spotify answers with an error; paged results do so
/// while they're enumerated.
/// </remarks>
public interface IPlaylistsService
{
    /// <summary>
    /// Gets a playlist, with a first page of its items if the current user owns it or collaborates
    /// on it.
    /// </summary>
    Task<Playlist> GetPlaylist(string playlistId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Changes only the details that aren't <see langword="null"/>. The user must own the playlist,
    /// and only a non-public playlist can be collaborative.
    /// </summary>
    /// <remarks>
    /// Requires <see cref="AuthorizationScope.PlaylistModifyPublic"/> and
    /// <see cref="AuthorizationScope.PlaylistModifyPrivate"/>.
    /// </remarks>
    Task ChangePlaylistDetails(
        string playlistId,
        string? name = null,
        bool? isPublic = null,
        bool? collaborative = null,
        string? description = null,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Gets a playlist's items, a page at a time. Only works for playlists the current user owns or
    /// collaborates on.
    /// </summary>
    /// <remarks>Requires <see cref="AuthorizationScope.PlaylistReadPrivate"/>.</remarks>
    IAsyncEnumerable<PlaylistTrack> GetPlaylistItems(
        string playlistId,
        int offset = 0,
        CancellationToken cancellationToken = default
    );

    /// <summary>Adds items to a playlist.</summary>
    /// <param name="playlistId">The playlist's Spotify ID.</param>
    /// <param name="uris">1 to 100 track or episode URIs.</param>
    /// <param name="position">Where to insert them; appended if <see langword="null"/>.</param>
    /// <param name="cancellationToken">A token to cancel the request.</param>
    /// <returns>The playlist's new snapshot ID.</returns>
    /// <remarks>
    /// Requires <see cref="AuthorizationScope.PlaylistModifyPublic"/> and
    /// <see cref="AuthorizationScope.PlaylistModifyPrivate"/>.
    /// </remarks>
    Task<string?> AddPlaylistItems(
        string playlistId,
        IEnumerable<string> uris,
        int? position = null,
        CancellationToken cancellationToken = default
    );

    /// <summary>Moves <paramref name="rangeLength"/> items (1 if <see langword="null"/>) from <paramref name="rangeStart"/> to before <paramref name="insertBefore"/>.</summary>
    /// <returns>The playlist's new snapshot ID.</returns>
    /// <remarks>
    /// Requires <see cref="AuthorizationScope.PlaylistModifyPublic"/> and
    /// <see cref="AuthorizationScope.PlaylistModifyPrivate"/>.
    /// </remarks>
    Task<string?> ReorderPlaylistItems(
        string playlistId,
        int rangeStart,
        int insertBefore,
        int? rangeLength = null,
        string? snapshotId = null,
        CancellationToken cancellationToken = default
    );

    /// <summary>Replaces all of a playlist's items with <paramref name="uris"/>.</summary>
    /// <param name="playlistId">The playlist's Spotify ID.</param>
    /// <param name="uris">Up to 100 track or episode URIs; none clears the playlist.</param>
    /// <param name="cancellationToken">A token to cancel the request.</param>
    /// <returns>The playlist's new snapshot ID.</returns>
    /// <remarks>
    /// Requires <see cref="AuthorizationScope.PlaylistModifyPublic"/> and
    /// <see cref="AuthorizationScope.PlaylistModifyPrivate"/>.
    /// </remarks>
    Task<string?> ReplacePlaylistItems(
        string playlistId,
        IEnumerable<string> uris,
        CancellationToken cancellationToken = default
    );

    /// <summary>Removes items from a playlist.</summary>
    /// <param name="playlistId">The playlist's Spotify ID.</param>
    /// <param name="uris">1 to 100 track or episode URIs; every occurrence is removed.</param>
    /// <param name="snapshotId">
    /// The snapshot to remove from, even if the playlist has changed since; the latest if
    /// <see langword="null"/>.
    /// </param>
    /// <param name="cancellationToken">A token to cancel the request.</param>
    /// <returns>The playlist's new snapshot ID.</returns>
    /// <remarks>
    /// Requires <see cref="AuthorizationScope.PlaylistModifyPublic"/> and
    /// <see cref="AuthorizationScope.PlaylistModifyPrivate"/>.
    /// </remarks>
    Task<string?> RemovePlaylistItems(
        string playlistId,
        IEnumerable<string> uris,
        string? snapshotId = null,
        CancellationToken cancellationToken = default
    );

    /// <summary>Gets the playlists the current user owns or follows, a page at a time.</summary>
    /// <remarks>Requires <see cref="AuthorizationScope.PlaylistReadPrivate"/>.</remarks>
    IAsyncEnumerable<SimplifiedPlaylist> GetCurrentUserPlaylists(
        int offset = 0,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Creates an empty playlist for the current user, public unless <paramref name="isPublic"/> is
    /// <see langword="false"/>. Only a non-public playlist can be collaborative.
    /// </summary>
    /// <remarks>
    /// Requires <see cref="AuthorizationScope.PlaylistModifyPublic"/> and
    /// <see cref="AuthorizationScope.PlaylistModifyPrivate"/>.
    /// </remarks>
    Task<Playlist> CreatePlaylist(
        string name,
        bool? isPublic = null,
        bool? collaborative = null,
        string? description = null,
        CancellationToken cancellationToken = default
    );

    /// <summary>Gets a playlist's current cover image, in its available sizes.</summary>
    Task<IReadOnlyList<Image>> GetPlaylistCoverImage(
        string playlistId,
        CancellationToken cancellationToken = default
    );

    /// <summary>Replaces a playlist's cover image with a custom one.</summary>
    /// <param name="playlistId">The playlist's Spotify ID.</param>
    /// <param name="base64Image">The Base64-encoded JPEG data, at most 256 KB.</param>
    /// <param name="cancellationToken">A token to cancel the request.</param>
    /// <remarks>
    /// Requires <see cref="AuthorizationScope.UgcImageUpload"/>,
    /// <see cref="AuthorizationScope.PlaylistModifyPublic"/> and
    /// <see cref="AuthorizationScope.PlaylistModifyPrivate"/>.
    /// </remarks>
    Task AddCustomPlaylistCoverImage(
        string playlistId,
        string base64Image,
        CancellationToken cancellationToken = default
    );
}
