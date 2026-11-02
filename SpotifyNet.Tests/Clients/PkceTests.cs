using System.Linq;
using System.Threading.Tasks;
using SpotifyNet.Clients;

namespace SpotifyNet.Tests.Clients;

public class PkceTests
{
    [Test]
    [Arguments(43)]
    [Arguments(128)]
    public async Task VerifierHasTheRequestedLengthAndAllowedCharacters(int length)
    {
        var (verifier, challenge) = PKCE.GetCodeVerifierAndChallenge(length);

        await Assert.That(verifier.Length).IsEqualTo(length);
        await Assert
            .That(verifier.All(c => char.IsAsciiLetterOrDigit(c) || "-._~".Contains(c)))
            .IsTrue();
        await Assert.That(challenge.Length).IsEqualTo(43);
    }
}
