using System.Text.Json.Serialization;
using SpotifyNet.Datastructures.Serialization;

namespace SpotifyNet.Datastructures.Spotify.Search;

[JsonConverter(typeof(StrictEnumConverter<SearchType>))]
public enum SearchType
{
    album,
    artist,
    playlist,
    track,
    show,
    episode,
    audiobook,
}
