using System.Text.Json.Serialization;

namespace SpotifyNet.Datastructures.Spotify;

/// <summary>A copyright statement for a work.</summary>
public sealed record Copyright
{
    /// <summary>The copyright text for this content.</summary>
    [JsonPropertyName("text")]
    public string? Text { get; init; }

    /// <summary>
    /// The type of copyright: <c>C</c> = the copyright, <c>P</c> = the sound recording
    /// (performance) copyright.
    /// </summary>
    [JsonPropertyName("type")]
    public string? Type { get; init; }
}
