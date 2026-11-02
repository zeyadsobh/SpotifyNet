using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using SpotifyNet.Datastructures.Spotify.Albums;
using SpotifyNet.Datastructures.Spotify.Player;
using SpotifyNet.Datastructures.Spotify.Search;
using SpotifyNet.Datastructures.Spotify.Users;
using static SpotifyNet.Services.WebAPI.WebAPIClientExtensions;

namespace SpotifyNet.Services.WebAPI;

internal static class Endpoints
{
    private const string BaseUrl = "https://api.spotify.com/v1";

    // The largest page most paged endpoints allow.
    private const int MaxLimit = 50;

    // Tells Spotify the client handles episodes as well as tracks (see PlayableItem).
    private const string AdditionalTypes = "track,episode";

    // Albums
    public static string GetAlbum(string id) => Url($"albums/{id}");

    public static string GetAlbumTracks(string id, int offset) =>
        Url($"albums/{id}/tracks", ("limit", MaxLimit), ("offset", Offset(offset)));

    public static string GetUserSavedAlbums(int offset) =>
        Url($"me/albums", ("limit", MaxLimit), ("offset", Offset(offset)));

    // Artists
    public static string GetArtist(string id) => Url($"artists/{id}");

    public static string GetArtistAlbums(
        string id,
        IEnumerable<AlbumGroup>? includeGroups,
        int offset
    ) =>
        // This endpoint allows at most 10 per page.
        Url(
            $"artists/{id}/albums",
            ("include_groups", includeGroups),
            ("limit", 10),
            ("offset", Offset(offset))
        );

    // Audiobooks
    public static string GetAudiobook(string id) => Url($"audiobooks/{id}");

    public static string GetAudiobookChapters(string id, int offset) =>
        Url($"audiobooks/{id}/chapters", ("limit", MaxLimit), ("offset", Offset(offset)));

    public static string GetUserSavedAudiobooks(int offset) =>
        Url($"me/audiobooks", ("limit", MaxLimit), ("offset", Offset(offset)));

    // Chapters
    public static string GetChapter(string id) => Url($"chapters/{id}");

    // Episodes
    public static string GetEpisode(string id) => Url($"episodes/{id}");

    public static string GetUserSavedEpisodes(int offset) =>
        Url($"me/episodes", ("limit", MaxLimit), ("offset", Offset(offset)));

    // Library
    public static string SaveItemsToLibrary(IEnumerable<string> uris) =>
        Url($"me/library", ("uris", uris));

    public static string RemoveItemsFromLibrary(IEnumerable<string> uris) =>
        Url($"me/library", ("uris", uris));

    public static string CheckUserSavedItems(IEnumerable<string> uris) =>
        Url($"me/library/contains", ("uris", uris));

    // Player
    public static string GetPlaybackState() =>
        Url($"me/player", ("additional_types", AdditionalTypes));

    public static string TransferPlayback() => Url($"me/player");

    public static string GetAvailableDevices() => Url($"me/player/devices");

    public static string GetCurrentlyPlayingTrack() =>
        Url($"me/player/currently-playing", ("additional_types", AdditionalTypes));

    public static string StartResumePlayback(string? deviceId) =>
        Url($"me/player/play", ("device_id", deviceId));

    public static string PausePlayback(string? deviceId) =>
        Url($"me/player/pause", ("device_id", deviceId));

    public static string SkipToNext(string? deviceId) =>
        Url($"me/player/next", ("device_id", deviceId));

    public static string SkipToPrevious(string? deviceId) =>
        Url($"me/player/previous", ("device_id", deviceId));

    public static string SeekToPosition(int positionMs, string? deviceId) =>
        Url($"me/player/seek", ("position_ms", positionMs), ("device_id", deviceId));

    public static string SetRepeatMode(RepeatState state, string? deviceId) =>
        Url($"me/player/repeat", ("state", state), ("device_id", deviceId));

    public static string SetPlaybackVolume(int volumePercent, string? deviceId) =>
        Url($"me/player/volume", ("volume_percent", volumePercent), ("device_id", deviceId));

    public static string TogglePlaybackShuffle(bool state, string? deviceId) =>
        Url($"me/player/shuffle", ("state", state), ("device_id", deviceId));

    public static string GetRecentlyPlayedTracks(DateTimeOffset? after, DateTimeOffset? before) =>
        Url(
            $"me/player/recently-played",
            ("limit", MaxLimit),
            ("after", after?.ToUnixTimeMilliseconds()),
            ("before", before?.ToUnixTimeMilliseconds())
        );

    public static string GetUserQueue() => Url($"me/player/queue");

    public static string AddItemToPlaybackQueue(string uri, string? deviceId) =>
        Url($"me/player/queue", ("uri", uri), ("device_id", deviceId));

    // Playlists
    public static string GetPlaylist(string id) =>
        Url($"playlists/{id}", ("additional_types", AdditionalTypes));

    public static string ChangePlaylistDetails(string id) => Url($"playlists/{id}");

    public static string GetPlaylistItems(string id, int offset) =>
        Url(
            $"playlists/{id}/items",
            ("limit", MaxLimit),
            ("offset", Offset(offset)),
            ("additional_types", AdditionalTypes)
        );

    public static string UpdatePlaylistItems(string id) => Url($"playlists/{id}/items");

    public static string AddItemsToPlaylist(string id) => Url($"playlists/{id}/items");

    public static string RemovePlaylistItems(string id) => Url($"playlists/{id}/items");

    public static string GetCurrentUserPlaylists(int offset) =>
        Url($"me/playlists", ("limit", MaxLimit), ("offset", Offset(offset)));

    public static string CreatePlaylist() => Url($"me/playlists");

    public static string GetPlaylistCoverImage(string id) => Url($"playlists/{id}/images");

    public static string AddCustomPlaylistCoverImage(string id) => Url($"playlists/{id}/images");

    // Search
    public static string Search(
        string query,
        IEnumerable<SearchType> types,
        int? limit,
        int? offset,
        bool includeExternalAudio
    ) =>
        Url(
            $"search",
            ("q", query),
            ("type", types),
            ("limit", limit),
            ("offset", Offset(offset)),
            ("include_external", includeExternalAudio ? "audio" : null)
        );

    // Shows
    public static string GetShow(string id) => Url($"shows/{id}");

    public static string GetShowEpisodes(string id, int offset) =>
        Url($"shows/{id}/episodes", ("limit", MaxLimit), ("offset", Offset(offset)));

    public static string GetUserSavedShows(int offset) =>
        Url($"me/shows", ("limit", MaxLimit), ("offset", Offset(offset)));

    // Tracks
    public static string GetTrack(string id) => Url($"tracks/{id}");

    public static string GetUserSavedTracks(int offset) =>
        Url($"me/tracks", ("limit", MaxLimit), ("offset", Offset(offset)));

    // Users
    public static string GetCurrentUserProfile() => Url($"me");

    public static string GetCurrentUserTopItems(string type, TimeRange? timeRange, int offset) =>
        Url(
            $"me/top/{type}",
            ("time_range", timeRange),
            ("limit", MaxLimit),
            ("offset", Offset(offset))
        );

    public static string GetFollowedArtists(string? after) =>
        Url($"me/following", ("type", "artist"), ("after", after), ("limit", MaxLimit));

    /// <summary>
    /// Escapes the path's interpolated IDs and the query values; null query values are left out.
    /// </summary>
    private static string Url(
        FormattableString path,
        params ReadOnlySpan<(string Name, object? Value)> query
    )
    {
        var arguments = path.GetArguments().Select(PathValue).ToArray();
        var url = new StringBuilder(BaseUrl)
            .Append('/')
            .AppendFormat(CultureInfo.InvariantCulture, path.Format, arguments);

        var separator = '?';
        foreach (var (name, value) in query)
        {
            if (value is not null)
            {
                url.Append(separator).Append(name).Append('=').Append(Escape(value));
                separator = '&';
            }
        }

        return url.ToString();
    }

    // A blank or dot-segment ID would silently address another endpoint, e.g. albums/ for
    // albums/{id}, as escaping keeps dots and Uri collapses dot segments.
    private static object PathValue(object? value)
    {
        var id = value as string;
        ArgumentException.ThrowIfNullOrWhiteSpace(id);
        if (id is "." or "..")
        {
            throw new ArgumentException($"`{id}` isn't an ID.", nameof(id));
        }

        return Escape(id);
    }

    // An offset indexes the results, so a negative one is never valid.
    private static int? Offset(int? offset)
    {
        ThrowIfNegative(offset);

        return offset;
    }

    private static string Escape(object value) => Uri.EscapeDataString(Format(value));

    private static string Format(object value) =>
        value switch
        {
            string text => text,
            bool flag => flag ? "true" : "false",
            IFormattable formattable => formattable.ToString(null, CultureInfo.InvariantCulture),
            IEnumerable values => string.Join(',', values.Cast<object>().Select(Format)),
            _ => value.ToString() ?? string.Empty,
        };
}
