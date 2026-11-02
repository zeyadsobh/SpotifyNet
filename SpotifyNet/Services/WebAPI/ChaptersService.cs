using System.Threading;
using System.Threading.Tasks;
using SpotifyNet.Clients.Abstractions;
using SpotifyNet.Datastructures.Spotify.Chapters;
using SpotifyNet.Services.Abstractions;
using SpotifyNet.Services.Abstractions.WebAPI;

namespace SpotifyNet.Services.WebAPI;

internal sealed class ChaptersService(
    IAuthorizationService authorizationService,
    IWebAPIClient webAPIClient
) : IChaptersService
{
    public Task<Chapter> GetChapter(string chapterId, CancellationToken cancellationToken) =>
        webAPIClient.GetAsync<Chapter>(
            Endpoints.GetChapter(chapterId),
            authorizationService.GetAccessToken(),
            cancellationToken
        );
}
