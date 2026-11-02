using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using SpotifyNet.Datastructures.Spotify;
using SpotifyNet.Datastructures.Spotify.Episodes;
using SpotifyNet.Datastructures.Spotify.Tracks;

namespace SpotifyNet.Tests.Datastructures;

public class PlayableItemConverterTests
{
    private const string EpisodeJson = """
        {
          "type": "episode", "id": "e", "name": "n", "uri": "u", "href": "h",
          "description": "d", "html_description": "d", "duration_ms": 1, "explicit": false,
          "external_urls": {}, "images": [], "is_externally_hosted": false, "is_playable": true,
          "languages": [], "release_date": "2024", "release_date_precision": "year",
          "show": {
            "type": "show", "id": "s", "name": "n", "uri": "u", "href": "h",
            "description": "d", "html_description": "d", "explicit": false, "external_urls": {},
            "images": [], "is_externally_hosted": null, "languages": [], "media_type": "audio",
            "copyrights": [], "total_episodes": 1
          }
        }
        """;

    [Test]
    [Arguments("""{"type":"track","id":"1"}""")]
    [Arguments("""{"id":"1","type":"track"}""")]
    public async Task ReadsAndWritesTrackWhereverTypeAppears(string json)
    {
        var item = JsonSerializer.Deserialize<PlayableItem>(json);
        var written = JsonNode.Parse(JsonSerializer.Serialize(item))!;

        await Assert.That(item).IsTypeOf<Track>();
        await Assert.That((string?)written["id"]).IsEqualTo("1");
    }

    [Test]
    public async Task ReadsEpisode() =>
        await Assert
            .That(JsonSerializer.Deserialize<PlayableItem>(EpisodeJson))
            .IsTypeOf<Episode>();

    [Test]
    [Arguments("""{"type":"ad","extra":{"a":[1]}}""", "ad")]
    [Arguments("""{"id":"9"}""", null)]
    public async Task KeepsUnrecognizedItemsAsRawJson(string json, string? type)
    {
        var item = JsonSerializer.Deserialize<PlayableItem>(json);

        await Assert.That(item).IsTypeOf<UnknownPlayableItem>();
        await Assert.That(((UnknownPlayableItem)item!).Type).IsEqualTo(type);
        await Assert.That(JsonSerializer.Serialize(item)).IsEqualTo(json);
    }

    [Test]
    public async Task ThrowsOnNonObject() =>
        await Assert
            .That(() => JsonSerializer.Deserialize<PlayableItem>("\"track\""))
            .Throws<JsonException>();
}
