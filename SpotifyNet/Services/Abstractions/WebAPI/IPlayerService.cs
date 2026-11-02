using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SpotifyNet.Datastructures.Spotify.Player;

namespace SpotifyNet.Services.Abstractions.WebAPI;

public interface IPlayerService
{
    /// <returns><see langword="null"/> if playback isn't available or active.</returns>
    Task<CurrentlyPlayingContext?> GetPlaybackState(CancellationToken cancellationToken = default);

    /// <param name="play">Start playing on the new device; keep the current state if not <see langword="true"/>.</param>
    Task TransferPlayback(
        string deviceId,
        bool? play = null,
        CancellationToken cancellationToken = default
    );

    Task<IReadOnlyList<Device>> GetAvailableDevices(CancellationToken cancellationToken = default);

    /// <returns><see langword="null"/> if nothing is playing.</returns>
    Task<CurrentlyPlayingContext?> GetCurrentlyPlaying(
        CancellationToken cancellationToken = default
    );

    /// <param name="deviceId">The device to target; the active one if <see langword="null"/>.</param>
    Task ResumePlayback(string? deviceId = null, CancellationToken cancellationToken = default);

    /// <summary>Plays an album, artist or playlist.</summary>
    /// <param name="offsetPosition">The index to start at; or give <paramref name="offsetUri"/>.</param>
    /// <param name="offsetUri">The item to start at; or give <paramref name="offsetPosition"/>.</param>
    Task PlayContext(
        string contextUri,
        int? offsetPosition = null,
        string? offsetUri = null,
        int? positionMs = null,
        string? deviceId = null,
        CancellationToken cancellationToken = default
    );

    /// <summary>Plays these track URIs.</summary>
    /// <param name="offsetPosition">The index to start at; or give <paramref name="offsetUri"/>.</param>
    /// <param name="offsetUri">The item to start at; or give <paramref name="offsetPosition"/>.</param>
    Task PlayItems(
        IEnumerable<string> uris,
        int? offsetPosition = null,
        string? offsetUri = null,
        int? positionMs = null,
        string? deviceId = null,
        CancellationToken cancellationToken = default
    );

    Task PausePlayback(string? deviceId = null, CancellationToken cancellationToken = default);

    Task SkipToNext(string? deviceId = null, CancellationToken cancellationToken = default);

    Task SkipToPrevious(string? deviceId = null, CancellationToken cancellationToken = default);

    /// <param name="positionMs">Past the item's end skips to the next one.</param>
    Task SeekToPosition(
        int positionMs,
        string? deviceId = null,
        CancellationToken cancellationToken = default
    );

    Task SetRepeatMode(
        RepeatState state,
        string? deviceId = null,
        CancellationToken cancellationToken = default
    );

    /// <param name="volumePercent">0 to 100.</param>
    Task SetPlaybackVolume(
        int volumePercent,
        string? deviceId = null,
        CancellationToken cancellationToken = default
    );

    Task SetPlaybackShuffle(
        bool state,
        string? deviceId = null,
        CancellationToken cancellationToken = default
    );

    /// <summary>Plays strictly after or before a moment (not both); the latest ones if neither.</summary>
    IAsyncEnumerable<PlayHistory> GetRecentlyPlayedTracks(
        DateTimeOffset? after = null,
        DateTimeOffset? before = null,
        CancellationToken cancellationToken = default
    );

    Task<PlaybackQueue> GetQueue(CancellationToken cancellationToken = default);

    /// <param name="uri">A track or episode URI.</param>
    Task AddItemToQueue(
        string uri,
        string? deviceId = null,
        CancellationToken cancellationToken = default
    );
}
