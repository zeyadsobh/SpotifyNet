using System.Text.Json.Serialization;

namespace SpotifyNet.Datastructures.Spotify.Audiobooks;

public sealed record Narrator
{
    [JsonPropertyName("name")]
    public string? Name { get; init; }
}
