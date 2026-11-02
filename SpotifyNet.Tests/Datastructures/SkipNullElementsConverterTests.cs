using System.Text.Json;
using System.Threading.Tasks;
using SpotifyNet.Datastructures.Spotify;

namespace SpotifyNet.Tests.Datastructures;

public class SkipNullElementsConverterTests
{
    [Test]
    public async Task SkipsNullItemsInPages()
    {
        var offsetPage = JsonSerializer.Deserialize<OffsetPaginationWrapper<ExternalUrls>>(
            """
            {"href":"h","limit":2,"next":null,"offset":0,"previous":null,"total":2,
             "items":[{"spotify":"a"},null]}
            """
        )!;
        var cursorPage = JsonSerializer.Deserialize<CursorPaginationWrapper<ExternalUrls>>(
            """{"items":[null,{"spotify":"b"}]}"""
        )!;

        await Assert.That(offsetPage.Items).IsEquivalentTo([new ExternalUrls { Spotify = "a" }]);
        await Assert.That(cursorPage.Items!).IsEquivalentTo([new ExternalUrls { Spotify = "b" }]);
    }
}
