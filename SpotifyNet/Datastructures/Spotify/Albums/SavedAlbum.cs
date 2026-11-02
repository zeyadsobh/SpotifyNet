using System;
using System.Text.Json.Serialization;

namespace SpotifyNet.Datastructures.Spotify.Albums;

public sealed record SavedAlbum
{
    [JsonPropertyName("added_at")]
    public DateTime? AddedAt { get; init; }

    [JsonPropertyName("album")]
    public Album? Album { get; init; }
}
