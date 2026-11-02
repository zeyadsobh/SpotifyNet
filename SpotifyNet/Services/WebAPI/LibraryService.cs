using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using SpotifyNet.Clients.Abstractions;
using SpotifyNet.Datastructures.Spotify.Authorization;
using SpotifyNet.Services.Abstractions;
using SpotifyNet.Services.Abstractions.WebAPI;
using static SpotifyNet.Services.WebAPI.WebAPIClientExtensions;

namespace SpotifyNet.Services.WebAPI;

internal sealed class LibraryService(
    IAuthorizationService authorizationService,
    IWebAPIClient webAPIClient
) : ILibraryService
{
    private const int MaxLibraryUris = 40;

    public Task SaveItems(IEnumerable<string> uris, CancellationToken cancellationToken) =>
        webAPIClient.PutAsync(
            Endpoints.SaveItemsToLibrary(Uris(uris, 1, MaxLibraryUris)),
            ModifyAccessToken,
            cancellationToken
        );

    public Task RemoveItems(IEnumerable<string> uris, CancellationToken cancellationToken) =>
        webAPIClient.DeleteAsync(
            Endpoints.RemoveItemsFromLibrary(Uris(uris, 1, MaxLibraryUris)),
            ModifyAccessToken,
            cancellationToken
        );

    public async IAsyncEnumerable<bool> CheckSavedItems(
        IEnumerable<string> uris,
        [EnumeratorCancellation] CancellationToken cancellationToken
    )
    {
        ArgumentNullException.ThrowIfNull(uris);

        // A request per 40 URIs, each sent once the previous chunk's results are consumed.
        foreach (var chunk in uris.Chunk(MaxLibraryUris))
        {
            var saved = await webAPIClient.GetAsync<bool[]>(
                Endpoints.CheckUserSavedItems(NonNullUris(chunk)),
                authorizationService.GetAccessToken(
                    AuthorizationScope.UserLibraryRead,
                    AuthorizationScope.UserFollowRead,
                    AuthorizationScope.PlaylistReadPrivate
                ),
                cancellationToken
            );

            foreach (var isSaved in saved)
            {
                cancellationToken.ThrowIfCancellationRequested();
                yield return isSaved;
            }
        }
    }

    private AccessTokenProvider ModifyAccessToken =>
        authorizationService.GetAccessToken(
            AuthorizationScope.UserLibraryModify,
            AuthorizationScope.UserFollowModify,
            AuthorizationScope.PlaylistModifyPublic
        );
}
