using System.Text.Json.Serialization;

namespace SpotifyNet.Datastructures.Spotify;

/// <summary>How far the user got in an episode or chapter.</summary>
public sealed record ResumePoint
{
    /// <summary>Whether the user has fully played the episode or chapter.</summary>
    [JsonPropertyName("fully_played")]
    public bool? FullyPlayed { get; init; }

    /// <summary>
    /// The user's most recent position in the episode or chapter, in milliseconds.
    /// </summary>
    [JsonPropertyName("resume_position_ms")]
    public int? ResumePositionMs { get; init; }
}
