using System.Text.Json.Serialization;
using SpotifyNet.Datastructures.Serialization;

namespace SpotifyNet.Datastructures.Spotify;

/// <summary>A track or episode; <see cref="UnknownPlayableItem"/> for unrecognized types.</summary>
[JsonConverter(typeof(PlayableItemConverter))]
public abstract record PlayableItem;
