using System.Text.Json.Serialization;
using SpotifyNet.Datastructures.Serialization;

namespace SpotifyNet.Datastructures.Spotify;

/// <summary>Why content is restricted.</summary>
[JsonConverter(typeof(UnknownFallbackEnumConverter<RestrictionReason>))]
public enum RestrictionReason
{
    /// <summary>A reason this library doesn't know; Spotify may add more.</summary>
    unknown = 0,

    /// <summary>The content isn't available in the given market.</summary>
    market,

    /// <summary>The content isn't available for the user's subscription type.</summary>
    product,

    /// <summary>
    /// The content is explicit and the user's account is set to not play explicit content.
    /// </summary>
    @explicit,

    /// <summary>Payment is required to play the content.</summary>
    payment_required,
}
