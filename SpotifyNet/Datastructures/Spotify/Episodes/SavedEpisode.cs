using System;
using System.Text.Json.Serialization;
using SpotifyNet.Datastructures.Serialization;

namespace SpotifyNet.Datastructures.Spotify.Episodes;

public sealed record SavedEpisode
{
    [JsonPropertyName("added_at")]
    public DateTime? AddedAt { get; init; }

    // Null for episodes that are no longer available, which Spotify sends as placeholders.
    [JsonPropertyName("episode")]
    [JsonConverter(typeof(UnavailableAsNullConverter<Episode>))]
    public Episode? Episode { get; init; }
}
