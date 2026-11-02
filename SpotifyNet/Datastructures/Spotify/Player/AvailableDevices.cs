using System.Text.Json.Serialization;

namespace SpotifyNet.Datastructures.Spotify.Player;

public sealed record AvailableDevices
{
    [JsonPropertyName("devices")]
    public required Device[] Devices { get; init; }
}
