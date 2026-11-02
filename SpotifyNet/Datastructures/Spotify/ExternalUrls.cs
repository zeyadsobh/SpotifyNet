using System.Text.Json.Serialization;

namespace SpotifyNet.Datastructures.Spotify;

public sealed record ExternalUrls
{
    [JsonPropertyName("spotify")]
    public string? Spotify { get; init; }
}
