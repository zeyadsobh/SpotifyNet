using System.Text.Json.Serialization;
using SpotifyNet.Datastructures.Serialization;

namespace SpotifyNet.Datastructures.Spotify.Users;

/// <summary>The time frame top items are computed over.</summary>
[JsonConverter(typeof(StrictEnumConverter<TimeRange>))]
public enum TimeRange
{
    /// <summary>About the last 4 weeks.</summary>
    short_term,

    /// <summary>About the last 6 months.</summary>
    medium_term,

    /// <summary>About the last year, including new data as it comes in.</summary>
    long_term,
}
