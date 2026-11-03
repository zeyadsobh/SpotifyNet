using System.Text.Json.Serialization;
using SpotifyNet.Datastructures.Spotify.Artists;
using SpotifyNet.Datastructures.Spotify.Tracks;

namespace SpotifyNet.Datastructures.Spotify.Albums;

/// <summary>An album, with its tracks, copyrights and external IDs.</summary>
public sealed record Album
{
    /// <summary>The type of the album, such as a single or a compilation.</summary>
    [JsonPropertyName("album_type")]
    public required AlbumType AlbumType { get; init; }

    /// <summary>The number of tracks in the album.</summary>
    [JsonPropertyName("total_tracks")]
    public required int TotalTracks { get; init; }

    /// <summary>Known external URLs for this album.</summary>
    [JsonPropertyName("external_urls")]
    public required ExternalUrls ExternalUrls { get; init; }

    /// <summary>A link to the Web API endpoint providing full details of the album.</summary>
    [JsonPropertyName("href")]
    public required string Href { get; init; }

    /// <summary>The Spotify ID for the album.</summary>
    [JsonPropertyName("id")]
    public required string Id { get; init; }

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
    public required string ReleaseDate { get; init; }

    /// <summary>The precision with which <see cref="ReleaseDate"/> is known.</summary>
    [JsonPropertyName("release_date_precision")]
    public required ReleaseDatePrecision ReleaseDatePrecision { get; init; }

    /// <summary>Included in the response when a content restriction is applied.</summary>
    [JsonPropertyName("restrictions")]
    public Restrictions? Restrictions { get; init; }

    /// <summary>The object type: <see cref="ItemType.album"/>.</summary>
    [JsonPropertyName("type")]
    public required ItemType Type { get; init; }

    /// <summary>The Spotify URI for the album.</summary>
    [JsonPropertyName("uri")]
    public required string Uri { get; init; }

    /// <summary>The copyright statements of the album.</summary>
    [JsonPropertyName("copyrights")]
    public required Copyright[] Copyrights { get; init; }

    /// <summary>Known external IDs for the album.</summary>
    [JsonPropertyName("external_ids")]
    public required ExternalIds ExternalIds { get; init; }

    /// <summary>The artists of the album.</summary>
    [JsonPropertyName("artists")]
    public required SimplifiedArtist[] Artists { get; init; }

    /// <summary>The tracks of the album.</summary>
    [JsonPropertyName("tracks")]
    public required OffsetPaginationWrapper<SimplifiedTrack> Tracks { get; init; }
}
