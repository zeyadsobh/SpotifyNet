using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using SpotifyNet.Clients;
using SpotifyNet.Clients.Abstractions;
using SpotifyNet.Clients.Options;
using SpotifyNet.Datastructures.Spotify.Authorization;

namespace SpotifyNet.Tests.Clients;

public class AuthorizationClientTests
{
    [Test]
    public async Task BuildsTheAuthorizeUriWithoutCallingSpotify()
    {
        var handler = new StubHandler();

        var authorization = Client(handler)
            .GetUserAuthorization([
                AuthorizationScope.UserReadPrivate,
                AuthorizationScope.UserReadEmail,
            ]);
        var query = HttpUtility.ParseQueryString(new Uri(authorization.AuthorizationUri).Query);

        await Assert.That(query["scope"]).IsEqualTo("user-read-private user-read-email");
        await Assert.That(query["state"]).IsEqualTo(authorization.State);
        await Assert.That(handler.Bodies).IsEmpty();
    }

    [Test]
    public async Task ThrowsWithTheOAuthErrorDescription()
    {
        var handler = new StubHandler(
            new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent(
                    """{"error":"invalid_grant","error_description":"Refresh token revoked"}"""
                ),
            }
        );

        var exception = (
            await Assert.ThrowsAsync<WebAPIException>(() =>
                Client(handler).RefreshUserAccessToken("refresh")
            )
        )!;

        await Assert.That(exception.StatusCode).IsEqualTo(HttpStatusCode.BadRequest);
        await Assert.That(exception.ErrorMessage).IsEqualTo("Refresh token revoked");
    }

    [Test]
    public async Task KeepsTokensOutOfParseFailures()
    {
        var handler = new StubHandler(
            new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("""{"access_token":"secret","expires_in":"soon"}"""),
            }
        );

        var exception = (
            await Assert.ThrowsAsync<WebAPIException>(() =>
                Client(handler).RefreshUserAccessToken("refresh")
            )
        )!;

        await Assert.That(exception.ToString()).DoesNotContain("secret");
    }

    private static IAuthorizationClient Client(StubHandler handler) =>
        new AuthorizationClient(
            Microsoft.Extensions.Options.Options.Create(
                new AuthorizationClientOptions
                {
                    AppClientId = "id",
                    AppRedirectUri = "http://localhost:5000/callback",
                }
            ),
            new HttpClient(handler)
        );
}
