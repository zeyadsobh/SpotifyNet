using System.Text.Json.Serialization;

namespace SpotifyNet.Datastructures.Spotify.Playlists;

/// <summary>The snapshot ID a playlist has after a change.</summary>
public sealed record PlaylistSnapshot
{
    /// <summary>The snapshot ID for the playlist.</summary>
    [JsonPropertyName("snapshot_id")]
    public string? SnapshotId { get; init; }
}
