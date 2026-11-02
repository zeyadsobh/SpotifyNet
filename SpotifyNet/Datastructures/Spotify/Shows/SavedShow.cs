using System;
using System.Text.Json.Serialization;

namespace SpotifyNet.Datastructures.Spotify.Shows;

public sealed record SavedShow
{
    [JsonPropertyName("added_at")]
    public DateTime? AddedAt { get; init; }

    [JsonPropertyName("show")]
    public SimplifiedShow? Show { get; init; }
}
