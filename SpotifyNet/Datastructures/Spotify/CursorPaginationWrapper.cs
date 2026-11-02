using System.Text.Json.Serialization;
using SpotifyNet.Datastructures.Serialization;

namespace SpotifyNet.Datastructures.Spotify;

public sealed record CursorPaginationWrapper<T>
{
    [JsonPropertyName("href")]
    public string? Href { get; init; }

    [JsonPropertyName("limit")]
    public int? Limit { get; init; }

    [JsonPropertyName("next")]
    public string? Next { get; init; }

    [JsonPropertyName("cursors")]
    public Cursors? Cursors { get; init; }

    [JsonPropertyName("total")]
    public int? Total { get; init; }

    [JsonPropertyName("items")]
    [JsonConverter(typeof(SkipNullElementsConverterFactory))]
    public T[]? Items { get; init; }
}
