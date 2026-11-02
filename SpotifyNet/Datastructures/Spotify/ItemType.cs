using System.Text.Json.Serialization;
using SpotifyNet.Datastructures.Serialization;

namespace SpotifyNet.Datastructures.Spotify;

[JsonConverter(typeof(UnknownFallbackEnumConverter<ItemType>))]
public enum ItemType
{
    unknown = 0,
    album,
    artist,
    track,
    playlist,
    user,
    show,
    episode,
    audiobook,
    chapter,
}
