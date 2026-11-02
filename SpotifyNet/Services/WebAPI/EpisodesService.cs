using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SpotifyNet.Clients.Abstractions;
using SpotifyNet.Datastructures.Spotify.Authorization;
using SpotifyNet.Datastructures.Spotify.Episodes;
using SpotifyNet.Services.Abstractions;
using SpotifyNet.Services.Abstractions.WebAPI;

namespace SpotifyNet.Services.WebAPI;

internal sealed class EpisodesService(
    IAuthorizationService authorizationService,
    IWebAPIClient webAPIClient
) : IEpisodesService
{
    public Task<Episode> GetEpisode(string episodeId, CancellationToken cancellationToken) =>
        webAPIClient.GetAsync<Episode>(
            Endpoints.GetEpisode(episodeId),
            authorizationService.GetAccessToken(AuthorizationScope.UserReadPlaybackPosition),
            cancellationToken
        );

    public IAsyncEnumerable<SavedEpisode> GetUserSavedEpisodes(
        int offset,
        CancellationToken cancellationToken
    ) =>
        webAPIClient.GetOffsetPaginated<SavedEpisode>(
            Endpoints.GetUserSavedEpisodes(offset),
            authorizationService.GetAccessToken(
                AuthorizationScope.UserLibraryRead,
                AuthorizationScope.UserReadPlaybackPosition
            ),
            cancellationToken
        );
}
