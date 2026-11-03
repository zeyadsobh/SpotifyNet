using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SpotifyNet.Clients.Abstractions;
using SpotifyNet.Datastructures.Spotify.Search;

namespace SpotifyNet.Services.Abstractions.WebAPI;

/// <summary>Searching the Spotify catalog.</summary>
/// <remarks>
/// Bad arguments throw <see cref="ArgumentNullException"/>, <see cref="ArgumentException"/> or
/// <see cref="ArgumentOutOfRangeException"/> before any request is sent. Requests throw
/// <see cref="AuthorizationException"/> if there's no access token yet or it lacks a required
/// scope, and <see cref="WebAPIException"/> if Spotify answers with an error.
/// </remarks>
public interface ISearchService
{
    /// <summary>
    /// Searches the catalog for items matching <paramref name="query"/>. Gets one page of results
    /// for each of <paramref name="types"/>, in one request.
    /// </summary>
    /// <param name="query">Supports Spotify's field filters, e.g. <c>artist:Miles year:1959</c>.</param>
    /// <param name="types">The kinds of item to search for.</param>
    /// <param name="limit">Results per type, at most 10; Spotify defaults to 5.</param>
    /// <param name="offset">At most 1000.</param>
    /// <param name="includeExternalAudio">Mark externally hosted audio as playable.</param>
    /// <param name="cancellationToken">A token to cancel the request.</param>
    Task<SearchResult> SearchItems(
        string query,
        IEnumerable<SearchType> types,
        int? limit = null,
        int? offset = null,
        bool includeExternalAudio = false,
        CancellationToken cancellationToken = default
    );
}
