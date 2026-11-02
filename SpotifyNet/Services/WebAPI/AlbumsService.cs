using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SpotifyNet.Clients.Abstractions;
using SpotifyNet.Datastructures.Spotify.Albums;
using SpotifyNet.Datastructures.Spotify.Authorization;
using SpotifyNet.Datastructures.Spotify.Tracks;
using SpotifyNet.Services.Abstractions;
using SpotifyNet.Services.Abstractions.WebAPI;

namespace SpotifyNet.Services.WebAPI;

internal sealed class AlbumsService(
    IAuthorizationService authorizationService,
    IWebAPIClient webAPIClient
) : IAlbumsService
{
    public Task<Album> GetAlbum(string albumId, CancellationToken cancellationToken) =>
        webAPIClient.GetAsync<Album>(
            Endpoints.GetAlbum(albumId),
            authorizationService.GetAccessToken(),
            cancellationToken
        );

    public IAsyncEnumerable<SimplifiedTrack> GetTracks(
        string albumId,
        int offset,
        CancellationToken cancellationToken
    ) =>
        webAPIClient.GetOffsetPaginated<SimplifiedTrack>(
            Endpoints.GetAlbumTracks(albumId, offset),
            authorizationService.GetAccessToken(),
            cancellationToken
        );

    public IAsyncEnumerable<SavedAlbum> GetUserSavedAlbums(
        int offset,
        CancellationToken cancellationToken
    ) =>
        webAPIClient.GetOffsetPaginated<SavedAlbum>(
            Endpoints.GetUserSavedAlbums(offset),
            authorizationService.GetAccessToken(AuthorizationScope.UserLibraryRead),
            cancellationToken
        );
}
