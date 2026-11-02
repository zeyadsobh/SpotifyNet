using System.Text.Json.Serialization;

namespace SpotifyNet.Datastructures.Spotify;

public sealed record Error
{
    [JsonPropertyName("status")]
    public required int Status { get; init; }

    [JsonPropertyName("message")]
    public required string Message { get; init; }
}
