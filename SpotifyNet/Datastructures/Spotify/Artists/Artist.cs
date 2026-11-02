using System.Text.Json.Serialization;

namespace SpotifyNet.Datastructures.Spotify.Artists;

public sealed record Artist : ITopItem
{
    static string ITopItem.TopItemType => "artists";

    [JsonPropertyName("external_urls")]
    public ExternalUrls? ExternalUrls { get; init; }

    [JsonPropertyName("href")]
    public string? Href { get; init; }

    [JsonPropertyName("id")]
    public string? Id { get; init; }

    [JsonPropertyName("images")]
    public Image[]? Images { get; init; }

    [JsonPropertyName("name")]
    public string? Name { get; init; }

    [JsonPropertyName("type")]
    public ItemType? Type { get; init; }

    [JsonPropertyName("uri")]
    public string? Uri { get; init; }
}
