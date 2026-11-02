using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SpotifyNet.Datastructures.Serialization;

/// <summary>Reads names the enum doesn't define as its <c>unknown</c> member.</summary>
internal sealed class UnknownFallbackEnumConverter<TEnum> : JsonConverter<TEnum>
    where TEnum : struct, Enum
{
    private static readonly TEnum Unknown = Enum.Parse<TEnum>("unknown");

    private static readonly Dictionary<string, TEnum> ValuesByName = BuildValuesByName();

    public override TEnum Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        if (reader.TokenType != JsonTokenType.String)
        {
            throw new JsonException(
                $"Expected a string for {typeof(TEnum).Name}, but got {reader.TokenType}."
            );
        }

        var name = reader.GetString()!;

        return ValuesByName.GetValueOrDefault(name, Unknown);
    }

    public override void Write(Utf8JsonWriter writer, TEnum value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }

    private static Dictionary<string, TEnum> BuildValuesByName()
    {
        var valuesByName = new Dictionary<string, TEnum>(StringComparer.OrdinalIgnoreCase);
        foreach (var value in Enum.GetValues<TEnum>())
        {
            valuesByName[value.ToString()] = value;
        }

        return valuesByName;
    }
}
