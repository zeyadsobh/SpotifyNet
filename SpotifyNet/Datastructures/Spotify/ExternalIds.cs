using System.Text.Json.Serialization;

namespace SpotifyNet.Datastructures.Spotify;

/// <summary>Known external identifiers of an item: ISRC, EAN and UPC.</summary>
public sealed record ExternalIds
{
    /// <summary>The International Standard Recording Code.</summary>
    [JsonPropertyName("isrc")]
    public string? ISRC { get; init; }

    /// <summary>The International Article Number.</summary>
    [JsonPropertyName("ean")]
    public string? EAN { get; init; }

    /// <summary>The Universal Product Code.</summary>
    [JsonPropertyName("upc")]
    public string? UPC { get; init; }
}
