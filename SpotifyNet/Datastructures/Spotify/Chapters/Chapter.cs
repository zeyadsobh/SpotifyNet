using System.Text.Json.Serialization;
using SpotifyNet.Datastructures.Spotify.Audiobooks;

namespace SpotifyNet.Datastructures.Spotify.Chapters;

/// <summary>An audiobook chapter, with its audiobook.</summary>
public sealed record Chapter
{
    /// <summary>The chapter's number in the audiobook.</summary>
    [JsonPropertyName("chapter_number")]
    public required int ChapterNumber { get; init; }

    /// <summary>A description of the chapter, with HTML tags stripped away.</summary>
    [JsonPropertyName("description")]
    public required string Description { get; init; }

    /// <summary>A description of the chapter, which may contain HTML tags.</summary>
    [JsonPropertyName("html_description")]
    public required string HtmlDescription { get; init; }

    /// <summary>The chapter length in milliseconds.</summary>
    [JsonPropertyName("duration_ms")]
    public required int DurationMs { get; init; }

    /// <summary>
    /// Whether the chapter has explicit content; <see langword="false"/> if it doesn't or that's
    /// unknown.
    /// </summary>
    [JsonPropertyName("explicit")]
    public required bool Explicit { get; init; }

    /// <summary>External URLs for this chapter.</summary>
    [JsonPropertyName("external_urls")]
    public required ExternalUrls ExternalUrls { get; init; }

    /// <summary>A link to the Web API endpoint providing full details of the chapter.</summary>
    [JsonPropertyName("href")]
    public required string Href { get; init; }

    /// <summary>The Spotify ID for the chapter.</summary>
    [JsonPropertyName("id")]
    public required string Id { get; init; }

    /// <summary>The cover art for the chapter in various sizes, widest first.</summary>
    [JsonPropertyName("images")]
    public required Image[] Images { get; init; }

    /// <summary>Whether the chapter is playable in the given market.</summary>
    [JsonPropertyName("is_playable")]
    public bool? IsPlayable { get; init; }

    /// <summary>The languages used in the chapter, as ISO 639-1 codes.</summary>
    [JsonPropertyName("languages")]
    public required string[] Languages { get; init; }

    /// <summary>The name of the chapter.</summary>
    [JsonPropertyName("name")]
    public required string Name { get; init; }

    /// <summary>
    /// The date the chapter was first released, as <c>1981-12-15</c>, <c>1981-12</c> or <c>1981</c>
    /// depending on <see cref="ReleaseDatePrecision"/>.
    /// </summary>
    [JsonPropertyName("release_date")]
    public required string ReleaseDate { get; init; }

    /// <summary>The precision with which <see cref="ReleaseDate"/> is known.</summary>
    [JsonPropertyName("release_date_precision")]
    public required ReleaseDatePrecision ReleaseDatePrecision { get; init; }

    /// <summary>
    /// The user's most recent position in the chapter; only set for a user token with the
    /// <c>user-read-playback-position</c> scope.
    /// </summary>
    [JsonPropertyName("resume_point")]
    public ResumePoint? ResumePoint { get; init; }

    /// <summary>The object type: <see cref="ItemType.episode"/>.</summary>
    [JsonPropertyName("type")]
    public required ItemType Type { get; init; }

    /// <summary>The Spotify URI for the chapter.</summary>
    [JsonPropertyName("uri")]
    public required string Uri { get; init; }

    /// <summary>Included in the response when a content restriction is applied.</summary>
    [JsonPropertyName("restrictions")]
    public Restrictions? Restrictions { get; init; }

    /// <summary>The audiobook the chapter belongs to.</summary>
    [JsonPropertyName("audiobook")]
    public required SimplifiedAudiobook Audiobook { get; init; }
}
