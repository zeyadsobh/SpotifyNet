using System.Text.Json.Serialization;
using SpotifyNet.Datastructures.Spotify.Albums;
using SpotifyNet.Datastructures.Spotify.Artists;

namespace SpotifyNet.Datastructures.Spotify.Tracks;

/// <summary>A track, with its album and external IDs.</summary>
public sealed record Track : PlayableItem, ITopItem
{
    static string ITopItem.TopItemType => "tracks";

    /// <summary>The album on which the track appears.</summary>
    [JsonPropertyName("album")]
    public SimplifiedAlbum? Album { get; init; }

    /// <summary>The artists who performed the track.</summary>
    [JsonPropertyName("artists")]
    public SimplifiedArtist[]? Artists { get; init; }

    /// <summary>
    /// The disc number, usually <c>1</c> unless the album consists of more than one disc.
    /// </summary>
    [JsonPropertyName("disc_number")]
    public int? DiscNumber { get; init; }

    /// <summary>The track length in milliseconds.</summary>
    [JsonPropertyName("duration_ms")]
    public int? DurationMs { get; init; }

    /// <summary>
    /// Whether the track has explicit lyrics; <see langword="false"/> if it doesn't or that's
    /// unknown.
    /// </summary>
    [JsonPropertyName("explicit")]
    public bool? Explicit { get; init; }

    /// <summary>Known external IDs for the track.</summary>
    [JsonPropertyName("external_ids")]
    public ExternalIds? ExternalIds { get; init; }

    /// <summary>Known external URLs for this track.</summary>
    [JsonPropertyName("external_urls")]
    public ExternalUrls? ExternalUrls { get; init; }

    /// <summary>A link to the Web API endpoint providing full details of the track.</summary>
    [JsonPropertyName("href")]
    public string? Href { get; init; }

    /// <summary>The Spotify ID for the track.</summary>
    [JsonPropertyName("id")]
    public string? Id { get; init; }

    /// <summary>
    /// Whether the track is playable in the given market; only part of the response when track
    /// relinking is applied.
    /// </summary>
    [JsonPropertyName("is_playable")]
    public bool? IsPlayable { get; init; }

    /// <summary>Included in the response when a content restriction is applied.</summary>
    [JsonPropertyName("restrictions")]
    public Restrictions? Restrictions { get; init; }

    /// <summary>The name of the track.</summary>
    [JsonPropertyName("name")]
    public string? Name { get; init; }

    /// <summary>
    /// The number of the track; if an album has several discs, its number on the specified disc.
    /// </summary>
    [JsonPropertyName("track_number")]
    public int? TrackNumber { get; init; }

    /// <summary>The object type: <see cref="ItemType.track"/>.</summary>
    [JsonPropertyName("type")]
    public ItemType? Type { get; init; }

    /// <summary>The Spotify URI for the track.</summary>
    [JsonPropertyName("uri")]
    public string? Uri { get; init; }

    /// <summary>Whether the track is from a local file.</summary>
    [JsonPropertyName("is_local")]
    public bool? IsLocal { get; init; }
}
