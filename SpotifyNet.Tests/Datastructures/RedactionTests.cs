using System.Threading.Tasks;
using SpotifyNet.Datastructures.Spotify.Authorization;

namespace SpotifyNet.Tests.Datastructures;

public class RedactionTests
{
    [Test]
    public async Task ToStringLeavesSecretsOut()
    {
        object[] credentials =
        [
            new AccessToken
            {
                Token = "secret",
                TokenType = "Bearer",
                ExpiresIn = 3600,
                RefreshToken = "secret",
            },
            new UserAuthorization
            {
                AuthorizationUri = "https://accounts.spotify.com/authorize",
                CodeVerifier = "secret",
                State = "state",
            },
        ];

        foreach (var credential in credentials)
        {
            await Assert.That(credential.ToString()).DoesNotContain("secret");
        }
    }
}
