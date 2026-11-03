using System.Text.Json.Serialization;
using SpotifyNet.Datastructures.Spotify.Episodes;

namespace SpotifyNet.Datastructures.Spotify.Shows;

/// <summary>A podcast show, with its episodes.</summary>
public sealed record Show
{
    /// <summary>The copyright statements of the show.</summary>
    [JsonPropertyName("copyrights")]
    public required Copyright[] Copyrights { get; init; }

    /// <summary>A description of the show, with HTML tags stripped away.</summary>
    [JsonPropertyName("description")]
    public required string Description { get; init; }

    /// <summary>A description of the show, which may contain HTML tags.</summary>
    [JsonPropertyName("html_description")]
    public required string HtmlDescription { get; init; }

    /// <summary>
    /// Whether the show has explicit content; <see langword="false"/> if it doesn't or that's
    /// unknown.
    /// </summary>
    [JsonPropertyName("explicit")]
    public required bool Explicit { get; init; }

    /// <summary>External URLs for this show.</summary>
    [JsonPropertyName("external_urls")]
    public required ExternalUrls ExternalUrls { get; init; }

    /// <summary>A link to the Web API endpoint providing full details of the show.</summary>
    [JsonPropertyName("href")]
    public required string Href { get; init; }

    /// <summary>The Spotify ID for the show.</summary>
    [JsonPropertyName("id")]
    public required string Id { get; init; }

    /// <summary>The cover art for the show in various sizes, widest first.</summary>
    [JsonPropertyName("images")]
    public required Image[] Images { get; init; }

    /// <summary>
    /// Whether all the show's episodes are hosted outside Spotify's CDN; might be
    /// <see langword="null"/>.
    /// </summary>
    [JsonPropertyName("is_externally_hosted")]
    public required bool? IsExternallyHosted { get; init; }

    /// <summary>The languages used in the show, as ISO 639 codes.</summary>
    [JsonPropertyName("languages")]
    public required string[] Languages { get; init; }

    /// <summary>The media type of the show.</summary>
    [JsonPropertyName("media_type")]
    public required string MediaType { get; init; }

    /// <summary>The name of the show.</summary>
    [JsonPropertyName("name")]
    public required string Name { get; init; }

    /// <summary>The object type: <see cref="ItemType.show"/>.</summary>
    [JsonPropertyName("type")]
    public required ItemType Type { get; init; }

    /// <summary>The Spotify URI for the show.</summary>
    [JsonPropertyName("uri")]
    public required string Uri { get; init; }

    /// <summary>The total number of episodes in the show.</summary>
    [JsonPropertyName("total_episodes")]
    public required int TotalEpisodes { get; init; }

    /// <summary>The episodes of the show.</summary>
    [JsonPropertyName("episodes")]
    public required OffsetPaginationWrapper<SimplifiedEpisode> Episodes { get; init; }
}
