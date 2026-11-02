using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SpotifyNet.Datastructures.Spotify;
using SpotifyNet.Datastructures.Spotify.Albums;
using SpotifyNet.Datastructures.Spotify.Artists;
using SpotifyNet.Services.Abstractions.WebAPI;
using SpotifyNet.Services.WebAPI;

namespace SpotifyNet.Tests.Services;

public class PaginationTests
{
    private const string FirstPage = "https://api.spotify.com/v1/me/albums?limit=50&offset=0";
    private const string SecondPage = "https://api.spotify.com/v1/me/albums?offset=50&limit=50";

    [Test]
    public async Task FollowsNextLinks()
    {
        var client = new FakeWebAPIClient
        {
            Responses = { [FirstPage] = Page(2, SecondPage), [SecondPage] = Page(1, null) },
        };

        var albums = await Service(client).Albums.GetUserSavedAlbums(0).ToListAsync();

        await Assert.That(albums.Count).IsEqualTo(3);
        await Assert
            .That(client.Requests.Select(r => r.Uri))
            .IsEquivalentTo([FirstPage, SecondPage]);
    }

    [Test]
    public async Task FetchesPagesOnlyAsNeeded()
    {
        var client = new FakeWebAPIClient { Responses = { [FirstPage] = Page(2, SecondPage) } };

        await Service(client).Albums.GetUserSavedAlbums(0).Take(1).ToListAsync();

        await Assert.That(client.Requests.Count).IsEqualTo(1);
    }

    [Test]
    public async Task StopsMidPageOnceCancelled()
    {
        var client = new FakeWebAPIClient { Responses = { [FirstPage] = Page(2, null) } };
        using var cancellation = new CancellationTokenSource();

        await using var albums = Service(client)
            .Albums.GetUserSavedAlbums(0, cancellation.Token)
            .GetAsyncEnumerator();
        await albums.MoveNextAsync();
        await cancellation.CancelAsync();

        await Assert
            .That(async () => await albums.MoveNextAsync())
            .Throws<OperationCanceledException>();
    }

    [Test]
    public async Task FollowsNextLinksInsideWrappers()
    {
        const string first = "https://api.spotify.com/v1/me/following?type=artist&limit=50";
        const string second =
            "https://api.spotify.com/v1/me/following?type=artist&after=a&limit=50";
        var client = new FakeWebAPIClient
        {
            Responses = { [first] = Artists(second), [second] = Artists(null) },
        };

        var artists = await Service(client).Users.GetFollowedArtists(null).ToListAsync();

        await Assert.That(artists.Count).IsEqualTo(2);
    }

    private static FollowedArtists Artists(string? next) =>
        new()
        {
            Artists = new() { Items = [new Artist()], Next = next },
        };

    private static IWebAPIService Service(FakeWebAPIClient client) =>
        new WebAPIService(new StubAuthorizationService(), client);

    private static OffsetPaginationWrapper<SavedAlbum> Page(int count, string? next) =>
        new()
        {
            Href = FirstPage,
            Limit = 50,
            Next = next,
            Offset = 0,
            Previous = null,
            Total = 3,
            Items = [.. Enumerable.Range(0, count).Select(_ => new SavedAlbum())],
        };
}
