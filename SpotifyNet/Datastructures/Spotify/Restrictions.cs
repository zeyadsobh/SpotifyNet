using System.Text.Json.Serialization;

namespace SpotifyNet.Datastructures.Spotify;

/// <summary>A content restriction that applies to an item.</summary>
public sealed record Restrictions
{
    /// <summary>
    /// The reason for the restriction; <see cref="RestrictionReason.unknown"/> if it's one this
    /// library doesn't know.
    /// </summary>
    [JsonPropertyName("reason")]
    public RestrictionReason? Reason { get; init; }
}
