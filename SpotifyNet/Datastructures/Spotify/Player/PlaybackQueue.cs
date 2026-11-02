using System.Text.Json.Serialization;

namespace SpotifyNet.Datastructures.Spotify.Player;

public sealed record PlaybackQueue
{
    [JsonPropertyName("currently_playing")]
    public PlayableItem? CurrentlyPlaying { get; init; }

    [JsonPropertyName("queue")]
    public PlayableItem[]? Queue { get; init; }
}
