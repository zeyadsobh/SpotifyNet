using System.Text.Json.Serialization;
using SpotifyNet.Datastructures.Serialization;

namespace SpotifyNet.Datastructures.Spotify.Player;

/// <summary>
/// The state of the user's playback: the item, its progress, the device and the player's modes.
/// </summary>
public sealed record CurrentlyPlayingContext
{
    /// <summary>The device that is currently active.</summary>
    [JsonPropertyName("device")]
    public Device? Device { get; init; }

    /// <summary>The repeat mode: <c>off</c>, <c>track</c> or <c>context</c>.</summary>
    [JsonPropertyName("repeat_state")]
    public string? RepeatState { get; init; }

    /// <summary>Whether shuffle is on.</summary>
    [JsonPropertyName("shuffle_state")]
    public bool? ShuffleState { get; init; }

    /// <summary>The context the item is playing from; can be <see langword="null"/>.</summary>
    [JsonPropertyName("context")]
    public Context? Context { get; init; }

    /// <summary>
    /// When the playback state last changed (play, pause, skip, scrub, new song, etc.), as a Unix
    /// timestamp in milliseconds.
    /// </summary>
    [JsonPropertyName("timestamp")]
    public long? Timestamp { get; init; }

    /// <summary>
    /// The progress into the item playing, in milliseconds; can be <see langword="null"/>.
    /// </summary>
    [JsonPropertyName("progress_ms")]
    public int? ProgressMs { get; init; }

    /// <summary>Whether something is currently playing.</summary>
    [JsonPropertyName("is_playing")]
    public bool? IsPlaying { get; init; }

    /// <summary>The track or episode currently playing; can be <see langword="null"/>.</summary>
    [JsonPropertyName("item")]
    public PlayableItem? Item { get; init; }

    /// <summary>
    /// The type of the item playing: <c>track</c>, <c>episode</c>, <c>ad</c> or <c>unknown</c>.
    /// </summary>
    [JsonPropertyName("currently_playing_type")]
    public string? CurrentlyPlayingType { get; init; }

    // Spotify nests these under "disallows", unlike the spec.
    /// <summary>
    /// The playback actions that aren't available in the current context, for updating the user
    /// interface.
    /// </summary>
    [JsonPropertyName("actions")]
    [JsonConverter(typeof(ActionsConverter))]
    public Disallows? Actions { get; init; }
}
