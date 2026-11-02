using System.Text.Json.Serialization;

namespace SpotifyNet.Datastructures.Spotify.Audiobooks;

public sealed record Author
{
    [JsonPropertyName("name")]
    public string? Name { get; init; }
}
