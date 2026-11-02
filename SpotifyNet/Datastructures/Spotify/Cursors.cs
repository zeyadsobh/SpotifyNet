using System.Text.Json.Serialization;

namespace SpotifyNet.Datastructures.Spotify;

public sealed record Cursors
{
    [JsonPropertyName("after")]
    public string? After { get; init; }

    [JsonPropertyName("before")]
    public string? Before { get; init; }
}
