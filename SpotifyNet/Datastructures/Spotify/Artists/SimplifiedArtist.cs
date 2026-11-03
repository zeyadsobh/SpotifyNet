using System.Text.Json.Serialization;

namespace SpotifyNet.Datastructures.Spotify.Artists;

/// <summary>An artist without their images.</summary>
public sealed record SimplifiedArtist
{
    /// <summary>Known external URLs for this artist.</summary>
    [JsonPropertyName("external_urls")]
    public ExternalUrls? ExternalUrls { get; init; }

    /// <summary>A link to the Web API endpoint providing full details of the artist.</summary>
    [JsonPropertyName("href")]
    public string? Href { get; init; }

    /// <summary>The Spotify ID for the artist.</summary>
    [JsonPropertyName("id")]
    public string? Id { get; init; }

    /// <summary>The name of the artist.</summary>
    [JsonPropertyName("name")]
    public string? Name { get; init; }

    /// <summary>The object type: <see cref="ItemType.artist"/>.</summary>
    [JsonPropertyName("type")]
    public ItemType? Type { get; init; }

    /// <summary>The Spotify URI for the artist.</summary>
    [JsonPropertyName("uri")]
    public string? Uri { get; init; }
}
