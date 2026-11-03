using System.Text.Json.Serialization;

namespace SpotifyNet.Datastructures.Spotify.Artists;

/// <summary>The artists the current user follows, as a cursor-paged list.</summary>
public sealed record FollowedArtists
{
    /// <summary>The followed artists.</summary>
    [JsonPropertyName("artists")]
    public required CursorPaginationWrapper<Artist> Artists { get; init; }
}
