using System.Text.Json.Serialization;

namespace SpotifyNet.Datastructures.Spotify;

public sealed record Context
{
    [JsonPropertyName("type")]
    public ItemType? Type { get; init; }

    [JsonPropertyName("href")]
    public string? Href { get; init; }

    [JsonPropertyName("external_urls")]
    public ExternalUrls? ExternalUrls { get; init; }

    [JsonPropertyName("uri")]
    public string? Uri { get; init; }
}
