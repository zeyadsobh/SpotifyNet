using System.Text.Json;
using System.Threading.Tasks;
using SpotifyNet.Datastructures.Spotify.Episodes;

namespace SpotifyNet.Tests.Datastructures;

public class UnavailableAsNullConverterTests
{
    [Test]
    public async Task ReadsUnavailableSavedEpisodesAsNull()
    {
        var saved = JsonSerializer.Deserialize<SavedEpisode>(
            """
            {"added_at":"2024-01-01T00:00:00Z",
             "episode":{"id":null,"duration_ms":null,"is_externally_hosted":null}}
            """
        )!;

        await Assert.That(saved.Episode).IsNull();
    }
}
