using System;
using System.Text.Json.Serialization;

namespace SpotifyNet.Datastructures.Serialization;

/// <summary>Reads and writes enum members by name only; numbers are rejected.</summary>
internal sealed class StrictEnumConverter<TEnum>()
    : JsonStringEnumConverter<TEnum>(allowIntegerValues: false)
    where TEnum : struct, Enum;
