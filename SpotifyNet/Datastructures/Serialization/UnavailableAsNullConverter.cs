using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SpotifyNet.Datastructures.Serialization;

/// <summary>Reads Spotify's placeholders for items that are no longer available (a null id) as null.</summary>
internal sealed class UnavailableAsNullConverter<T> : JsonConverter<T>
    where T : class
{
    public override T? Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        using var document = JsonDocument.ParseValue(ref reader);
        var element = document.RootElement;

        return
            element.ValueKind == JsonValueKind.Object
            && element.TryGetProperty("id", out var id)
            && id.ValueKind == JsonValueKind.Null
            ? null
            : element.Deserialize<T>(options);
    }

    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options) =>
        JsonSerializer.Serialize(writer, value, options);
}
