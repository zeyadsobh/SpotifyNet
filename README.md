# SpotifyNet

A .NET 10 client for the [Spotify Web API](https://developer.spotify.com/documentation/web-api), covering every non-deprecated endpoint in [open-api-schema.yaml](open-api-schema.yaml).

## Setup

Set `Spotify:AuthorizationClient:AppClientId` and `AppRedirectUri` from your Spotify app and register the services with `services.AddSpotifyNet()` (`using SpotifyNet.DependencyInjection;`), as in [the Playground](SpotifyNet.Playground/Program.cs). Then authorize once with the scopes you need:

```csharp
await tokenAcquirer.EnsureTokenExists([AuthorizationScope.UserLibraryRead, AuthorizationScope.UserTopRead]);
```

This runs the PKCE authorization flow in the browser and stores the token in `%LOCALAPPDATA%\SpotifyNet` (or the directory given to `AuthorizationRepository`), refreshing it when it expires.

## Usage

`IWebAPIService` has a service per area: Albums, Artists, Audiobooks, Chapters, Episodes, Library, Player, Playlists, Search, Shows, Tracks and Users.

```csharp
var album = await webAPI.Albums.GetAlbum("4aawyAB9vmqN3uQ7FjRGTy");

await foreach (var saved in webAPI.Tracks.GetUserSavedTracks())
{
    Console.WriteLine(saved.Track?.Name);
}

var topArtists = await webAPI.Users.GetTopItems<Artist>(TimeRange.short_term).Take(10).ToListAsync();
```

- Paged endpoints return `IAsyncEnumerable<T>` and load pages as you enumerate.
- Each method checks that the token has its endpoint's scopes, and throws `AuthorizationException` before calling Spotify if not.
- Failed requests throw `WebAPIException` with the status code and Spotify's message. Rate-limited ones are retried after their `Retry-After` delay (up to 3 times, for delays of 60 s or less).
- Playback state and currently playing return `null` when nothing is playing.
