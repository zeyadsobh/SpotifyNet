using System.Text.Json.Serialization;
using SpotifyNet.Datastructures.Serialization;

namespace SpotifyNet.Datastructures.Spotify;

[JsonConverter(typeof(StrictEnumConverter<ReleaseDatePrecision>))]
public enum ReleaseDatePrecision
{
    year,
    month,
    day,
}
