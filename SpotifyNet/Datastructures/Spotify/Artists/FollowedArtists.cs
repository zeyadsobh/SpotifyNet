using System.Text.Json.Serialization;

namespace SpotifyNet.Datastructures.Spotify.Artists;

public sealed record FollowedArtists
{
    [JsonPropertyName("artists")]
    public required CursorPaginationWrapper<Artist> Artists { get; init; }
}
