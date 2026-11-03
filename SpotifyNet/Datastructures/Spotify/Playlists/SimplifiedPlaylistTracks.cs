using System.Text.Json.Serialization;

namespace SpotifyNet.Datastructures.Spotify.Playlists;

/// <summary>A link to a playlist's items, and how many there are.</summary>
public sealed record SimplifiedPlaylistTracks
{
    /// <summary>
    /// A link to the Web API endpoint where full details of the playlist's tracks can be retrieved.
    /// </summary>
    [JsonPropertyName("href")]
    public string? Href { get; init; }

    /// <summary>Number of tracks in the playlist.</summary>
    [JsonPropertyName("total")]
    public int? Total { get; init; }
}
