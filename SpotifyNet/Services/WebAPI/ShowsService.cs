using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SpotifyNet.Clients.Abstractions;
using SpotifyNet.Datastructures.Spotify.Authorization;
using SpotifyNet.Datastructures.Spotify.Episodes;
using SpotifyNet.Datastructures.Spotify.Shows;
using SpotifyNet.Services.Abstractions;
using SpotifyNet.Services.Abstractions.WebAPI;

namespace SpotifyNet.Services.WebAPI;

internal sealed class ShowsService(
    IAuthorizationService authorizationService,
    IWebAPIClient webAPIClient
) : IShowsService
{
    public Task<Show> GetShow(string showId, CancellationToken cancellationToken) =>
        webAPIClient.GetAsync<Show>(
            Endpoints.GetShow(showId),
            authorizationService.GetAccessToken(AuthorizationScope.UserReadPlaybackPosition),
            cancellationToken
        );

    public IAsyncEnumerable<SimplifiedEpisode> GetEpisodes(
        string showId,
        int offset,
        CancellationToken cancellationToken
    ) =>
        webAPIClient.GetOffsetPaginated<SimplifiedEpisode>(
            Endpoints.GetShowEpisodes(showId, offset),
            authorizationService.GetAccessToken(AuthorizationScope.UserReadPlaybackPosition),
            cancellationToken
        );

    public IAsyncEnumerable<SavedShow> GetUserSavedShows(
        int offset,
        CancellationToken cancellationToken
    ) =>
        webAPIClient.GetOffsetPaginated<SavedShow>(
            Endpoints.GetUserSavedShows(offset),
            authorizationService.GetAccessToken(AuthorizationScope.UserLibraryRead),
            cancellationToken
        );
}
