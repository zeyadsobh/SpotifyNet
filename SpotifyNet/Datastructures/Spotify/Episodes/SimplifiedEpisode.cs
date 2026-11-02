using System.Text.Json.Serialization;

namespace SpotifyNet.Datastructures.Spotify.Episodes;

public sealed record SimplifiedEpisode
{
    [JsonPropertyName("description")]
    public required string Description { get; init; }

    [JsonPropertyName("html_description")]
    public required string HtmlDescription { get; init; }

    [JsonPropertyName("duration_ms")]
    public required int DurationMs { get; init; }

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
    public bool? IsExternallyHosted { get; init; }

    [JsonPropertyName("is_playable")]
    public bool? IsPlayable { get; init; }

    [JsonPropertyName("languages")]
    public required string[] Languages { get; init; }

    [JsonPropertyName("name")]
    public required string Name { get; init; }

    [JsonPropertyName("release_date")]
    public required string ReleaseDate { get; init; }

    [JsonPropertyName("release_date_precision")]
    public required ReleaseDatePrecision ReleaseDatePrecision { get; init; }

    [JsonPropertyName("resume_point")]
    public ResumePoint? ResumePoint { get; init; }

    [JsonPropertyName("type")]
    public required ItemType Type { get; init; }

    [JsonPropertyName("uri")]
    public required string Uri { get; init; }

    [JsonPropertyName("restrictions")]
    public Restrictions? Restrictions { get; init; }
}
