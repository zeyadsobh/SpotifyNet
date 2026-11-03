using System.Text.Json.Serialization;
using SpotifyNet.Datastructures.Serialization;

namespace SpotifyNet.Datastructures.Spotify;

/// <summary>A page of a cursor-paged list; the cursors find the next set of items.</summary>
public sealed record CursorPaginationWrapper<T>
{
    /// <summary>A link to the Web API endpoint returning the full result of the request.</summary>
    [JsonPropertyName("href")]
    public string? Href { get; init; }

    /// <summary>
    /// The maximum number of items in the response (as set in the query or by default).
    /// </summary>
    [JsonPropertyName("limit")]
    public int? Limit { get; init; }

    /// <summary>URL to the next page of items; <see langword="null"/> if none.</summary>
    [JsonPropertyName("next")]
    public string? Next { get; init; }

    /// <summary>The cursors used to find the next set of items.</summary>
    [JsonPropertyName("cursors")]
    public Cursors? Cursors { get; init; }

    /// <summary>The total number of items available to return.</summary>
    [JsonPropertyName("total")]
    public int? Total { get; init; }

    /// <summary>
    /// The items on this page; any <see langword="null"/> ones Spotify sends are left out.
    /// </summary>
    [JsonPropertyName("items")]
    [JsonConverter(typeof(SkipNullElementsConverterFactory))]
    public T[]? Items { get; init; }
}
