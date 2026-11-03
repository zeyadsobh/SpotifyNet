using System.Text.Json.Serialization;
using SpotifyNet.Datastructures.Spotify.Albums;
using SpotifyNet.Datastructures.Spotify.Artists;
using SpotifyNet.Datastructures.Spotify.Audiobooks;
using SpotifyNet.Datastructures.Spotify.Episodes;
using SpotifyNet.Datastructures.Spotify.Playlists;
using SpotifyNet.Datastructures.Spotify.Shows;
using SpotifyNet.Datastructures.Spotify.Tracks;

namespace SpotifyNet.Datastructures.Spotify.Search;

/// <summary>The results of a search: a page of items for each type searched.</summary>
public sealed record SearchResult
{
    /// <summary>
    /// A page of matching tracks; <see langword="null"/> unless tracks were searched.
    /// </summary>
    [JsonPropertyName("tracks")]
    public OffsetPaginationWrapper<Track>? Tracks { get; init; }

    /// <summary>
    /// A page of matching artists; <see langword="null"/> unless artists were searched.
    /// </summary>
    [JsonPropertyName("artists")]
    public OffsetPaginationWrapper<Artist>? Artists { get; init; }

    /// <summary>
    /// A page of matching albums; <see langword="null"/> unless albums were searched.
    /// </summary>
    [JsonPropertyName("albums")]
    public OffsetPaginationWrapper<SimplifiedAlbum>? Albums { get; init; }

    /// <summary>
    /// A page of matching playlists; <see langword="null"/> unless playlists were searched.
    /// </summary>
    [JsonPropertyName("playlists")]
    public OffsetPaginationWrapper<SimplifiedPlaylist>? Playlists { get; init; }

    /// <summary>
    /// A page of matching shows; <see langword="null"/> unless shows were searched.
    /// </summary>
    [JsonPropertyName("shows")]
    public OffsetPaginationWrapper<SimplifiedShow>? Shows { get; init; }

    /// <summary>
    /// A page of matching episodes; <see langword="null"/> unless episodes were searched.
    /// </summary>
    [JsonPropertyName("episodes")]
    public OffsetPaginationWrapper<SimplifiedEpisode>? Episodes { get; init; }

    /// <summary>
    /// A page of matching audiobooks; <see langword="null"/> unless audiobooks were searched.
    /// </summary>
    [JsonPropertyName("audiobooks")]
    public OffsetPaginationWrapper<SimplifiedAudiobook>? Audiobooks { get; init; }
}
