using System.Text.Json.Serialization;
using SpotifyNet.Datastructures.Spotify.Artists;

namespace SpotifyNet.Datastructures.Spotify.Albums;

// Looser than the spec: local files in playlists have albums with the optional fields null or missing.
public sealed record SimplifiedAlbum
{
    [JsonPropertyName("album_type")]
    public AlbumType? AlbumType { get; init; }

    [JsonPropertyName("total_tracks")]
    public int? TotalTracks { get; init; }

    [JsonPropertyName("external_urls")]
    public required ExternalUrls ExternalUrls { get; init; }

    [JsonPropertyName("href")]
    public string? Href { get; init; }

    [JsonPropertyName("id")]
    public string? Id { get; init; }

    [JsonPropertyName("images")]
    public required Image[] Images { get; init; }

    [JsonPropertyName("name")]
    public required string Name { get; init; }

    [JsonPropertyName("release_date")]
    public string? ReleaseDate { get; init; }

    [JsonPropertyName("release_date_precision")]
    public ReleaseDatePrecision? ReleaseDatePrecision { get; init; }

    [JsonPropertyName("restrictions")]
    public Restrictions? Restrictions { get; init; }

    [JsonPropertyName("type")]
    public required ItemType Type { get; init; }

    [JsonPropertyName("uri")]
    public string? Uri { get; init; }

    [JsonPropertyName("artists")]
    public required SimplifiedArtist[] Artists { get; init; }
}
