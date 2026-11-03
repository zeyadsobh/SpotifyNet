using System;
using System.Text.Json.Serialization;

namespace SpotifyNet.Datastructures.Spotify.Tracks;

/// <summary>A track saved in the current user's library.</summary>
public sealed record SavedTrack
{
    /// <summary>The date and time the track was saved, in UTC.</summary>
    [JsonPropertyName("added_at")]
    public DateTime? AddedAt { get; init; }

    /// <summary>The saved track.</summary>
    [JsonPropertyName("track")]
    public Track? Track { get; init; }
}
