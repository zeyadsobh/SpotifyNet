using System.Text.Json.Serialization;

namespace SpotifyNet.Datastructures.Spotify.Player;

public sealed record Disallows
{
    [JsonPropertyName("interrupting_playback")]
    public bool? InterruptingPlayback { get; init; }

    [JsonPropertyName("pausing")]
    public bool? Pausing { get; init; }

    [JsonPropertyName("resuming")]
    public bool? Resuming { get; init; }

    [JsonPropertyName("seeking")]
    public bool? Seeking { get; init; }

    [JsonPropertyName("skipping_next")]
    public bool? SkippingNext { get; init; }

    [JsonPropertyName("skipping_prev")]
    public bool? SkippingPrev { get; init; }

    [JsonPropertyName("toggling_repeat_context")]
    public bool? TogglingRepeatContext { get; init; }

    [JsonPropertyName("toggling_shuffle")]
    public bool? TogglingShuffle { get; init; }

    [JsonPropertyName("toggling_repeat_track")]
    public bool? TogglingRepeatTrack { get; init; }

    [JsonPropertyName("transferring_playback")]
    public bool? TransferringPlayback { get; init; }
}
