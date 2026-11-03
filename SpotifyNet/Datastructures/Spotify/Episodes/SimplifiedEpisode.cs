using System.Text.Json.Serialization;

namespace SpotifyNet.Datastructures.Spotify.Episodes;

/// <summary>A podcast episode without its show.</summary>
public sealed record SimplifiedEpisode
{
    /// <summary>A description of the episode, with HTML tags stripped away.</summary>
    [JsonPropertyName("description")]
    public required string Description { get; init; }

    /// <summary>A description of the episode, which may contain HTML tags.</summary>
    [JsonPropertyName("html_description")]
    public required string HtmlDescription { get; init; }

    /// <summary>The episode length in milliseconds.</summary>
    [JsonPropertyName("duration_ms")]
    public required int DurationMs { get; init; }

    /// <summary>
    /// Whether the episode has explicit content; <see langword="false"/> if it doesn't or that's
    /// unknown.
    /// </summary>
    [JsonPropertyName("explicit")]
    public required bool Explicit { get; init; }

    /// <summary>External URLs for this episode.</summary>
    [JsonPropertyName("external_urls")]
    public required ExternalUrls ExternalUrls { get; init; }

    /// <summary>A link to the Web API endpoint providing full details of the episode.</summary>
    [JsonPropertyName("href")]
    public required string Href { get; init; }

    /// <summary>The Spotify ID for the episode.</summary>
    [JsonPropertyName("id")]
    public required string Id { get; init; }

    /// <summary>The cover art for the episode in various sizes, widest first.</summary>
    [JsonPropertyName("images")]
    public required Image[] Images { get; init; }

    /// <summary>Whether the episode is hosted outside Spotify's CDN.</summary>
    [JsonPropertyName("is_externally_hosted")]
    public bool? IsExternallyHosted { get; init; }

    /// <summary>Whether the episode is playable in the given market.</summary>
    [JsonPropertyName("is_playable")]
    public bool? IsPlayable { get; init; }

    /// <summary>The languages used in the episode, as ISO 639-1 codes.</summary>
    [JsonPropertyName("languages")]
    public required string[] Languages { get; init; }

    /// <summary>The name of the episode.</summary>
    [JsonPropertyName("name")]
    public required string Name { get; init; }

    /// <summary>
    /// The date the episode was first released, as <c>1981-12-15</c>, <c>1981-12</c> or <c>1981</c>
    /// depending on <see cref="ReleaseDatePrecision"/>.
    /// </summary>
    [JsonPropertyName("release_date")]
    public required string ReleaseDate { get; init; }

    /// <summary>The precision with which <see cref="ReleaseDate"/> is known.</summary>
    [JsonPropertyName("release_date_precision")]
    public required ReleaseDatePrecision ReleaseDatePrecision { get; init; }

    /// <summary>
    /// The user's most recent position in the episode; only set for a user token with the
    /// <c>user-read-playback-position</c> scope.
    /// </summary>
    [JsonPropertyName("resume_point")]
    public ResumePoint? ResumePoint { get; init; }

    /// <summary>The object type: <see cref="ItemType.episode"/>.</summary>
    [JsonPropertyName("type")]
    public required ItemType Type { get; init; }

    /// <summary>The Spotify URI for the episode.</summary>
    [JsonPropertyName("uri")]
    public required string Uri { get; init; }

    /// <summary>Included in the response when a content restriction is applied.</summary>
    [JsonPropertyName("restrictions")]
    public Restrictions? Restrictions { get; init; }
}
