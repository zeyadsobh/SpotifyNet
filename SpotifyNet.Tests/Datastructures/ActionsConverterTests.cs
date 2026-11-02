using System.Text.Json;
using System.Threading.Tasks;
using SpotifyNet.Datastructures.Spotify.Player;

namespace SpotifyNet.Tests.Datastructures;

public class ActionsConverterTests
{
    [Test]
    [Arguments("""{"actions":{"disallows":{"resuming":true}}}""")] // As Spotify sends it.
    [Arguments("""{"actions":{"resuming":true}}""")] // As the spec has it.
    public async Task ReadsBothShapes(string json)
    {
        var context = JsonSerializer.Deserialize<CurrentlyPlayingContext>(json)!;

        await Assert.That(context.Actions?.Resuming).IsTrue();
    }
}
