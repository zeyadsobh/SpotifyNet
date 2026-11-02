using System.Text.Json.Serialization;

namespace SpotifyNet.Datastructures.Spotify;

public sealed record ResumePoint
{
    [JsonPropertyName("fully_played")]
    public bool? FullyPlayed { get; init; }

    [JsonPropertyName("resume_position_ms")]
    public int? ResumePositionMs { get; init; }
}
