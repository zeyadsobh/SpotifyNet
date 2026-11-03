using System.Text.Json.Serialization;
using SpotifyNet.Datastructures.Serialization;

namespace SpotifyNet.Datastructures.Spotify;

/// <summary>The type of a Spotify object.</summary>
[JsonConverter(typeof(UnknownFallbackEnumConverter<ItemType>))]
public enum ItemType
{
    /// <summary>A type this library doesn't know.</summary>
    unknown = 0,

    /// <summary>An album.</summary>
    album,

    /// <summary>An artist.</summary>
    artist,

    /// <summary>A track.</summary>
    track,

    /// <summary>A playlist.</summary>
    playlist,

    /// <summary>A user.</summary>
    user,

    /// <summary>A podcast show.</summary>
    show,

    /// <summary>A podcast episode.</summary>
    episode,

    /// <summary>An audiobook.</summary>
    audiobook,

    /// <summary>An audiobook chapter.</summary>
    chapter,
}
