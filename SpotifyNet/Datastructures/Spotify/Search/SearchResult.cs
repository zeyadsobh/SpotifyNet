using System.Text.Json.Serialization;
using SpotifyNet.Datastructures.Spotify.Albums;
using SpotifyNet.Datastructures.Spotify.Artists;
using SpotifyNet.Datastructures.Spotify.Audiobooks;
using SpotifyNet.Datastructures.Spotify.Episodes;
using SpotifyNet.Datastructures.Spotify.Playlists;
using SpotifyNet.Datastructures.Spotify.Shows;
using SpotifyNet.Datastructures.Spotify.Tracks;

namespace SpotifyNet.Datastructures.Spotify.Search;

public sealed record SearchResult
{
    [JsonPropertyName("tracks")]
    public OffsetPaginationWrapper<Track>? Tracks { get; init; }

    [JsonPropertyName("artists")]
    public OffsetPaginationWrapper<Artist>? Artists { get; init; }

    [JsonPropertyName("albums")]
    public OffsetPaginationWrapper<SimplifiedAlbum>? Albums { get; init; }

    [JsonPropertyName("playlists")]
    public OffsetPaginationWrapper<SimplifiedPlaylist>? Playlists { get; init; }

    [JsonPropertyName("shows")]
    public OffsetPaginationWrapper<SimplifiedShow>? Shows { get; init; }

    [JsonPropertyName("episodes")]
    public OffsetPaginationWrapper<SimplifiedEpisode>? Episodes { get; init; }

    [JsonPropertyName("audiobooks")]
    public OffsetPaginationWrapper<SimplifiedAudiobook>? Audiobooks { get; init; }
}
