using SpotifyNet.Clients.Abstractions;
using SpotifyNet.Services.Abstractions;
using SpotifyNet.Services.Abstractions.WebAPI;

namespace SpotifyNet.Services.WebAPI;

/// <summary>
/// The <see cref="IWebAPIService"/> whose services share one <see cref="IWebAPIClient"/> and get
/// their tokens from one <see cref="IAuthorizationService"/>.
/// </summary>
public sealed class WebAPIService(
    IAuthorizationService authorizationService,
    IWebAPIClient webAPIClient
) : IWebAPIService
{
    /// <inheritdoc />
    public IAlbumsService Albums { get; } = new AlbumsService(authorizationService, webAPIClient);

    /// <inheritdoc />
    public IArtistsService Artists { get; } =
        new ArtistsService(authorizationService, webAPIClient);

    /// <inheritdoc />
    public IAudiobooksService Audiobooks { get; } =
        new AudiobooksService(authorizationService, webAPIClient);

    /// <inheritdoc />
    public IChaptersService Chapters { get; } =
        new ChaptersService(authorizationService, webAPIClient);

    /// <inheritdoc />
    public IEpisodesService Episodes { get; } =
        new EpisodesService(authorizationService, webAPIClient);

    /// <inheritdoc />
    public ILibraryService Library { get; } =
        new LibraryService(authorizationService, webAPIClient);

    /// <inheritdoc />
    public IPlayerService Player { get; } = new PlayerService(authorizationService, webAPIClient);

    /// <inheritdoc />
    public IPlaylistsService Playlists { get; } =
        new PlaylistsService(authorizationService, webAPIClient);

    /// <inheritdoc />
    public ISearchService Search { get; } = new SearchService(authorizationService, webAPIClient);

    /// <inheritdoc />
    public IShowsService Shows { get; } = new ShowsService(authorizationService, webAPIClient);

    /// <inheritdoc />
    public ITracksService Tracks { get; } = new TracksService(authorizationService, webAPIClient);

    /// <inheritdoc />
    public IUsersService Users { get; } = new UsersService(authorizationService, webAPIClient);
}
