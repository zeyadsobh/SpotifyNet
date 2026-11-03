using System.Text.Json.Serialization;

namespace SpotifyNet.Datastructures.Spotify.Audiobooks;

/// <summary>The author of an audiobook.</summary>
public sealed record Author
{
    /// <summary>The name of the author.</summary>
    [JsonPropertyName("name")]
    public string? Name { get; init; }
}
