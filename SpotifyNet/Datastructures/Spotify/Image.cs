using System.Text.Json.Serialization;

namespace SpotifyNet.Datastructures.Spotify;

/// <summary>An image, with its URL and size in pixels.</summary>
public sealed record Image
{
    /// <summary>The source URL of the image.</summary>
    [JsonPropertyName("url")]
    public required string Url { get; init; }

    /// <summary>The image width in pixels.</summary>
    [JsonPropertyName("width")]
    public required int? Width { get; init; }

    /// <summary>The image height in pixels.</summary>
    [JsonPropertyName("height")]
    public required int? Height { get; init; }
}
