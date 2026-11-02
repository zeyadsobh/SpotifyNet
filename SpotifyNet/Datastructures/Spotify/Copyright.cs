using System.Text.Json.Serialization;

namespace SpotifyNet.Datastructures.Spotify;

public sealed record Copyright
{
    [JsonPropertyName("text")]
    public string? Text { get; init; }

    [JsonPropertyName("type")]
    public string? Type { get; init; }
}
