using System;
using System.Text.Json.Serialization;

namespace SpotifyNet.Datastructures.Spotify.Tracks;

public sealed record SavedTrack
{
    [JsonPropertyName("added_at")]
    public DateTime? AddedAt { get; init; }

    [JsonPropertyName("track")]
    public Track? Track { get; init; }
}
