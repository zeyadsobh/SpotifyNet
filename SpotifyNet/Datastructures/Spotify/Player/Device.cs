using System.Text.Json.Serialization;

namespace SpotifyNet.Datastructures.Spotify.Player;

/// <summary>A Spotify Connect device.</summary>
public sealed record Device
{
    /// <summary>
    /// The device ID. It's unique and persistent only to some extent, so refetch it periodically
    /// rather than caching it.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; init; }

    /// <summary>Whether this device is the currently active one.</summary>
    [JsonPropertyName("is_active")]
    public bool? IsActive { get; init; }

    /// <summary>Whether this device is currently in a private session.</summary>
    [JsonPropertyName("is_private_session")]
    public bool? IsPrivateSession { get; init; }

    /// <summary>
    /// Whether controlling this device is restricted; if so, it accepts no Web API commands.
    /// </summary>
    [JsonPropertyName("is_restricted")]
    public bool? IsRestricted { get; init; }

    /// <summary>
    /// A human-readable name for the device: one the user can configure, or a generic one for the
    /// manufacturer or model.
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; init; }

    /// <summary>
    /// The device type, such as <c>computer</c>, <c>smartphone</c> or <c>speaker</c>.
    /// </summary>
    [JsonPropertyName("type")]
    public string? Type { get; init; }

    /// <summary>The current volume in percent.</summary>
    [JsonPropertyName("volume_percent")]
    public int? VolumePercent { get; init; }

    /// <summary>Whether this device can be used to set the volume.</summary>
    [JsonPropertyName("supports_volume")]
    public bool? SupportsVolume { get; init; }
}
