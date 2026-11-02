using System.Text.Json.Serialization;

namespace SpotifyNet.Datastructures.Spotify.Player;

public sealed record Device
{
    [JsonPropertyName("id")]
    public string? Id { get; init; }

    [JsonPropertyName("is_active")]
    public bool? IsActive { get; init; }

    [JsonPropertyName("is_private_session")]
    public bool? IsPrivateSession { get; init; }

    [JsonPropertyName("is_restricted")]
    public bool? IsRestricted { get; init; }

    [JsonPropertyName("name")]
    public string? Name { get; init; }

    [JsonPropertyName("type")]
    public string? Type { get; init; }

    [JsonPropertyName("volume_percent")]
    public int? VolumePercent { get; init; }

    [JsonPropertyName("supports_volume")]
    public bool? SupportsVolume { get; init; }
}
