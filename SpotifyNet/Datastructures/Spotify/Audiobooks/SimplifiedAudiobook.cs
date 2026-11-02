using System.Text.Json.Serialization;

namespace SpotifyNet.Datastructures.Spotify.Audiobooks;

public sealed record SimplifiedAudiobook
{
    [JsonPropertyName("authors")]
    public required Author[] Authors { get; init; }

    [JsonPropertyName("copyrights")]
    public required Copyright[] Copyrights { get; init; }

    [JsonPropertyName("description")]
    public required string Description { get; init; }

    [JsonPropertyName("html_description")]
    public required string HtmlDescription { get; init; }

    [JsonPropertyName("edition")]
    public string? Edition { get; init; }

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

    [JsonPropertyName("languages")]
    public required string[] Languages { get; init; }

    [JsonPropertyName("media_type")]
    public required string MediaType { get; init; }

    [JsonPropertyName("name")]
    public required string Name { get; init; }

    [JsonPropertyName("narrators")]
    public required Narrator[] Narrators { get; init; }

    [JsonPropertyName("type")]
    public required ItemType Type { get; init; }

    [JsonPropertyName("uri")]
    public required string Uri { get; init; }

    [JsonPropertyName("total_chapters")]
    public required int TotalChapters { get; init; }
}
