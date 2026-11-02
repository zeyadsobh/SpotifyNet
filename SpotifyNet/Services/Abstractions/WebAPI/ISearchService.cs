using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SpotifyNet.Datastructures.Spotify.Search;

namespace SpotifyNet.Services.Abstractions.WebAPI;

public interface ISearchService
{
    /// <summary>Gets one page of results for each of <paramref name="types"/>, in one request.</summary>
    /// <param name="query">Supports Spotify's field filters, e.g. <c>artist:Miles year:1959</c>.</param>
    /// <param name="limit">Results per type, at most 10; Spotify defaults to 5.</param>
    /// <param name="offset">At most 1000.</param>
    /// <param name="includeExternalAudio">Mark externally hosted audio as playable.</param>
    Task<SearchResult> SearchItems(
        string query,
        IEnumerable<SearchType> types,
        int? limit = null,
        int? offset = null,
        bool includeExternalAudio = false,
        CancellationToken cancellationToken = default
    );
}
