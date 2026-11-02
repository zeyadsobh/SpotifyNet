using System.Text.Json.Serialization;

namespace SpotifyNet.Datastructures.Spotify.Users;

public sealed record User
{
    [JsonPropertyName("account_id")]
    public string? AccountId { get; init; }

    [JsonPropertyName("display_name")]
    public string? DisplayName { get; init; }

    [JsonPropertyName("external_urls")]
    public ExternalUrls? ExternalUrls { get; init; }

    [JsonPropertyName("href")]
    public string? Href { get; init; }

    [JsonPropertyName("id")]
    public string? Id { get; init; }

    [JsonPropertyName("images")]
    public Image[]? Images { get; init; }

    [JsonPropertyName("type")]
    public ItemType? Type { get; init; }

    [JsonPropertyName("uri")]
    public string? Uri { get; init; }
}
