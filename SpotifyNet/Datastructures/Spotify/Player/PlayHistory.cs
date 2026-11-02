using System;
using System.Text.Json.Serialization;
using SpotifyNet.Datastructures.Spotify.Tracks;

namespace SpotifyNet.Datastructures.Spotify.Player;

public sealed record PlayHistory
{
    [JsonPropertyName("track")]
    public Track? Track { get; init; }

    [JsonPropertyName("played_at")]
    public DateTime? PlayedAt { get; init; }

    [JsonPropertyName("context")]
    public Context? Context { get; init; }
}
