using System.Text.Json.Serialization;

namespace SpotifyNet.Datastructures.Spotify;

/// <summary>Where playback comes from: an artist, playlist, album or show.</summary>
public sealed record Context
{
    /// <summary>The kind of context: an artist, playlist, album or show.</summary>
    [JsonPropertyName("type")]
    public ItemType? Type { get; init; }

    /// <summary>A link to the Web API endpoint providing full details of the context.</summary>
    [JsonPropertyName("href")]
    public string? Href { get; init; }

    /// <summary>External URLs for this context.</summary>
    [JsonPropertyName("external_urls")]
    public ExternalUrls? ExternalUrls { get; init; }

    /// <summary>The Spotify URI for the context.</summary>
    [JsonPropertyName("uri")]
    public string? Uri { get; init; }
}
