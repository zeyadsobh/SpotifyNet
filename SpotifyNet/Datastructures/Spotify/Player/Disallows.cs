using System.Text.Json.Serialization;

namespace SpotifyNet.Datastructures.Spotify.Player;

/// <summary>
/// The playback actions that aren't allowed right now; each flag is <see langword="true"/> for one
/// that isn't.
/// </summary>
public sealed record Disallows
{
    /// <summary>Whether interrupting playback is disallowed.</summary>
    [JsonPropertyName("interrupting_playback")]
    public bool? InterruptingPlayback { get; init; }

    /// <summary>Whether pausing is disallowed.</summary>
    [JsonPropertyName("pausing")]
    public bool? Pausing { get; init; }

    /// <summary>Whether resuming is disallowed.</summary>
    [JsonPropertyName("resuming")]
    public bool? Resuming { get; init; }

    /// <summary>Whether seeking the playback location is disallowed.</summary>
    [JsonPropertyName("seeking")]
    public bool? Seeking { get; init; }

    /// <summary>Whether skipping to the next context is disallowed.</summary>
    [JsonPropertyName("skipping_next")]
    public bool? SkippingNext { get; init; }

    /// <summary>Whether skipping to the previous context is disallowed.</summary>
    [JsonPropertyName("skipping_prev")]
    public bool? SkippingPrev { get; init; }

    /// <summary>Whether toggling the repeat context flag is disallowed.</summary>
    [JsonPropertyName("toggling_repeat_context")]
    public bool? TogglingRepeatContext { get; init; }

    /// <summary>Whether toggling shuffle is disallowed.</summary>
    [JsonPropertyName("toggling_shuffle")]
    public bool? TogglingShuffle { get; init; }

    /// <summary>Whether toggling the repeat track flag is disallowed.</summary>
    [JsonPropertyName("toggling_repeat_track")]
    public bool? TogglingRepeatTrack { get; init; }

    /// <summary>Whether transferring playback between devices is disallowed.</summary>
    [JsonPropertyName("transferring_playback")]
    public bool? TransferringPlayback { get; init; }
}
