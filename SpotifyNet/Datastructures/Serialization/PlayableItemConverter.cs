using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using SpotifyNet.Datastructures.Spotify;
using SpotifyNet.Datastructures.Spotify.Episodes;
using SpotifyNet.Datastructures.Spotify.Tracks;

namespace SpotifyNet.Datastructures.Serialization;

/// <summary>Dispatches on the <c>type</c> field, wherever it appears in the object.</summary>
internal sealed class PlayableItemConverter : JsonConverter<PlayableItem>
{
    public override PlayableItem? Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        using var document = JsonDocument.ParseValue(ref reader);
        var element = document.RootElement;

        if (element.ValueKind != JsonValueKind.Object)
        {
            throw new JsonException(
                $"Expected an object for {nameof(PlayableItem)}, but got {element.ValueKind}."
            );
        }

        var type =
            element.TryGetProperty("type", out var typeElement)
            && typeElement.ValueKind == JsonValueKind.String
                ? typeElement.GetString()
                : null;

        return type switch
        {
            "track" => element.Deserialize<Track>(options),
            "episode" => element.Deserialize<Episode>(options),
            _ => new UnknownPlayableItem { Type = type, Json = element.Clone() },
        };
    }

    public override void Write(
        Utf8JsonWriter writer,
        PlayableItem value,
        JsonSerializerOptions options
    )
    {
        switch (value)
        {
            case Track track:
                JsonSerializer.Serialize(writer, track, options);
                break;
            case Episode episode:
                JsonSerializer.Serialize(writer, episode, options);
                break;
            case UnknownPlayableItem unknown:
                unknown.Json.WriteTo(writer);
                break;
            default:
                throw new JsonException(
                    $"Cannot serialize {nameof(PlayableItem)} of type {value.GetType().Name}."
                );
        }
    }
}
