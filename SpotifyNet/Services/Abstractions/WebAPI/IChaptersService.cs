using System;
using System.Threading;
using System.Threading.Tasks;
using SpotifyNet.Clients.Abstractions;
using SpotifyNet.Datastructures.Spotify.Chapters;

namespace SpotifyNet.Services.Abstractions.WebAPI;

/// <summary>
/// Audiobook chapters. Chapters are only available in the US, UK, Canada, Ireland, New Zealand
/// and Australia markets.
/// </summary>
/// <remarks>
/// Bad arguments throw <see cref="ArgumentNullException"/>, <see cref="ArgumentException"/> or
/// <see cref="ArgumentOutOfRangeException"/> before any request is sent. Requests throw
/// <see cref="AuthorizationException"/> if there's no access token yet or it lacks a required
/// scope, and <see cref="WebAPIException"/> if Spotify answers with an error.
/// </remarks>
public interface IChaptersService
{
    /// <summary>Gets an audiobook chapter's catalog information.</summary>
    Task<Chapter> GetChapter(string chapterId, CancellationToken cancellationToken = default);
}
