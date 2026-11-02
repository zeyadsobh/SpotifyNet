using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SpotifyNet.Datastructures.Spotify.Audiobooks;
using SpotifyNet.Datastructures.Spotify.Chapters;

namespace SpotifyNet.Services.Abstractions.WebAPI;

public interface IAudiobooksService
{
    Task<Audiobook> GetAudiobook(string audiobookId, CancellationToken cancellationToken = default);

    IAsyncEnumerable<SimplifiedChapter> GetChapters(
        string audiobookId,
        int offset = 0,
        CancellationToken cancellationToken = default
    );

    IAsyncEnumerable<SimplifiedAudiobook> GetUserSavedAudiobooks(
        int offset = 0,
        CancellationToken cancellationToken = default
    );
}
