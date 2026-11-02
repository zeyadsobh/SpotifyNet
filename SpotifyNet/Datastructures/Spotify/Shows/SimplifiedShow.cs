using System.Text.Json.Serialization;

namespace SpotifyNet.Datastructures.Spotify.Shows;

public sealed record SimplifiedShow
{
    [JsonPropertyName("copyrights")]
    public required Copyright[] Copyrights { get; init; }

    [JsonPropertyName("description")]
    public required string Description { get; init; }

    [JsonPropertyName("html_description")]
    public required string HtmlDescription { get; init; }

    [JsonPropertyName("explicit")]
    public required bool Explicit { get; init; }

    [JsonPropertyName("external_urls")]
    public required ExternalUrls ExternalUrls { get; init; }

    [JsonPropertyName("href")]
    public required string Href { get; init; }

    [JsonPropertyName("id")]
    public required string Id { get; init; }

    [JsonPropertyName("images")]
    public required Image[] Images { get; init; }

    [JsonPropertyName("is_externally_hosted")]
    public required bool? IsExternallyHosted { get; init; }

    [JsonPropertyName("languages")]
    public required string[] Languages { get; init; }

    [JsonPropertyName("media_type")]
    public required string MediaType { get; init; }

    [JsonPropertyName("name")]
    public required string Name { get; init; }

    [JsonPropertyName("type")]
    public required ItemType Type { get; init; }

    [JsonPropertyName("uri")]
    public required string Uri { get; init; }

    [JsonPropertyName("total_episodes")]
    public int? TotalEpisodes { get; init; }
}
