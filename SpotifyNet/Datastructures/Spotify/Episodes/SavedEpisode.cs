using System;
using System.Text.Json.Serialization;
using SpotifyNet.Datastructures.Serialization;

namespace SpotifyNet.Datastructures.Spotify.Episodes;

/// <summary>An episode saved in the current user's library.</summary>
public sealed record SavedEpisode
{
    /// <summary>The date and time the episode was saved, in UTC.</summary>
    [JsonPropertyName("added_at")]
    public DateTime? AddedAt { get; init; }

    // Null for episodes that are no longer available, which Spotify sends as placeholders.
    /// <summary>The saved episode; <see langword="null"/> if it's no longer available.</summary>
    [JsonPropertyName("episode")]
    [JsonConverter(typeof(UnavailableAsNullConverter<Episode>))]
    public Episode? Episode { get; init; }
}
