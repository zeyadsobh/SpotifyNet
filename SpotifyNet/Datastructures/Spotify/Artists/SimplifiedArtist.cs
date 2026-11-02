using System.Text.Json.Serialization;

namespace SpotifyNet.Datastructures.Spotify.Artists;

public sealed record SimplifiedArtist
{
    [JsonPropertyName("external_urls")]
    public ExternalUrls? ExternalUrls { get; init; }

    [JsonPropertyName("href")]
    public string? Href { get; init; }

    [JsonPropertyName("id")]
    public string? Id { get; init; }

    [JsonPropertyName("name")]
    public string? Name { get; init; }

    [JsonPropertyName("type")]
    public ItemType? Type { get; init; }

    [JsonPropertyName("uri")]
    public string? Uri { get; init; }
}
