using System.Text.Json.Serialization;
using SpotifyNet.Datastructures.Serialization;

namespace SpotifyNet.Datastructures.Spotify.Albums;

/// <summary>The type of an album.</summary>
[JsonConverter(typeof(StrictEnumConverter<AlbumType>))]
public enum AlbumType
{
    /// <summary>An album.</summary>
    album,

    /// <summary>A single.</summary>
    single,

    /// <summary>A compilation.</summary>
    compilation,

    /// <summary>An EP.</summary>
    // Not in the spec, but Spotify sends it (as "EP", e.g. in top tracks).
    ep,
}
