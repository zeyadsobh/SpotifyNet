using System.Text.Json.Serialization;

namespace SpotifyNet.Datastructures.Spotify.Player;

/// <summary>The item playing and the queue after it.</summary>
public sealed record PlaybackQueue
{
    /// <summary>The track or episode currently playing; can be <see langword="null"/>.</summary>
    [JsonPropertyName("currently_playing")]
    public PlayableItem? CurrentlyPlaying { get; init; }

    /// <summary>The tracks or episodes in the queue; can be empty.</summary>
    [JsonPropertyName("queue")]
    public PlayableItem[]? Queue { get; init; }
}
