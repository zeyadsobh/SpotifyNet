using System.Text.Json.Serialization;

namespace SpotifyNet.Datastructures.Spotify.Audiobooks;

/// <summary>An audiobook without its chapters.</summary>
public sealed record SimplifiedAudiobook
{
    /// <summary>The authors of the audiobook.</summary>
    [JsonPropertyName("authors")]
    public required Author[] Authors { get; init; }

    /// <summary>The copyright statements of the audiobook.</summary>
    [JsonPropertyName("copyrights")]
    public required Copyright[] Copyrights { get; init; }

    /// <summary>A description of the audiobook, with HTML tags stripped away.</summary>
    [JsonPropertyName("description")]
    public required string Description { get; init; }

    /// <summary>A description of the audiobook, which may contain HTML tags.</summary>
    [JsonPropertyName("html_description")]
    public required string HtmlDescription { get; init; }

    /// <summary>The edition of the audiobook.</summary>
    [JsonPropertyName("edition")]
    public string? Edition { get; init; }

    /// <summary>
    /// Whether the audiobook has explicit content; <see langword="false"/> if it doesn't or that's
    /// unknown.
    /// </summary>
    [JsonPropertyName("explicit")]
    public required bool Explicit { get; init; }

    /// <summary>External URLs for this audiobook.</summary>
    [JsonPropertyName("external_urls")]
    public required ExternalUrls ExternalUrls { get; init; }

    /// <summary>A link to the Web API endpoint providing full details of the audiobook.</summary>
    [JsonPropertyName("href")]
    public required string Href { get; init; }

    /// <summary>The Spotify ID for the audiobook.</summary>
    [JsonPropertyName("id")]
    public required string Id { get; init; }

    /// <summary>The cover art for the audiobook in various sizes, widest first.</summary>
    [JsonPropertyName("images")]
    public required Image[] Images { get; init; }

    /// <summary>The languages used in the audiobook, as ISO 639 codes.</summary>
    [JsonPropertyName("languages")]
    public required string[] Languages { get; init; }

    /// <summary>The media type of the audiobook.</summary>
    [JsonPropertyName("media_type")]
    public required string MediaType { get; init; }

    /// <summary>The name of the audiobook.</summary>
    [JsonPropertyName("name")]
    public required string Name { get; init; }

    /// <summary>The narrators of the audiobook.</summary>
    [JsonPropertyName("narrators")]
    public required Narrator[] Narrators { get; init; }

    /// <summary>The object type: <see cref="ItemType.audiobook"/>.</summary>
    [JsonPropertyName("type")]
    public required ItemType Type { get; init; }

    /// <summary>The Spotify URI for the audiobook.</summary>
    [JsonPropertyName("uri")]
    public required string Uri { get; init; }

    /// <summary>The number of chapters in this audiobook.</summary>
    [JsonPropertyName("total_chapters")]
    public required int TotalChapters { get; init; }
}
