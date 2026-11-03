using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SpotifyNet.Clients.Abstractions;
using SpotifyNet.Datastructures.Spotify.Albums;
using SpotifyNet.Datastructures.Spotify.Artists;

namespace SpotifyNet.Services.Abstractions.WebAPI;

/// <summary>Artists and their albums.</summary>
/// <remarks>
/// Bad arguments throw <see cref="ArgumentNullException"/>, <see cref="ArgumentException"/> or
/// <see cref="ArgumentOutOfRangeException"/> before any request is sent. Requests throw
/// <see cref="AuthorizationException"/> if there's no access token yet or it lacks a required
/// scope, and <see cref="WebAPIException"/> if Spotify answers with an error; paged results do so
/// while they're enumerated.
/// </remarks>
public interface IArtistsService
{
    /// <summary>Gets an artist's catalog information.</summary>
    Task<Artist> GetArtist(string artistId, CancellationToken cancellationToken = default);

    /// <summary>Gets an artist's albums, a page at a time.</summary>
    /// <param name="artistId">The artist's Spotify ID.</param>
    /// <param name="includeGroups">Only these groups; all of them if <see langword="null"/>.</param>
    /// <param name="offset">The index of the first album to return.</param>
    /// <param name="cancellationToken">A token to cancel the request.</param>
    IAsyncEnumerable<SimplifiedAlbum> GetAlbums(
        string artistId,
        IEnumerable<AlbumGroup>? includeGroups = null,
        int offset = 0,
        CancellationToken cancellationToken = default
    );
}
