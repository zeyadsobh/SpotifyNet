using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SpotifyNet.Datastructures.Spotify;
using SpotifyNet.Datastructures.Spotify.Playlists;

namespace SpotifyNet.Services.Abstractions.WebAPI;

public interface IPlaylistsService
{
    Task<Playlist> GetPlaylist(string playlistId, CancellationToken cancellationToken = default);

    /// <summary>Changes only the details that aren't <see langword="null"/>.</summary>
    Task ChangePlaylistDetails(
        string playlistId,
        string? name = null,
        bool? isPublic = null,
        bool? collaborative = null,
        string? description = null,
        CancellationToken cancellationToken = default
    );

    IAsyncEnumerable<PlaylistTrack> GetPlaylistItems(
        string playlistId,
        int offset = 0,
        CancellationToken cancellationToken = default
    );

    /// <param name="uris">1 to 100 track or episode URIs.</param>
    /// <param name="position">Where to insert them; appended if <see langword="null"/>.</param>
    /// <returns>The playlist's new snapshot ID.</returns>
    Task<string?> AddPlaylistItems(
        string playlistId,
        IEnumerable<string> uris,
        int? position = null,
        CancellationToken cancellationToken = default
    );

    /// <summary>Moves <paramref name="rangeLength"/> items (1 if <see langword="null"/>) from <paramref name="rangeStart"/> to before <paramref name="insertBefore"/>.</summary>
    /// <returns>The playlist's new snapshot ID.</returns>
    Task<string?> ReorderPlaylistItems(
        string playlistId,
        int rangeStart,
        int insertBefore,
        int? rangeLength = null,
        string? snapshotId = null,
        CancellationToken cancellationToken = default
    );

    /// <param name="uris">Up to 100 track or episode URIs; none clears the playlist.</param>
    /// <returns>The playlist's new snapshot ID.</returns>
    Task<string?> ReplacePlaylistItems(
        string playlistId,
        IEnumerable<string> uris,
        CancellationToken cancellationToken = default
    );

    /// <param name="uris">1 to 100 track or episode URIs; every occurrence is removed.</param>
    /// <returns>The playlist's new snapshot ID.</returns>
    Task<string?> RemovePlaylistItems(
        string playlistId,
        IEnumerable<string> uris,
        string? snapshotId = null,
        CancellationToken cancellationToken = default
    );

    IAsyncEnumerable<SimplifiedPlaylist> GetCurrentUserPlaylists(
        int offset = 0,
        CancellationToken cancellationToken = default
    );

    Task<Playlist> CreatePlaylist(
        string name,
        bool? isPublic = null,
        bool? collaborative = null,
        string? description = null,
        CancellationToken cancellationToken = default
    );

    Task<IReadOnlyList<Image>> GetPlaylistCoverImage(
        string playlistId,
        CancellationToken cancellationToken = default
    );

    Task AddCustomPlaylistCoverImage(
        string playlistId,
        string base64Image,
        CancellationToken cancellationToken = default
    );
}
