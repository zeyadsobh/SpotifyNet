using System;
using System.IO;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SpotifyNet.Datastructures.Spotify.Authorization;
using SpotifyNet.Datastructures.Spotify.Playlists;
using SpotifyNet.Datastructures.Spotify.Tracks;
using SpotifyNet.DependencyInjection;
using SpotifyNet.Services.Abstractions;
using SpotifyNet.Services.Abstractions.WebAPI;

namespace SpotifyNet.Playground;

internal sealed class Program
{
    private static readonly JsonSerializerOptions indentedJsonOptions = new(
        JsonSerializerOptions.Default
    )
    {
        WriteIndented = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
    };

    private static readonly JsonSerializerOptions compactJsonOptions = new(
        JsonSerializerOptions.Default
    )
    {
        WriteIndented = false,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
    };

    public static Task Main(string[] args)
    {
        var builder = Host.CreateDefaultBuilder(args);

        builder.ConfigureServices(services => services.AddSpotifyNet());

        var host = builder.Build();

        var tokenAcquirerService = host.Services.GetRequiredService<ITokenAcquirerService>();
        var webAPIService = host.Services.GetRequiredService<IWebAPIService>();

        return Foo(tokenAcquirerService, webAPIService);
    }

    private static async Task Foo(
        ITokenAcquirerService tokenAcquirerService,
        IWebAPIService webAPIService
    )
    {
        var scopes = new[]
        {
            AuthorizationScope.UserTopRead,
            AuthorizationScope.UserLibraryRead,
            AuthorizationScope.PlaylistReadPrivate,
            AuthorizationScope.UserModifyPlaybackState,
            AuthorizationScope.UserReadCurrentlyPlaying,
            AuthorizationScope.PlaylistModifyPrivate,
            AuthorizationScope.PlaylistModifyPublic,
            AuthorizationScope.UgcImageUpload,
        };

        await tokenAcquirerService.EnsureTokenExists(scopes);

        await webAPIService.Player.SetPlaybackVolume(55);
    }

    public static string SpotifyLinkToId(string spotifyLink) =>
        new Uri(spotifyLink).Segments[2].Trim('/');

    private static string FullTrackName(
        PlaylistTrack playlistTrack,
        bool includeAlbumName = false
    ) => FullTrackName((Track)playlistTrack.Item!, includeAlbumName);

    private static string FullTrackName(SavedTrack savedTrack, bool includeAlbumName = false) =>
        FullTrackName(savedTrack.Track!, includeAlbumName);

    private static string FullTrackName(Track track, bool includeAlbumName = false)
    {
        var trackName = track.Name;

        var artists = track.Artists?.Select(a => a.Name) ?? Array.Empty<string>();
        var artistsNames = string.Join(", ", artists);

        if (includeAlbumName)
        {
            var albumName = track.Album?.Name;
            return $"{trackName} - {artistsNames} ({albumName})";
        }

        return $"{trackName} - {artistsNames}";
    }

    private static async Task<T> Read<T>(string path)
    {
        await using var fs = File.OpenRead(path);
        var item = await JsonSerializer.DeserializeAsync<T>(fs);
        return item!;
    }

    private static async Task Write<T>(string path, T item, bool indented = false)
    {
        if (File.Exists(path))
        {
            File.Delete(path);
        }
        else
        {
            var directory = Path.GetDirectoryName(path);
            if (!string.IsNullOrEmpty(directory))
            {
                Directory.CreateDirectory(directory);
            }
        }

        await using var fs = File.OpenWrite(path);

        var options = indented ? indentedJsonOptions : compactJsonOptions;
        await JsonSerializer.SerializeAsync(fs, item, options);
    }
}
