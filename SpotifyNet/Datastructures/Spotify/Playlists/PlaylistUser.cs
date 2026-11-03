using System.Text.Json.Serialization;

namespace SpotifyNet.Datastructures.Spotify.Playlists;

/// <summary>The user who added an item to a playlist.</summary>
public sealed record PlaylistUser
{
    /// <summary>Known public external URLs for this user.</summary>
    [JsonPropertyName("external_urls")]
    public ExternalUrls? ExternalUrls { get; init; }

    /// <summary>A link to the Web API endpoint for this user.</summary>
    [JsonPropertyName("href")]
    public string? Href { get; init; }

    /// <summary>The Spotify user ID for this user.</summary>
    [JsonPropertyName("id")]
    public string? Id { get; init; }

    /// <summary>The object type: <see cref="ItemType.user"/>.</summary>
    [JsonPropertyName("type")]
    public ItemType? Type { get; init; }

    /// <summary>The Spotify URI for this user.</summary>
    [JsonPropertyName("uri")]
    public string? Uri { get; init; }
}
