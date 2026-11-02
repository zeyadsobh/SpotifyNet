using System.Text.Json.Serialization;

namespace SpotifyNet.Datastructures.Spotify;

public sealed record Restrictions
{
    [JsonPropertyName("reason")]
    public RestrictionReason? Reason { get; init; }
}
