using System.Text.Json.Serialization;

namespace SpotifyNet.Datastructures.Spotify.Playlists;

public sealed record SimplifiedPlaylistTracks
{
    [JsonPropertyName("href")]
    public string? Href { get; init; }

    [JsonPropertyName("total")]
    public int? Total { get; init; }
}
