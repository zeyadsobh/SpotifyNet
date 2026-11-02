namespace SpotifyNet.Services.Abstractions.WebAPI;

public interface IWebAPIService
{
    IAlbumsService Albums { get; }

    IArtistsService Artists { get; }

    IAudiobooksService Audiobooks { get; }

    IChaptersService Chapters { get; }

    IEpisodesService Episodes { get; }

    ILibraryService Library { get; }

    IPlayerService Player { get; }

    IPlaylistsService Playlists { get; }

    ISearchService Search { get; }

    IShowsService Shows { get; }

    ITracksService Tracks { get; }

    IUsersService Users { get; }
}
