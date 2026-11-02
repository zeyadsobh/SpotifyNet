using System.Text.Json.Serialization;
using SpotifyNet.Datastructures.Serialization;

namespace SpotifyNet.Datastructures.Spotify.Albums;

[JsonConverter(typeof(StrictEnumConverter<AlbumType>))]
public enum AlbumType
{
    album,
    single,
    compilation,

    // Not in the spec, but Spotify sends it (as "EP", e.g. in top tracks).
    ep,
}
