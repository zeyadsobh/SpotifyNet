using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using SpotifyNet.Datastructures.Spotify.Player;

namespace SpotifyNet.Datastructures.Serialization;

/// <summary>
/// Reads playback actions as the spec has them (the flags themselves) and as Spotify sends them
/// (nested under <c>disallows</c>), and writes them as Spotify sends them.
/// </summary>
internal sealed class ActionsConverter : JsonConverter<Disallows>
{
    public override Disallows? Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        using var document = JsonDocument.ParseValue(ref reader);
        var element = document.RootElement;

        if (
            element.ValueKind == JsonValueKind.Object
            && element.TryGetProperty("disallows", out var disallows)
        )
        {
            element = disallows;
        }

        return element.Deserialize<Disallows>(options);
    }

    public override void Write(
        Utf8JsonWriter writer,
        Disallows value,
        JsonSerializerOptions options
    )
    {
        writer.WriteStartObject();
        writer.WritePropertyName("disallows");
        JsonSerializer.Serialize(writer, value, options);
        writer.WriteEndObject();
    }
}
