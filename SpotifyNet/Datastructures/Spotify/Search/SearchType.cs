using System.Text.Json.Serialization;
using SpotifyNet.Datastructures.Serialization;

namespace SpotifyNet.Datastructures.Spotify.Search;

/// <summary>A type of item a search can look for.</summary>
[JsonConverter(typeof(StrictEnumConverter<SearchType>))]
public enum SearchType
{
    /// <summary>Albums.</summary>
    album,

    /// <summary>Artists.</summary>
    artist,

    /// <summary>Playlists.</summary>
    playlist,

    /// <summary>Tracks.</summary>
    track,

    /// <summary>Podcast shows.</summary>
    show,

    /// <summary>Podcast episodes.</summary>
    episode,

    /// <summary>Audiobooks.</summary>
    audiobook,
}
