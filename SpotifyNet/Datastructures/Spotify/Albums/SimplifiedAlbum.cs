using System.Text.Json.Serialization;
using SpotifyNet.Datastructures.Spotify.Artists;

namespace SpotifyNet.Datastructures.Spotify.Albums;

// Looser than the spec: local files in playlists have albums with the optional fields null or missing.
/// <summary>An album without its tracks, copyrights and external IDs.</summary>
public sealed record SimplifiedAlbum
{
    /// <summary>The type of the album, such as a single or a compilation.</summary>
    [JsonPropertyName("album_type")]
    public AlbumType? AlbumType { get; init; }

    /// <summary>The number of tracks in the album.</summary>
    [JsonPropertyName("total_tracks")]
    public int? TotalTracks { get; init; }

    /// <summary>Known external URLs for this album.</summary>
    [JsonPropertyName("external_urls")]
    public required ExternalUrls ExternalUrls { get; init; }

    /// <summary>A link to the Web API endpoint providing full details of the album.</summary>
    [JsonPropertyName("href")]
    public string? Href { get; init; }

    /// <summary>The Spotify ID for the album.</summary>
    [JsonPropertyName("id")]
    public string? Id { get; init; }

    /// <summary>The cover art for the album in various sizes, widest first.</summary>
    [JsonPropertyName("images")]
    public required Image[] Images { get; init; }

    /// <summary>The name of the album; may be empty after an album takedown.</summary>
    [JsonPropertyName("name")]
    public required string Name { get; init; }

    /// <summary>
    /// The date the album was first released, as <c>1981-12-15</c>, <c>1981-12</c> or <c>1981</c>
    /// depending on <see cref="ReleaseDatePrecision"/>.
    /// </summary>
    [JsonPropertyName("release_date")]
    public string? ReleaseDate { get; init; }

    /// <summary>The precision with which <see cref="ReleaseDate"/> is known.</summary>
    [JsonPropertyName("release_date_precision")]
    public ReleaseDatePrecision? ReleaseDatePrecision { get; init; }

    /// <summary>Included in the response when a content restriction is applied.</summary>
    [JsonPropertyName("restrictions")]
    public Restrictions? Restrictions { get; init; }

    /// <summary>The object type: <see cref="ItemType.album"/>.</summary>
    [JsonPropertyName("type")]
    public required ItemType Type { get; init; }

    /// <summary>The Spotify URI for the album.</summary>
    [JsonPropertyName("uri")]
    public string? Uri { get; init; }

    /// <summary>The artists of the album.</summary>
    [JsonPropertyName("artists")]
    public required SimplifiedArtist[] Artists { get; init; }
}
