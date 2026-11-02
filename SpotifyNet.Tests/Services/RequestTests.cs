using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using SpotifyNet.Datastructures.Spotify;
using SpotifyNet.Datastructures.Spotify.Albums;
using SpotifyNet.Datastructures.Spotify.Player;
using SpotifyNet.Datastructures.Spotify.Search;
using SpotifyNet.Datastructures.Spotify.Tracks;
using SpotifyNet.Datastructures.Spotify.Users;
using SpotifyNet.Services.Abstractions.WebAPI;
using SpotifyNet.Services.WebAPI;
using TUnit.Assertions.Enums;

namespace SpotifyNet.Tests.Services;

/// <summary>Request shapes that are easy to get wrong.</summary>
public class RequestTests
{
    // How WebAPIClient serializes bodies.
    private static readonly JsonSerializerOptions BodyOptions = new(JsonSerializerDefaults.Web)
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
    };

    [Test]
    public async Task ArtistAlbumsUseTheirSmallerPageSize()
    {
        const string uri =
            "https://api.spotify.com/v1/artists/a/albums?include_groups=album%2Cappears_on&limit=10&offset=0";
        var client = new FakeWebAPIClient { Responses = { [uri] = EmptyPage<SimplifiedAlbum>() } };

        await Service(client)
            .Artists.GetAlbums("a", [AlbumGroup.album, AlbumGroup.appears_on], 0)
            .ToListAsync();

        await Assert.That(client.Requests.Single().Uri).IsEqualTo(uri);
    }

    [Test]
    public async Task SearchEncodesTheQuery()
    {
        const string uri =
            "https://api.spotify.com/v1/search?q=AC%2FDC%20%26%20year%3A1980&type=track%2Calbum&include_external=audio";
        var client = new FakeWebAPIClient { Responses = { [uri] = new SearchResult() } };

        await Service(client)
            .Search.SearchItems(
                "AC/DC & year:1980",
                [SearchType.track, SearchType.album],
                null,
                null,
                true
            );

        await Assert.That(client.Requests.Single().Uri).IsEqualTo(uri);
    }

    [Test]
    public async Task ShuffleSendsLowercaseState()
    {
        var client = new FakeWebAPIClient();

        await Service(client).Player.SetPlaybackShuffle(true, "d");

        await Assert
            .That(client.Requests.Single().Uri)
            .IsEqualTo("https://api.spotify.com/v1/me/player/shuffle?state=true&device_id=d");
    }

    [Test]
    public async Task RecentlyPlayedSendsUnixMilliseconds()
    {
        const string uri =
            "https://api.spotify.com/v1/me/player/recently-played?limit=50&after=1484811043508";
        var client = new FakeWebAPIClient
        {
            Responses = { [uri] = new CursorPaginationWrapper<PlayHistory>() },
        };

        await Service(client)
            .Player.GetRecentlyPlayedTracks(
                DateTimeOffset.FromUnixTimeMilliseconds(1484811043508),
                null
            )
            .ToListAsync();

        await Assert.That(client.Requests.Single().Uri).IsEqualTo(uri);
    }

    [Test]
    [Arguments(2, null, """{"uris":["u"],"offset":{"position":2}}""")]
    [Arguments(null, "u", """{"uris":["u"],"offset":{"uri":"u"}}""")]
    [Arguments(null, null, """{"uris":["u"]}""")]
    public async Task PlaySendsTheOffsetAsPositionOrUri(int? position, string? uri, string body)
    {
        var client = new FakeWebAPIClient();

        await Service(client).Player.PlayItems(["u"], position, uri);

        await Assert
            .That(JsonSerializer.Serialize(client.Requests.Single().Body, BodyOptions))
            .IsEqualTo(body);
    }

    [Test]
    public async Task PlayRejectsBothOffsetForms() =>
        await Assert
            .That(() => Service(new()).Player.PlayItems(["u"], 2, "u"))
            .Throws<ArgumentException>();

    [Test]
    public async Task PlayEnumeratesItsUrisOnce()
    {
        var client = new FakeWebAPIClient();
        var enumerations = 0;

        IEnumerable<string> LazyUris()
        {
            enumerations++;
            yield return "u";
        }

        await Service(client).Player.PlayItems(LazyUris());
        // Serializing the body would enumerate the URIs again if they were still lazy.
        JsonSerializer.Serialize(client.Requests.Single().Body, BodyOptions);

        await Assert.That(enumerations).IsEqualTo(1);
    }

    [Test]
    [Arguments("")]
    [Arguments(" ")]
    [Arguments(".")]
    [Arguments("..")]
    public async Task RejectsIdsThatWouldChangeTheEndpoint(string id) =>
        await Assert
            .That(() => (Task)Service(new()).Albums.GetAlbum(id))
            .Throws<ArgumentException>();

    [Test]
    public async Task RejectsNegativeOffsetsAndLimits()
    {
        var service = Service(new());

        await Assert
            .That(() => service.Tracks.GetUserSavedTracks(-1))
            .ThrowsExactly<ArgumentOutOfRangeException>();
        await Assert
            .That(() => (Task)service.Search.SearchItems("q", [SearchType.track], offset: -1))
            .ThrowsExactly<ArgumentOutOfRangeException>();
        await Assert
            .That(() => (Task)service.Search.SearchItems("q", [SearchType.track], limit: -1))
            .ThrowsExactly<ArgumentOutOfRangeException>();
    }

    [Test]
    public async Task RejectsNegativePositions()
    {
        var service = Service(new());

        await Assert
            .That(() => service.Player.PlayItems(["u"], offsetPosition: -1))
            .ThrowsExactly<ArgumentOutOfRangeException>();
        await Assert
            .That(() => service.Player.PlayContext("c", positionMs: -1))
            .ThrowsExactly<ArgumentOutOfRangeException>();
        await Assert
            .That(() => (Task)service.Playlists.AddPlaylistItems("p", ["u"], -1))
            .ThrowsExactly<ArgumentOutOfRangeException>();
        await Assert
            .That(() => (Task)service.Playlists.ReorderPlaylistItems("p", -1, 0))
            .ThrowsExactly<ArgumentOutOfRangeException>();
    }

    [Test]
    public async Task RejectsNullUris()
    {
        var service = Service(new());
        string[] uris = ["spotify:track:a", null!];

        await Assert.That(() => service.Library.SaveItems(uris)).ThrowsExactly<ArgumentException>();
        await Assert
            .That(async () => await service.Library.CheckSavedItems(uris).AnyAsync())
            .ThrowsExactly<ArgumentException>();
        await Assert.That(() => service.Player.PlayItems(uris)).ThrowsExactly<ArgumentException>();
    }

    [Test]
    public async Task TopItemsPathFollowsTheItemType()
    {
        const string uri =
            "https://api.spotify.com/v1/me/top/tracks?time_range=short_term&limit=50&offset=0";
        var client = new FakeWebAPIClient { Responses = { [uri] = EmptyPage<Track>() } };

        await Service(client).Users.GetTopItems<Track>(TimeRange.short_term, 0).ToListAsync();

        await Assert.That(client.Requests.Single().Uri).IsEqualTo(uri);
    }

    [Test]
    public async Task ChecksSavedItemsInChunksOf40()
    {
        var first =
            "https://api.spotify.com/v1/me/library/contains?uris="
            + string.Join("%2C", Enumerable.Repeat("spotify%3Atrack%3Aa", 40));
        const string second =
            "https://api.spotify.com/v1/me/library/contains?uris=spotify%3Atrack%3Ab";
        var client = new FakeWebAPIClient
        {
            Responses =
            {
                [first] = Enumerable.Repeat(false, 40).ToArray(),
                [second] = new[] { true },
            },
        };

        string[] uris = [.. Enumerable.Repeat("spotify:track:a", 40), "spotify:track:b"];

        var saved = await Service(client).Library.CheckSavedItems(uris).ToListAsync();

        await Assert
            .That(saved)
            .IsEquivalentTo([.. Enumerable.Repeat(false, 40), true], CollectionOrdering.Matching);
        await Assert.That(client.Requests.Count).IsEqualTo(2);

        client.Requests.Clear();
        await Service(client).Library.CheckSavedItems(uris).Take(1).ToListAsync();

        await Assert.That(client.Requests.Count).IsEqualTo(1);
    }

    private static IWebAPIService Service(FakeWebAPIClient client) =>
        new WebAPIService(new StubAuthorizationService(), client);

    private static OffsetPaginationWrapper<T> EmptyPage<T>() =>
        new()
        {
            Href = "",
            Limit = 0,
            Next = null,
            Offset = 0,
            Previous = null,
            Total = 0,
            Items = [],
        };
}
