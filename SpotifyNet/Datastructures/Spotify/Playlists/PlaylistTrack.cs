using System;
using System.Text.Json.Serialization;

namespace SpotifyNet.Datastructures.Spotify.Playlists;

public sealed record PlaylistTrack
{
    [JsonPropertyName("added_at")]
    public DateTime? AddedAt { get; init; }

    [JsonPropertyName("added_by")]
    public PlaylistUser? AddedBy { get; init; }

    [JsonPropertyName("is_local")]
    public bool? IsLocal { get; init; }

    [JsonPropertyName("item")]
    public PlayableItem? Item { get; init; }
}
