using System.Text.Json.Serialization;

namespace SpotifyNet.Datastructures.Spotify;

public sealed record ExternalIds
{
    [JsonPropertyName("isrc")]
    public string? ISRC { get; init; }

    [JsonPropertyName("ean")]
    public string? EAN { get; init; }

    [JsonPropertyName("upc")]
    public string? UPC { get; init; }
}
