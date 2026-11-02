using System.Text.Json.Serialization;
using SpotifyNet.Datastructures.Spotify.Artists;
using SpotifyNet.Datastructures.Spotify.Tracks;

namespace SpotifyNet.Datastructures.Spotify.Albums;

public sealed record Album
{
    [JsonPropertyName("album_type")]
    public required AlbumType AlbumType { get; init; }

    [JsonPropertyName("total_tracks")]
    public required int TotalTracks { get; init; }

    [JsonPropertyName("external_urls")]
    public required ExternalUrls ExternalUrls { get; init; }

    [JsonPropertyName("href")]
    public required string Href { get; init; }

    [JsonPropertyName("id")]
    public required string Id { get; init; }

    [JsonPropertyName("images")]
    public required Image[] Images { get; init; }

    [JsonPropertyName("name")]
    public required string Name { get; init; }

    [JsonPropertyName("release_date")]
    public required string ReleaseDate { get; init; }

    [JsonPropertyName("release_date_precision")]
    public required ReleaseDatePrecision ReleaseDatePrecision { get; init; }

    [JsonPropertyName("restrictions")]
    public Restrictions? Restrictions { get; init; }

    [JsonPropertyName("type")]
    public required ItemType Type { get; init; }

    [JsonPropertyName("uri")]
    public required string Uri { get; init; }

    [JsonPropertyName("copyrights")]
    public required Copyright[] Copyrights { get; init; }

    [JsonPropertyName("external_ids")]
    public required ExternalIds ExternalIds { get; init; }

    [JsonPropertyName("artists")]
    public required SimplifiedArtist[] Artists { get; init; }

    [JsonPropertyName("tracks")]
    public required OffsetPaginationWrapper<SimplifiedTrack> Tracks { get; init; }
}
