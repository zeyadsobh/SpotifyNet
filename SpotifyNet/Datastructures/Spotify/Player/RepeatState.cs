using System.Text.Json.Serialization;
using SpotifyNet.Datastructures.Serialization;

namespace SpotifyNet.Datastructures.Spotify.Player;

[JsonConverter(typeof(StrictEnumConverter<RepeatState>))]
public enum RepeatState
{
    /// <summary>Repeat the current track.</summary>
    track,

    /// <summary>Repeat the current context.</summary>
    context,

    off,
}
