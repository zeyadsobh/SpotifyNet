using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using SpotifyNet.Clients;
using SpotifyNet.Clients.Abstractions;

namespace SpotifyNet.Tests.Clients;

public class WebAPIClientTests
{
    private const string Uri = "https://api.spotify.com/v1/me/player";

    private static readonly AccessTokenProvider Token = _ => Task.FromResult("token");

    [Test]
    public async Task GetsATokenForEachAttempt()
    {
        var handler = new StubHandler(RateLimited(0), Json(HttpStatusCode.OK, "{}"));
        var tokens = 0;

        await Client(handler).GetAsync<object>(Uri, _ => Task.FromResult($"token{++tokens}"));

        await Assert.That(handler.Tokens).IsEquivalentTo(new string?[] { "token1", "token2" });
    }

    [Test]
    public async Task GetOrDefaultReturnsNullOnNoContent()
    {
        var client = Client(new StubHandler(new HttpResponseMessage(HttpStatusCode.NoContent)));

        await Assert.That(await client.GetOrDefaultAsync<object>(Uri, Token)).IsNull();
    }

    [Test]
    public async Task RejectsANullBody() =>
        await Assert
            .That(() =>
                (Task)
                    Client(new StubHandler(Json(HttpStatusCode.OK, "null")))
                        .GetAsync<object>(Uri, Token)
            )
            .Throws<WebAPIException>();

    [Test]
    public async Task SendsJsonBodyWithoutNulls()
    {
        var handler = new StubHandler(Json(HttpStatusCode.Created, """{"snapshot_id":"s"}"""));

        var response = await Client(handler)
            .PostAsync<object, Dictionary<string, string>>(
                Uri,
                new { uris = new[] { "spotify:track:1" }, position = (int?)null },
                Token
            );

        await Assert
            .That(handler.Bodies)
            .IsEquivalentTo(new string?[] { """{"uris":["spotify:track:1"]}""" });
        await Assert.That(response["snapshot_id"]).IsEqualTo("s");
    }

    [Test]
    [Arguments("""{"error":{"status":404,"message":"Not found."}}""", "Not found.")]
    [Arguments("""{"status":400,"message":"Bad uris."}""", "Bad uris.")]
    [Arguments("<html></html>", null)]
    public async Task ThrowsWithSpotifyErrorMessage(string body, string? message)
    {
        var client = Client(new StubHandler(Json(HttpStatusCode.BadRequest, body)));

        var exception = (
            await Assert.ThrowsAsync<WebAPIException>(() => client.GetAsync<object>(Uri, Token))
        )!;

        await Assert.That(exception.StatusCode).IsEqualTo(HttpStatusCode.BadRequest);
        await Assert.That(exception.ErrorMessage).IsEqualTo(message);
        await Assert.That(exception.Content).IsEqualTo(body);
    }

    [Test]
    public async Task RetriesRateLimitedRequestWithItsBody()
    {
        var handler = new StubHandler(
            RateLimited(0),
            Json(HttpStatusCode.Created, """{"snapshot_id":"s"}""")
        );
        var uris = new List<string> { "a" };

        // The caller changing the body while the request waits mustn't change what's sent.
        var response = await Client(handler)
            .PostAsync<object, Dictionary<string, string>>(
                Uri,
                new { uris },
                _ =>
                {
                    uris.Add("b");
                    return Task.FromResult("token");
                }
            );

        await Assert
            .That(handler.Bodies)
            .IsEquivalentTo(new string?[] { """{"uris":["a"]}""", """{"uris":["a"]}""" });
        await Assert.That(response["snapshot_id"]).IsEqualTo("s");
    }

    [Test]
    [Arguments(0, 4)]
    [Arguments(61, 1)]
    [Arguments(null, 1)]
    public async Task GivesUpOnRateLimit(int? retryAfterSeconds, int requests)
    {
        var handler = new StubHandler([
            .. Enumerable.Range(0, 4).Select(_ => RateLimited(retryAfterSeconds)),
        ]);

        var exception = (
            await Assert.ThrowsAsync<WebAPIException>(() => Client(handler).PutAsync(Uri, Token))
        )!;

        await Assert.That(exception.StatusCode).IsEqualTo(HttpStatusCode.TooManyRequests);
        await Assert
            .That(exception.RetryAfter)
            .IsEqualTo(retryAfterSeconds is { } s ? TimeSpan.FromSeconds(s) : null);
        await Assert.That(handler.Bodies.Count).IsEqualTo(requests);
    }

    private static HttpResponseMessage RateLimited(int? retryAfterSeconds) =>
        new(HttpStatusCode.TooManyRequests)
        {
            Headers =
            {
                RetryAfter = retryAfterSeconds is { } s ? new(TimeSpan.FromSeconds(s)) : null,
            },
        };

    private static IWebAPIClient Client(StubHandler handler) => new WebAPIClient(new(handler));

    private static HttpResponseMessage Json(HttpStatusCode status, string json) =>
        new(status) { Content = new StringContent(json, null, "application/json") };
}
