using System.Text.Json.Serialization;

namespace SpotifyNet.Datastructures.Spotify;

/// <summary>Known external URLs of an object.</summary>
public sealed record ExternalUrls
{
    /// <summary>The Spotify URL for the object.</summary>
    [JsonPropertyName("spotify")]
    public string? Spotify { get; init; }
}
