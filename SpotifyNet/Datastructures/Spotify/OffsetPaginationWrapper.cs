using System.Text.Json.Serialization;
using SpotifyNet.Datastructures.Serialization;

namespace SpotifyNet.Datastructures.Spotify;

/// <summary>A page of an offset-paged list, with links to the next and previous pages.</summary>
public sealed record OffsetPaginationWrapper<T>
{
    /// <summary>A link to the Web API endpoint returning the full result of the request.</summary>
    [JsonPropertyName("href")]
    public required string Href { get; init; }

    /// <summary>
    /// The maximum number of items in the response (as set in the query or by default).
    /// </summary>
    [JsonPropertyName("limit")]
    public required int Limit { get; init; }

    /// <summary>URL to the next page of items; <see langword="null"/> if none.</summary>
    [JsonPropertyName("next")]
    public required string? Next { get; init; }

    /// <summary>The offset of the items returned (as set in the query or by default).</summary>
    [JsonPropertyName("offset")]
    public required int Offset { get; init; }

    /// <summary>URL to the previous page of items; <see langword="null"/> if none.</summary>
    [JsonPropertyName("previous")]
    public required string? Previous { get; init; }

    /// <summary>The total number of items available to return.</summary>
    [JsonPropertyName("total")]
    public required int Total { get; init; }

    /// <summary>
    /// The items on this page; any <see langword="null"/> ones Spotify sends are left out.
    /// </summary>
    [JsonPropertyName("items")]
    [JsonConverter(typeof(SkipNullElementsConverterFactory))]
    public required T[] Items { get; init; }
}
