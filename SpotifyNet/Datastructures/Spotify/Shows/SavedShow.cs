using System;
using System.Text.Json.Serialization;

namespace SpotifyNet.Datastructures.Spotify.Shows;

/// <summary>A show saved in the current user's library.</summary>
public sealed record SavedShow
{
    /// <summary>The date and time the show was saved, in UTC.</summary>
    [JsonPropertyName("added_at")]
    public DateTime? AddedAt { get; init; }

    /// <summary>The saved show.</summary>
    [JsonPropertyName("show")]
    public SimplifiedShow? Show { get; init; }
}
