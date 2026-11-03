using System;
using System.Text.Json.Serialization;

namespace SpotifyNet.Datastructures.Spotify.Playlists;

/// <summary>A track or episode in a playlist, with who added it and when.</summary>
public sealed record PlaylistTrack
{
    /// <summary>
    /// The date and time the track or episode was added, in UTC; some very old playlists may return
    /// <see langword="null"/>.
    /// </summary>
    [JsonPropertyName("added_at")]
    public DateTime? AddedAt { get; init; }

    /// <summary>
    /// The Spotify user who added the track or episode; some very old playlists may return
    /// <see langword="null"/>.
    /// </summary>
    [JsonPropertyName("added_by")]
    public PlaylistUser? AddedBy { get; init; }

    /// <summary>Whether this track or episode is a local file.</summary>
    [JsonPropertyName("is_local")]
    public bool? IsLocal { get; init; }

    /// <summary>The track or episode.</summary>
    [JsonPropertyName("item")]
    public PlayableItem? Item { get; init; }
}
