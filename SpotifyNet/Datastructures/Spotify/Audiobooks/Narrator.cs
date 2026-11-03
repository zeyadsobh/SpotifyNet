using System.Text.Json.Serialization;

namespace SpotifyNet.Datastructures.Spotify.Audiobooks;

/// <summary>The narrator of an audiobook.</summary>
public sealed record Narrator
{
    /// <summary>The name of the narrator.</summary>
    [JsonPropertyName("name")]
    public string? Name { get; init; }
}
