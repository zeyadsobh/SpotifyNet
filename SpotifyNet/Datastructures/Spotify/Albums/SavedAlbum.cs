using System;
using System.Text.Json.Serialization;

namespace SpotifyNet.Datastructures.Spotify.Albums;

/// <summary>An album saved in the current user's library.</summary>
public sealed record SavedAlbum
{
    /// <summary>The date and time the album was saved, in UTC.</summary>
    [JsonPropertyName("added_at")]
    public DateTime? AddedAt { get; init; }

    /// <summary>The saved album.</summary>
    [JsonPropertyName("album")]
    public Album? Album { get; init; }
}
