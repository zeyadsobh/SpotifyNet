namespace SpotifyNet.Services.Abstractions.WebAPI;

/// <summary>The Spotify Web API, with a service for each area.</summary>
/// <remarks>
/// Requests need an access token for the current user: sign in with
/// <see cref="ITokenAcquirerService"/> first.
/// </remarks>
public interface IWebAPIService
{
    /// <summary>Albums, their tracks and the current user's saved albums.</summary>
    IAlbumsService Albums { get; }

    /// <summary>Artists and their albums.</summary>
    IArtistsService Artists { get; }

    /// <summary>Audiobooks, their chapters and the current user's saved audiobooks.</summary>
    IAudiobooksService Audiobooks { get; }

    /// <summary>Audiobook chapters.</summary>
    IChaptersService Chapters { get; }

    /// <summary>Podcast episodes and the current user's saved episodes.</summary>
    IEpisodesService Episodes { get; }

    /// <summary>Saving, removing and checking items in the current user's library.</summary>
    ILibraryService Library { get; }

    /// <summary>Playback state and controls for the current user's devices.</summary>
    IPlayerService Player { get; }

    /// <summary>
    /// Playlists, their items and cover images, and the current user's playlists.
    /// </summary>
    IPlaylistsService Playlists { get; }

    /// <summary>Searching the Spotify catalog.</summary>
    ISearchService Search { get; }

    /// <summary>Podcast shows, their episodes and the current user's saved shows.</summary>
    IShowsService Shows { get; }

    /// <summary>Tracks and the current user's saved tracks.</summary>
    ITracksService Tracks { get; }

    /// <summary>The current user's profile, top items and followed artists.</summary>
    IUsersService Users { get; }
}
