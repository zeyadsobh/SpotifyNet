using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SpotifyNet.Clients.Abstractions;
using SpotifyNet.Datastructures.Spotify.Audiobooks;
using SpotifyNet.Datastructures.Spotify.Authorization;
using SpotifyNet.Datastructures.Spotify.Chapters;
using SpotifyNet.Services.Abstractions;
using SpotifyNet.Services.Abstractions.WebAPI;

namespace SpotifyNet.Services.WebAPI;

internal sealed class AudiobooksService(
    IAuthorizationService authorizationService,
    IWebAPIClient webAPIClient
) : IAudiobooksService
{
    public Task<Audiobook> GetAudiobook(string audiobookId, CancellationToken cancellationToken) =>
        webAPIClient.GetAsync<Audiobook>(
            Endpoints.GetAudiobook(audiobookId),
            authorizationService.GetAccessToken(),
            cancellationToken
        );

    public IAsyncEnumerable<SimplifiedChapter> GetChapters(
        string audiobookId,
        int offset,
        CancellationToken cancellationToken
    ) =>
        webAPIClient.GetOffsetPaginated<SimplifiedChapter>(
            Endpoints.GetAudiobookChapters(audiobookId, offset),
            authorizationService.GetAccessToken(),
            cancellationToken
        );

    public IAsyncEnumerable<SimplifiedAudiobook> GetUserSavedAudiobooks(
        int offset,
        CancellationToken cancellationToken
    ) =>
        webAPIClient.GetOffsetPaginated<SimplifiedAudiobook>(
            Endpoints.GetUserSavedAudiobooks(offset),
            authorizationService.GetAccessToken(AuthorizationScope.UserLibraryRead),
            cancellationToken
        );
}
