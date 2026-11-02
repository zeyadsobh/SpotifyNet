using System.Text.Json.Serialization;

namespace SpotifyNet.Datastructures.Spotify;

public sealed record Image
{
    [JsonPropertyName("url")]
    public required string Url { get; init; }

    [JsonPropertyName("width")]
    public required int? Width { get; init; }

    [JsonPropertyName("height")]
    public required int? Height { get; init; }
}
