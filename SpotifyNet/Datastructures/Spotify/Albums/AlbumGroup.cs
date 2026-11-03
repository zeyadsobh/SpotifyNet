using System.Text.Json.Serialization;
using SpotifyNet.Datastructures.Serialization;

namespace SpotifyNet.Datastructures.Spotify.Albums;

/// <summary>An album's relationship to an artist, for filtering their discography.</summary>
[JsonConverter(typeof(StrictEnumConverter<AlbumGroup>))]
public enum AlbumGroup
{
    /// <summary>An album by the artist.</summary>
    album,

    /// <summary>A single by the artist.</summary>
    single,

    /// <summary>An album the artist appears on.</summary>
    appears_on,

    /// <summary>A compilation featuring the artist.</summary>
    compilation,
}
