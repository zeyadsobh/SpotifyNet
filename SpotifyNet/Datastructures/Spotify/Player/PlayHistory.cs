using System;
using System.Text.Json.Serialization;
using SpotifyNet.Datastructures.Spotify.Tracks;

namespace SpotifyNet.Datastructures.Spotify.Player;

/// <summary>A track the user played, with when and where from.</summary>
public sealed record PlayHistory
{
    /// <summary>The track the user listened to.</summary>
    [JsonPropertyName("track")]
    public Track? Track { get; init; }

    /// <summary>The date and time the track was played.</summary>
    [JsonPropertyName("played_at")]
    public DateTime? PlayedAt { get; init; }

    /// <summary>The context the track was played from.</summary>
    [JsonPropertyName("context")]
    public Context? Context { get; init; }
}
