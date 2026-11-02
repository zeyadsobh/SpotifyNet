using System.Text.Json.Serialization;

namespace SpotifyNet.Datastructures.Spotify.Playlists;

public sealed record PlaylistSnapshot
{
    [JsonPropertyName("snapshot_id")]
    public string? SnapshotId { get; init; }
}
