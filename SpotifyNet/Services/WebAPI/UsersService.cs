using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SpotifyNet.Clients.Abstractions;
using SpotifyNet.Datastructures.Spotify;
using SpotifyNet.Datastructures.Spotify.Artists;
using SpotifyNet.Datastructures.Spotify.Authorization;
using SpotifyNet.Datastructures.Spotify.Users;
using SpotifyNet.Services.Abstractions;
using SpotifyNet.Services.Abstractions.WebAPI;

namespace SpotifyNet.Services.WebAPI;

internal sealed class UsersService(
    IAuthorizationService authorizationService,
    IWebAPIClient webAPIClient
) : IUsersService
{
    public Task<User> GetCurrentUserProfile(CancellationToken cancellationToken) =>
        webAPIClient.GetAsync<User>(
            Endpoints.GetCurrentUserProfile(),
            authorizationService.GetAccessToken(
                AuthorizationScope.UserReadPrivate,
                AuthorizationScope.UserReadEmail
            ),
            cancellationToken
        );

    public IAsyncEnumerable<T> GetTopItems<T>(
        TimeRange? timeRange,
        int offset,
        CancellationToken cancellationToken
    )
        where T : ITopItem =>
        webAPIClient.GetOffsetPaginated<T>(
            Endpoints.GetCurrentUserTopItems(T.TopItemType, timeRange, offset),
            authorizationService.GetAccessToken(AuthorizationScope.UserTopRead),
            cancellationToken
        );

    public IAsyncEnumerable<Artist> GetFollowedArtists(
        string? after,
        CancellationToken cancellationToken
    ) =>
        webAPIClient.Paginate<FollowedArtists, Artist>(
            Endpoints.GetFollowedArtists(after),
            authorizationService.GetAccessToken(AuthorizationScope.UserFollowRead),
            response => (response.Artists.Items ?? [], response.Artists.Next),
            cancellationToken
        );
}
