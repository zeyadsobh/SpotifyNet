using System.Text.Json.Serialization;

namespace SpotifyNet.Datastructures.Spotify.Playlists;

/// <summary>A playlist, with its owner and a first page of its items.</summary>
public sealed record Playlist
{
    /// <summary>Whether the owner allows other users to modify the playlist.</summary>
    [JsonPropertyName("collaborative")]
    public bool? Collaborative { get; init; }

    /// <summary>
    /// The playlist's description; only returned for modified, verified playlists, otherwise
    /// <see langword="null"/>.
    /// </summary>
    [JsonPropertyName("description")]
    public string? Description { get; init; }

    /// <summary>Known external URLs for this playlist.</summary>
    [JsonPropertyName("external_urls")]
    public ExternalUrls? ExternalUrls { get; init; }

    /// <summary>A link to the Web API endpoint providing full details of the playlist.</summary>
    [JsonPropertyName("href")]
    public string? Href { get; init; }

    /// <summary>The Spotify ID for the playlist.</summary>
    [JsonPropertyName("id")]
    public string? Id { get; init; }

    /// <summary>
    /// Up to three images for the playlist, largest first; their URLs are temporary and expire in
    /// less than a day.
    /// </summary>
    [JsonPropertyName("images")]
    public Image[]? Images { get; init; }

    /// <summary>The name of the playlist.</summary>
    [JsonPropertyName("name")]
    public string? Name { get; init; }

    /// <summary>The user who owns the playlist.</summary>
    [JsonPropertyName("owner")]
    public PlaylistOwner? Owner { get; init; }

    /// <summary>
    /// Whether the playlist is public (if added to the user's profile) or private;
    /// <see langword="null"/> if that isn't relevant.
    /// </summary>
    [JsonPropertyName("public")]
    public bool? Public { get; init; }

    /// <summary>
    /// The version identifier for the current playlist; it can be supplied in other requests to
    /// target a specific version.
    /// </summary>
    [JsonPropertyName("snapshot_id")]
    public string? SnapshotId { get; init; }

    /// <summary>
    /// The playlist's items; only available for playlists the current user owns or collaborates on.
    /// </summary>
    [JsonPropertyName("items")]
    public OffsetPaginationWrapper<PlaylistTrack>? Items { get; init; }

    /// <summary>The object type: <see cref="ItemType.playlist"/>.</summary>
    [JsonPropertyName("type")]
    public ItemType? Type { get; init; }

    /// <summary>The Spotify URI for the playlist.</summary>
    [JsonPropertyName("uri")]
    public string? Uri { get; init; }
}
