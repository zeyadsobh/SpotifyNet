using System.Text.Json.Serialization;
using SpotifyNet.Datastructures.Spotify.Albums;
using SpotifyNet.Datastructures.Spotify.Artists;

namespace SpotifyNet.Datastructures.Spotify.Tracks;

public sealed record Track : PlayableItem, ITopItem
{
    static string ITopItem.TopItemType => "tracks";

    [JsonPropertyName("album")]
    public SimplifiedAlbum? Album { get; init; }

    [JsonPropertyName("artists")]
    public SimplifiedArtist[]? Artists { get; init; }

    [JsonPropertyName("disc_number")]
    public int? DiscNumber { get; init; }

    [JsonPropertyName("duration_ms")]
    public int? DurationMs { get; init; }

    [JsonPropertyName("explicit")]
    public bool? Explicit { get; init; }

    [JsonPropertyName("external_ids")]
    public ExternalIds? ExternalIds { get; init; }

    [JsonPropertyName("external_urls")]
    public ExternalUrls? ExternalUrls { get; init; }

    [JsonPropertyName("href")]
    public string? Href { get; init; }

    [JsonPropertyName("id")]
    public string? Id { get; init; }

    [JsonPropertyName("is_playable")]
    public bool? IsPlayable { get; init; }

    [JsonPropertyName("restrictions")]
    public Restrictions? Restrictions { get; init; }

    [JsonPropertyName("name")]
    public string? Name { get; init; }

    [JsonPropertyName("track_number")]
    public int? TrackNumber { get; init; }

    [JsonPropertyName("type")]
    public ItemType? Type { get; init; }

    [JsonPropertyName("uri")]
    public string? Uri { get; init; }

    [JsonPropertyName("is_local")]
    public bool? IsLocal { get; init; }
}
