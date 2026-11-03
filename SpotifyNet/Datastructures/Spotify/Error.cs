using System.Text.Json.Serialization;

namespace SpotifyNet.Datastructures.Spotify;

/// <summary>
/// An error from the Web API: its HTTP status code and a short description of the cause.
/// </summary>
public sealed record Error
{
    /// <summary>The HTTP status code, also returned in the response header.</summary>
    [JsonPropertyName("status")]
    public required int Status { get; init; }

    /// <summary>A short description of the cause of the error.</summary>
    [JsonPropertyName("message")]
    public required string Message { get; init; }
}
