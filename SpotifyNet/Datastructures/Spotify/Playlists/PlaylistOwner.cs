using System.Text.Json.Serialization;

namespace SpotifyNet.Datastructures.Spotify.Playlists;

public sealed record PlaylistOwner
{
    [JsonPropertyName("external_urls")]
    public ExternalUrls? ExternalUrls { get; init; }

    [JsonPropertyName("href")]
    public string? Href { get; init; }

    [JsonPropertyName("id")]
    public string? Id { get; init; }

    [JsonPropertyName("type")]
    public ItemType? Type { get; init; }

    [JsonPropertyName("uri")]
    public string? Uri { get; init; }

    [JsonPropertyName("display_name")]
    public string? DisplayName { get; init; }
}
