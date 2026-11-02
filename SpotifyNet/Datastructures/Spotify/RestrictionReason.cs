using System.Text.Json.Serialization;
using SpotifyNet.Datastructures.Serialization;

namespace SpotifyNet.Datastructures.Spotify;

[JsonConverter(typeof(UnknownFallbackEnumConverter<RestrictionReason>))]
public enum RestrictionReason
{
    unknown = 0,
    market,
    product,
    @explicit,
    payment_required,
}
