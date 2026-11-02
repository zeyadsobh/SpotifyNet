using System.Text.Json.Serialization;
using SpotifyNet.Datastructures.Serialization;

namespace SpotifyNet.Datastructures.Spotify.Albums;

/// <summary>An album's relationship to an artist, for filtering their discography.</summary>
[JsonConverter(typeof(StrictEnumConverter<AlbumGroup>))]
public enum AlbumGroup
{
    album,
    single,
    appears_on,
    compilation,
}
