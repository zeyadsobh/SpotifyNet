using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SpotifyNet.Clients.Abstractions;
using SpotifyNet.Datastructures.Spotify.Audiobooks;
using SpotifyNet.Datastructures.Spotify.Authorization;
using SpotifyNet.Datastructures.Spotify.Chapters;

namespace SpotifyNet.Services.Abstractions.WebAPI;

/// <summary>
/// Audiobooks, their chapters and the current user's saved audiobooks. Audiobooks are only
/// available in the US, UK, Canada, Ireland, New Zealand and Australia markets.
/// </summary>
/// <remarks>
/// Bad arguments throw <see cref="ArgumentNullException"/>, <see cref="ArgumentException"/> or
/// <see cref="ArgumentOutOfRangeException"/> before any request is sent. Requests throw
/// <see cref="AuthorizationException"/> if there's no access token yet or it lacks a required
/// scope, and <see cref="WebAPIException"/> if Spotify answers with an error; paged results do so
/// while they're enumerated.
/// </remarks>
public interface IAudiobooksService
{
    /// <summary>Gets an audiobook's catalog information.</summary>
    Task<Audiobook> GetAudiobook(string audiobookId, CancellationToken cancellationToken = default);

    /// <summary>Gets an audiobook's chapters, a page at a time.</summary>
    IAsyncEnumerable<SimplifiedChapter> GetChapters(
        string audiobookId,
        int offset = 0,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Gets the audiobooks saved in the current user's library, a page at a time.
    /// </summary>
    /// <remarks>Requires <see cref="AuthorizationScope.UserLibraryRead"/>.</remarks>
    IAsyncEnumerable<SimplifiedAudiobook> GetUserSavedAudiobooks(
        int offset = 0,
        CancellationToken cancellationToken = default
    );
}
