using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SpotifyNet.Clients.Abstractions;
using SpotifyNet.Datastructures.Spotify.Authorization;
using SpotifyNet.Datastructures.Spotify.Player;

namespace SpotifyNet.Services.Abstractions.WebAPI;

/// <summary>Playback state and controls for the current user's devices.</summary>
/// <remarks>
/// <para>
/// Controls target the user's active device unless given a <c>deviceId</c>. They only work for
/// Spotify Premium users, and the order they run in isn't guaranteed across player endpoints.
/// </para>
/// <para>
/// Bad arguments throw <see cref="ArgumentNullException"/>, <see cref="ArgumentException"/> or
/// <see cref="ArgumentOutOfRangeException"/> before any request is sent. Requests throw
/// <see cref="AuthorizationException"/> if there's no access token yet or it lacks a required
/// scope, and <see cref="WebAPIException"/> if Spotify answers with an error;
/// <see cref="GetRecentlyPlayedTracks"/> does so while it's enumerated.
/// </para>
/// </remarks>
public interface IPlayerService
{
    /// <summary>
    /// Gets the playback state: the item playing, its progress and the active device.
    /// </summary>
    /// <returns><see langword="null"/> if playback isn't available or active.</returns>
    /// <remarks>Requires <see cref="AuthorizationScope.UserReadPlaybackState"/>.</remarks>
    Task<CurrentlyPlayingContext?> GetPlaybackState(CancellationToken cancellationToken = default);

    /// <summary>Transfers playback to another device.</summary>
    /// <param name="deviceId">The device to transfer playback to.</param>
    /// <param name="play">Start playing on the new device; keep the current state if not <see langword="true"/>.</param>
    /// <param name="cancellationToken">A token to cancel the request.</param>
    /// <remarks>Requires <see cref="AuthorizationScope.UserModifyPlaybackState"/>.</remarks>
    Task TransferPlayback(
        string deviceId,
        bool? play = null,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Gets the user's available Spotify Connect devices. Some device models aren't supported and
    /// aren't listed.
    /// </summary>
    /// <remarks>Requires <see cref="AuthorizationScope.UserReadPlaybackState"/>.</remarks>
    Task<IReadOnlyList<Device>> GetAvailableDevices(CancellationToken cancellationToken = default);

    /// <summary>Gets what's currently playing on the user's Spotify account.</summary>
    /// <returns><see langword="null"/> if nothing is playing.</returns>
    /// <remarks>Requires <see cref="AuthorizationScope.UserReadCurrentlyPlaying"/>.</remarks>
    Task<CurrentlyPlayingContext?> GetCurrentlyPlaying(
        CancellationToken cancellationToken = default
    );

    /// <summary>Resumes the current playback.</summary>
    /// <param name="deviceId">The device to target; the active one if <see langword="null"/>.</param>
    /// <param name="cancellationToken">A token to cancel the request.</param>
    /// <remarks>Requires <see cref="AuthorizationScope.UserModifyPlaybackState"/>.</remarks>
    Task ResumePlayback(string? deviceId = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Plays an album, artist or playlist. Offsets are only available for albums and playlists.
    /// </summary>
    /// <param name="contextUri">The URI of the album, artist or playlist to play.</param>
    /// <param name="offsetPosition">The index to start at; or give <paramref name="offsetUri"/>.</param>
    /// <param name="offsetUri">The item to start at; or give <paramref name="offsetPosition"/>.</param>
    /// <param name="positionMs">Where to start in the item, in milliseconds.</param>
    /// <param name="deviceId">The device to target; the active one if <see langword="null"/>.</param>
    /// <param name="cancellationToken">A token to cancel the request.</param>
    /// <remarks>Requires <see cref="AuthorizationScope.UserModifyPlaybackState"/>.</remarks>
    Task PlayContext(
        string contextUri,
        int? offsetPosition = null,
        string? offsetUri = null,
        int? positionMs = null,
        string? deviceId = null,
        CancellationToken cancellationToken = default
    );

    /// <summary>Plays these track URIs.</summary>
    /// <param name="uris">The Spotify URIs of the tracks to play.</param>
    /// <param name="offsetPosition">The index to start at; or give <paramref name="offsetUri"/>.</param>
    /// <param name="offsetUri">The item to start at; or give <paramref name="offsetPosition"/>.</param>
    /// <param name="positionMs">Where to start in the item, in milliseconds.</param>
    /// <param name="deviceId">The device to target; the active one if <see langword="null"/>.</param>
    /// <param name="cancellationToken">A token to cancel the request.</param>
    /// <remarks>Requires <see cref="AuthorizationScope.UserModifyPlaybackState"/>.</remarks>
    Task PlayItems(
        IEnumerable<string> uris,
        int? offsetPosition = null,
        string? offsetUri = null,
        int? positionMs = null,
        string? deviceId = null,
        CancellationToken cancellationToken = default
    );

    /// <summary>Pauses the current playback.</summary>
    /// <remarks>Requires <see cref="AuthorizationScope.UserModifyPlaybackState"/>.</remarks>
    Task PausePlayback(string? deviceId = null, CancellationToken cancellationToken = default);

    /// <summary>Skips to the next item in the queue.</summary>
    /// <remarks>Requires <see cref="AuthorizationScope.UserModifyPlaybackState"/>.</remarks>
    Task SkipToNext(string? deviceId = null, CancellationToken cancellationToken = default);

    /// <summary>Skips to the previous item in the queue.</summary>
    /// <remarks>Requires <see cref="AuthorizationScope.UserModifyPlaybackState"/>.</remarks>
    Task SkipToPrevious(string? deviceId = null, CancellationToken cancellationToken = default);

    /// <summary>Seeks to a position in the item playing.</summary>
    /// <param name="positionMs">
    /// The position in milliseconds; past the item's end skips to the next one.
    /// </param>
    /// <param name="deviceId">The device to target; the active one if <see langword="null"/>.</param>
    /// <param name="cancellationToken">A token to cancel the request.</param>
    /// <remarks>Requires <see cref="AuthorizationScope.UserModifyPlaybackState"/>.</remarks>
    Task SeekToPosition(
        int positionMs,
        string? deviceId = null,
        CancellationToken cancellationToken = default
    );

    /// <summary>Sets the repeat mode of the current playback.</summary>
    /// <remarks>Requires <see cref="AuthorizationScope.UserModifyPlaybackState"/>.</remarks>
    Task SetRepeatMode(
        RepeatState state,
        string? deviceId = null,
        CancellationToken cancellationToken = default
    );

    /// <summary>Sets the volume of the current playback device.</summary>
    /// <param name="volumePercent">0 to 100.</param>
    /// <param name="deviceId">The device to target; the active one if <see langword="null"/>.</param>
    /// <param name="cancellationToken">A token to cancel the request.</param>
    /// <remarks>Requires <see cref="AuthorizationScope.UserModifyPlaybackState"/>.</remarks>
    Task SetPlaybackVolume(
        int volumePercent,
        string? deviceId = null,
        CancellationToken cancellationToken = default
    );

    /// <summary>Turns shuffle on or off for the current playback.</summary>
    /// <remarks>Requires <see cref="AuthorizationScope.UserModifyPlaybackState"/>.</remarks>
    Task SetPlaybackShuffle(
        bool state,
        string? deviceId = null,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Gets the tracks the current user played recently, a page at a time. Plays strictly after or
    /// before a moment (not both); the latest ones if neither.
    /// </summary>
    /// <remarks>
    /// Requires <see cref="AuthorizationScope.UserReadRecentlyPlayed"/>. Podcast episodes aren't
    /// supported.
    /// </remarks>
    IAsyncEnumerable<PlayHistory> GetRecentlyPlayedTracks(
        DateTimeOffset? after = null,
        DateTimeOffset? before = null,
        CancellationToken cancellationToken = default
    );

    /// <summary>Gets the playback queue: the item playing and those up next.</summary>
    /// <remarks>
    /// Requires <see cref="AuthorizationScope.UserReadCurrentlyPlaying"/> and
    /// <see cref="AuthorizationScope.UserReadPlaybackState"/>.
    /// </remarks>
    Task<PlaybackQueue> GetQueue(CancellationToken cancellationToken = default);

    /// <summary>Adds an item to the current user's playback queue.</summary>
    /// <param name="uri">A track or episode URI.</param>
    /// <param name="deviceId">The device to target; the active one if <see langword="null"/>.</param>
    /// <param name="cancellationToken">A token to cancel the request.</param>
    /// <remarks>Requires <see cref="AuthorizationScope.UserModifyPlaybackState"/>.</remarks>
    Task AddItemToQueue(
        string uri,
        string? deviceId = null,
        CancellationToken cancellationToken = default
    );
}
