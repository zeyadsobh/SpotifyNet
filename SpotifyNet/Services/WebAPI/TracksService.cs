using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SpotifyNet.Clients.Abstractions;
using SpotifyNet.Datastructures.Spotify.Authorization;
using SpotifyNet.Datastructures.Spotify.Tracks;
using SpotifyNet.Services.Abstractions;
using SpotifyNet.Services.Abstractions.WebAPI;

namespace SpotifyNet.Services.WebAPI;

internal sealed class TracksService(
    IAuthorizationService authorizationService,
    IWebAPIClient webAPIClient
) : ITracksService
{
    public IAsyncEnumerable<SavedTrack> GetUserSavedTracks(
        int offset,
        CancellationToken cancellationToken
    ) =>
        webAPIClient.GetOffsetPaginated<SavedTrack>(
            Endpoints.GetUserSavedTracks(offset),
            authorizationService.GetAccessToken(AuthorizationScope.UserLibraryRead),
            cancellationToken
        );

    public Task<Track> GetTrack(string trackId, CancellationToken cancellationToken) =>
        webAPIClient.GetAsync<Track>(
            Endpoints.GetTrack(trackId),
            authorizationService.GetAccessToken(),
            cancellationToken
        );
}
