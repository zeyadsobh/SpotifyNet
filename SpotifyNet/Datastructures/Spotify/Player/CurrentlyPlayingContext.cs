using System.Text.Json.Serialization;
using SpotifyNet.Datastructures.Serialization;

namespace SpotifyNet.Datastructures.Spotify.Player;

public sealed record CurrentlyPlayingContext
{
    [JsonPropertyName("device")]
    public Device? Device { get; init; }

    [JsonPropertyName("repeat_state")]
    public string? RepeatState { get; init; }

    [JsonPropertyName("shuffle_state")]
    public bool? ShuffleState { get; init; }

    [JsonPropertyName("context")]
    public Context? Context { get; init; }

    [JsonPropertyName("timestamp")]
    public long? Timestamp { get; init; }

    [JsonPropertyName("progress_ms")]
    public int? ProgressMs { get; init; }

    [JsonPropertyName("is_playing")]
    public bool? IsPlaying { get; init; }

    [JsonPropertyName("item")]
    public PlayableItem? Item { get; init; }

    [JsonPropertyName("currently_playing_type")]
    public string? CurrentlyPlayingType { get; init; }

    // Spotify nests these under "disallows", unlike the spec.
    [JsonPropertyName("actions")]
    [JsonConverter(typeof(ActionsConverter))]
    public Disallows? Actions { get; init; }
}
