using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SpotifyNet.Clients.Abstractions;
using SpotifyNet.Datastructures.Spotify.Albums;
using SpotifyNet.Datastructures.Spotify.Artists;
using SpotifyNet.Services.Abstractions;
using SpotifyNet.Services.Abstractions.WebAPI;

namespace SpotifyNet.Services.WebAPI;

internal sealed class ArtistsService(
    IAuthorizationService authorizationService,
    IWebAPIClient webAPIClient
) : IArtistsService
{
    public Task<Artist> GetArtist(string artistId, CancellationToken cancellationToken) =>
        webAPIClient.GetAsync<Artist>(
            Endpoints.GetArtist(artistId),
            authorizationService.GetAccessToken(),
            cancellationToken
        );

    public IAsyncEnumerable<SimplifiedAlbum> GetAlbums(
        string artistId,
        IEnumerable<AlbumGroup>? includeGroups,
        int offset,
        CancellationToken cancellationToken
    ) =>
        webAPIClient.GetOffsetPaginated<SimplifiedAlbum>(
            Endpoints.GetArtistAlbums(artistId, includeGroups, offset),
            authorizationService.GetAccessToken(),
            cancellationToken
        );
}
