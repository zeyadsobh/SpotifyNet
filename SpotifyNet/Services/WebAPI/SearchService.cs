using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SpotifyNet.Clients.Abstractions;
using SpotifyNet.Datastructures.Spotify.Search;
using SpotifyNet.Services.Abstractions;
using SpotifyNet.Services.Abstractions.WebAPI;
using static SpotifyNet.Services.WebAPI.WebAPIClientExtensions;

namespace SpotifyNet.Services.WebAPI;

internal sealed class SearchService(
    IAuthorizationService authorizationService,
    IWebAPIClient webAPIClient
) : ISearchService
{
    public Task<SearchResult> SearchItems(
        string query,
        IEnumerable<SearchType> types,
        int? limit,
        int? offset,
        bool includeExternalAudio,
        CancellationToken cancellationToken
    )
    {
        ArgumentNullException.ThrowIfNull(query);
        ArgumentNullException.ThrowIfNull(types);
        ThrowIfNegative(limit);

        return webAPIClient.GetAsync<SearchResult>(
            Endpoints.Search(query, types, limit, offset, includeExternalAudio),
            authorizationService.GetAccessToken(),
            cancellationToken
        );
    }
}
