using SpotifyNet.Clients.Abstractions;
using SpotifyNet.Services.Abstractions;
using SpotifyNet.Services.Abstractions.WebAPI;

namespace SpotifyNet.Services.WebAPI;

public sealed class WebAPIService(
    IAuthorizationService authorizationService,
    IWebAPIClient webAPIClient
) : IWebAPIService
{
    public IAlbumsService Albums { get; } = new AlbumsService(authorizationService, webAPIClient);

    public IArtistsService Artists { get; } =
        new ArtistsService(authorizationService, webAPIClient);

    public IAudiobooksService Audiobooks { get; } =
        new AudiobooksService(authorizationService, webAPIClient);

    public IChaptersService Chapters { get; } =
        new ChaptersService(authorizationService, webAPIClient);

    public IEpisodesService Episodes { get; } =
        new EpisodesService(authorizationService, webAPIClient);

    public ILibraryService Library { get; } =
        new LibraryService(authorizationService, webAPIClient);

    public IPlayerService Player { get; } = new PlayerService(authorizationService, webAPIClient);

    public IPlaylistsService Playlists { get; } =
        new PlaylistsService(authorizationService, webAPIClient);

    public ISearchService Search { get; } = new SearchService(authorizationService, webAPIClient);

    public IShowsService Shows { get; } = new ShowsService(authorizationService, webAPIClient);

    public ITracksService Tracks { get; } = new TracksService(authorizationService, webAPIClient);

    public IUsersService Users { get; } = new UsersService(authorizationService, webAPIClient);
}
