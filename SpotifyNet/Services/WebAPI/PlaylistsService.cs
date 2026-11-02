using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading;
using System.Threading.Tasks;
using SpotifyNet.Clients.Abstractions;
using SpotifyNet.Datastructures.Spotify;
using SpotifyNet.Datastructures.Spotify.Authorization;
using SpotifyNet.Datastructures.Spotify.Playlists;
using SpotifyNet.Services.Abstractions;
using SpotifyNet.Services.Abstractions.WebAPI;
using static SpotifyNet.Services.WebAPI.WebAPIClientExtensions;

namespace SpotifyNet.Services.WebAPI;

internal sealed class PlaylistsService(
    IAuthorizationService authorizationService,
    IWebAPIClient webAPIClient
) : IPlaylistsService
{
    private const int MaxPlaylistItems = 100;

    public Task<Playlist> GetPlaylist(string playlistId, CancellationToken cancellationToken) =>
        webAPIClient.GetAsync<Playlist>(
            Endpoints.GetPlaylist(playlistId),
            authorizationService.GetAccessToken(),
            cancellationToken
        );

    public Task ChangePlaylistDetails(
        string playlistId,
        string? name,
        bool? isPublic,
        bool? collaborative,
        string? description,
        CancellationToken cancellationToken
    ) =>
        webAPIClient.PutAsync(
            Endpoints.ChangePlaylistDetails(playlistId),
            new
            {
                name,
                @public = isPublic,
                collaborative,
                description,
            },
            ModifyAccessToken,
            cancellationToken
        );

    public IAsyncEnumerable<PlaylistTrack> GetPlaylistItems(
        string playlistId,
        int offset,
        CancellationToken cancellationToken
    ) =>
        webAPIClient.GetOffsetPaginated<PlaylistTrack>(
            Endpoints.GetPlaylistItems(playlistId, offset),
            authorizationService.GetAccessToken(AuthorizationScope.PlaylistReadPrivate),
            cancellationToken
        );

    public async Task<string?> AddPlaylistItems(
        string playlistId,
        IEnumerable<string> uris,
        int? position,
        CancellationToken cancellationToken
    )
    {
        ThrowIfNegative(position);

        var body = new { uris = Uris(uris, 1, MaxPlaylistItems), position };

        var snapshot = await webAPIClient.PostAsync<object, PlaylistSnapshot>(
            Endpoints.AddItemsToPlaylist(playlistId),
            body,
            ModifyAccessToken,
            cancellationToken
        );

        return snapshot.SnapshotId;
    }

    public async Task<string?> ReorderPlaylistItems(
        string playlistId,
        int rangeStart,
        int insertBefore,
        int? rangeLength,
        string? snapshotId,
        CancellationToken cancellationToken
    )
    {
        ThrowIfNegative(rangeStart);
        ThrowIfNegative(insertBefore);
        ThrowIfNegative(rangeLength);

        var body = new
        {
            range_start = rangeStart,
            insert_before = insertBefore,
            range_length = rangeLength,
            snapshot_id = snapshotId,
        };

        var snapshot = await webAPIClient.PutAsync<object, PlaylistSnapshot>(
            Endpoints.UpdatePlaylistItems(playlistId),
            body,
            ModifyAccessToken,
            cancellationToken
        );

        return snapshot.SnapshotId;
    }

    public async Task<string?> ReplacePlaylistItems(
        string playlistId,
        IEnumerable<string> uris,
        CancellationToken cancellationToken
    )
    {
        var body = new { uris = Uris(uris, 0, MaxPlaylistItems) };

        var snapshot = await webAPIClient.PutAsync<object, PlaylistSnapshot>(
            Endpoints.UpdatePlaylistItems(playlistId),
            body,
            ModifyAccessToken,
            cancellationToken
        );

        return snapshot.SnapshotId;
    }

    public async Task<string?> RemovePlaylistItems(
        string playlistId,
        IEnumerable<string> uris,
        string? snapshotId,
        CancellationToken cancellationToken
    )
    {
        var body = new
        {
            items = Uris(uris, 1, MaxPlaylistItems).Select(uri => new { uri }),
            snapshot_id = snapshotId,
        };

        var snapshot = await webAPIClient.DeleteAsync<object, PlaylistSnapshot>(
            Endpoints.RemovePlaylistItems(playlistId),
            body,
            ModifyAccessToken,
            cancellationToken
        );

        return snapshot.SnapshotId;
    }

    public IAsyncEnumerable<SimplifiedPlaylist> GetCurrentUserPlaylists(
        int offset,
        CancellationToken cancellationToken
    ) =>
        webAPIClient.GetOffsetPaginated<SimplifiedPlaylist>(
            Endpoints.GetCurrentUserPlaylists(offset),
            authorizationService.GetAccessToken(AuthorizationScope.PlaylistReadPrivate),
            cancellationToken
        );

    public Task<Playlist> CreatePlaylist(
        string name,
        bool? isPublic,
        bool? collaborative,
        string? description,
        CancellationToken cancellationToken
    )
    {
        ArgumentNullException.ThrowIfNull(name);

        return webAPIClient.PostAsync<object, Playlist>(
            Endpoints.CreatePlaylist(),
            new
            {
                name,
                @public = isPublic,
                collaborative,
                description,
            },
            ModifyAccessToken,
            cancellationToken
        );
    }

    public Task<IReadOnlyList<Image>> GetPlaylistCoverImage(
        string playlistId,
        CancellationToken cancellationToken
    ) =>
        webAPIClient.GetAsync<IReadOnlyList<Image>>(
            Endpoints.GetPlaylistCoverImage(playlistId),
            authorizationService.GetAccessToken(),
            cancellationToken
        );

    public Task AddCustomPlaylistCoverImage(
        string playlistId,
        string base64Image,
        CancellationToken cancellationToken
    )
    {
        ArgumentNullException.ThrowIfNull(base64Image);

        return webAPIClient.PutAsync(
            Endpoints.AddCustomPlaylistCoverImage(playlistId),
            base64Image,
            MediaTypeNames.Image.Jpeg,
            authorizationService.GetAccessToken(
                AuthorizationScope.UgcImageUpload,
                AuthorizationScope.PlaylistModifyPublic,
                AuthorizationScope.PlaylistModifyPrivate
            ),
            cancellationToken
        );
    }

    // The spec requires both modify scopes for every playlist change, public or private.
    private AccessTokenProvider ModifyAccessToken =>
        authorizationService.GetAccessToken(
            AuthorizationScope.PlaylistModifyPublic,
            AuthorizationScope.PlaylistModifyPrivate
        );
}
