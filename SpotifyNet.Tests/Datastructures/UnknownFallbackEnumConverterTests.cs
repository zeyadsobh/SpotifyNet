using System.Text.Json;
using System.Threading.Tasks;
using SpotifyNet.Datastructures.Spotify;
using SpotifyNet.Datastructures.Spotify.Albums;

namespace SpotifyNet.Tests.Datastructures;

public class UnknownFallbackEnumConverterTests
{
    [Test]
    [Arguments("\"album\"", ItemType.album)]
    [Arguments("\"ALBUM\"", ItemType.album)]
    [Arguments("\"collection\"", ItemType.unknown)]
    [Arguments("null", null)]
    public async Task ReadsItemType(string json, ItemType? expected) =>
        await Assert.That(JsonSerializer.Deserialize<ItemType?>(json)).IsEqualTo(expected);

    [Test]
    public async Task ReadsUndefinedRestrictionReasonAsUnknown() =>
        await Assert
            .That(JsonSerializer.Deserialize<RestrictionReason>("\"new\""))
            .IsEqualTo(RestrictionReason.unknown);

    [Test]
    public async Task ThrowsOnNonString() =>
        await Assert.That(() => JsonSerializer.Deserialize<ItemType>("1")).Throws<JsonException>();

    [Test]
    public async Task ReadsEpAlbums() =>
        await Assert.That(JsonSerializer.Deserialize<AlbumType>("\"EP\"")).IsEqualTo(AlbumType.ep);

    [Test]
    [Arguments("\"mixtape\"")]
    [Arguments("1")]
    public async Task KeepsClosedEnumsStrict(string json) =>
        await Assert
            .That(() => JsonSerializer.Deserialize<AlbumType>(json))
            .Throws<JsonException>();
}
