using System.Text.Json.Serialization;

namespace SpotifyNet.Datastructures.Spotify.Users;

/// <summary>The current user's profile.</summary>
public sealed record User
{
    /// <summary>
    /// A public, immutable, pseudoanonymous identifier for the user's account; use it rather than
    /// <see cref="Id"/> for account linking, as it never changes.
    /// </summary>
    [JsonPropertyName("account_id")]
    public string? AccountId { get; init; }

    /// <summary>
    /// The name displayed on the user's profile; <see langword="null"/> if not available.
    /// </summary>
    [JsonPropertyName("display_name")]
    public string? DisplayName { get; init; }

    /// <summary>Known external URLs for this user.</summary>
    [JsonPropertyName("external_urls")]
    public ExternalUrls? ExternalUrls { get; init; }

    /// <summary>A link to the Web API endpoint for this user.</summary>
    [JsonPropertyName("href")]
    public string? Href { get; init; }

    /// <summary>
    /// The Spotify user ID for the user; for account linking, use <see cref="AccountId"/> instead,
    /// which is immutable.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; init; }

    /// <summary>The user's profile image.</summary>
    [JsonPropertyName("images")]
    public Image[]? Images { get; init; }

    /// <summary>The object type: <see cref="ItemType.user"/>.</summary>
    [JsonPropertyName("type")]
    public ItemType? Type { get; init; }

    /// <summary>The Spotify URI for the user.</summary>
    [JsonPropertyName("uri")]
    public string? Uri { get; init; }
}
