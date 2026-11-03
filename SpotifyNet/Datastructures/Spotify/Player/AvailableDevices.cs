using System.Text.Json.Serialization;

namespace SpotifyNet.Datastructures.Spotify.Player;

/// <summary>The devices the user can play on.</summary>
public sealed record AvailableDevices
{
    /// <summary>The user's available devices.</summary>
    [JsonPropertyName("devices")]
    public required Device[] Devices { get; init; }
}
