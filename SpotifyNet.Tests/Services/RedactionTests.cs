using System;
using System.Threading.Tasks;
using SpotifyNet.Services;
using SpotifyNet.Services.Abstractions;

namespace SpotifyNet.Tests.Services;

public class RedactionTests
{
    [Test]
    public async Task ToStringLeavesSecretsOut()
    {
        object[] credentials =
        [
            new AccessTokenMetadata
            {
                AuthorizationScopes = ["user-read-private"],
                Token = "secret",
                ExpiresAtUtc = DateTimeOffset.UnixEpoch,
                RefreshToken = "secret",
            },
            new UserAuthorizationMetadata
            {
                AuthorizationScopes = ["user-read-private"],
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
