using System.Text.Json.Serialization;
using SpotifyNet.Datastructures.Serialization;

namespace SpotifyNet.Datastructures.Spotify;

/// <summary>How precisely a release date is known.</summary>
[JsonConverter(typeof(StrictEnumConverter<ReleaseDatePrecision>))]
public enum ReleaseDatePrecision
{
    /// <summary>Known to the year, such as <c>1981</c>.</summary>
    year,

    /// <summary>Known to the month, such as <c>1981-12</c>.</summary>
    month,

    /// <summary>Known to the day, such as <c>1981-12-15</c>.</summary>
    day,
}
