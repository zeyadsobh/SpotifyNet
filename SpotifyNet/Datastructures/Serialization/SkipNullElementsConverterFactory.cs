using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SpotifyNet.Datastructures.Serialization;

/// <summary>
/// Drops null elements from arrays. Spotify sends them in some pages (e.g. playlists), though the
/// spec types items as non-null.
/// </summary>
internal sealed class SkipNullElementsConverterFactory : JsonConverterFactory
{
    public override bool CanConvert(Type typeToConvert) => typeToConvert.IsArray;

    public override JsonConverter CreateConverter(
        Type typeToConvert,
        JsonSerializerOptions options
    ) =>
        (JsonConverter)
            Activator.CreateInstance(
                typeof(SkipNullElementsConverter<>).MakeGenericType(typeToConvert.GetElementType()!)
            )!;

    private sealed class SkipNullElementsConverter<T> : JsonConverter<T[]>
    {
        public override T[]? Read(
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options
        ) =>
            JsonSerializer.Deserialize<List<T?>>(ref reader, options) is { } items
                ? [.. items.OfType<T>()]
                : null;

        public override void Write(
            Utf8JsonWriter writer,
            T[] value,
            JsonSerializerOptions options
        ) => JsonSerializer.Serialize(writer, value, options);
    }
}
