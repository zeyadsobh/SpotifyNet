using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SpotifyNet.Clients.Abstractions;
using SpotifyNet.Datastructures.Spotify.Authorization;
using SpotifyNet.Datastructures.Spotify.Player;
using SpotifyNet.Services.Abstractions;
using SpotifyNet.Services.Abstractions.WebAPI;
using static SpotifyNet.Services.WebAPI.WebAPIClientExtensions;

namespace SpotifyNet.Services.WebAPI;

internal sealed class PlayerService(
    IAuthorizationService authorizationService,
    IWebAPIClient webAPIClient
) : IPlayerService
{
    public Task<CurrentlyPlayingContext?> GetPlaybackState(CancellationToken cancellationToken) =>
        webAPIClient.GetOrDefaultAsync<CurrentlyPlayingContext>(
            Endpoints.GetPlaybackState(),
            authorizationService.GetAccessToken(AuthorizationScope.UserReadPlaybackState),
            cancellationToken
        );

    public Task TransferPlayback(string deviceId, bool? play, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(deviceId);

        // The spec takes an array, but only supports a single device.
        return webAPIClient.PutAsync(
            Endpoints.TransferPlayback(),
            new { device_ids = new[] { deviceId }, play },
            ModifyAccessToken,
            cancellationToken
        );
    }

    public async Task<IReadOnlyList<Device>> GetAvailableDevices(
        CancellationToken cancellationToken
    )
    {
        var response = await webAPIClient.GetAsync<AvailableDevices>(
            Endpoints.GetAvailableDevices(),
            authorizationService.GetAccessToken(AuthorizationScope.UserReadPlaybackState),
            cancellationToken
        );

        return response.Devices;
    }

    // Spotify answers 204 when nothing is playing, though the spec only documents 200.
    public Task<CurrentlyPlayingContext?> GetCurrentlyPlaying(
        CancellationToken cancellationToken
    ) =>
        webAPIClient.GetOrDefaultAsync<CurrentlyPlayingContext>(
            Endpoints.GetCurrentlyPlayingTrack(),
            authorizationService.GetAccessToken(AuthorizationScope.UserReadCurrentlyPlaying),
            cancellationToken
        );

    public Task ResumePlayback(string? deviceId, CancellationToken cancellationToken) =>
        webAPIClient.PutAsync(
            Endpoints.StartResumePlayback(deviceId),
            ModifyAccessToken,
            cancellationToken
        );

    public Task PlayContext(
        string contextUri,
        int? offsetPosition,
        string? offsetUri,
        int? positionMs,
        string? deviceId,
        CancellationToken cancellationToken
    ) =>
        StartPlayback(
            contextUri,
            null,
            offsetPosition,
            offsetUri,
            positionMs,
            deviceId,
            cancellationToken
        );

    public Task PlayItems(
        IEnumerable<string> uris,
        int? offsetPosition,
        string? offsetUri,
        int? positionMs,
        string? deviceId,
        CancellationToken cancellationToken
    ) =>
        StartPlayback(
            null,
            uris,
            offsetPosition,
            offsetUri,
            positionMs,
            deviceId,
            cancellationToken
        );

    public Task PausePlayback(string? deviceId, CancellationToken cancellationToken) =>
        webAPIClient.PutAsync(
            Endpoints.PausePlayback(deviceId),
            ModifyAccessToken,
            cancellationToken
        );

    public Task SkipToNext(string? deviceId, CancellationToken cancellationToken) =>
        webAPIClient.PostAsync(
            Endpoints.SkipToNext(deviceId),
            ModifyAccessToken,
            cancellationToken
        );

    public Task SkipToPrevious(string? deviceId, CancellationToken cancellationToken) =>
        webAPIClient.PostAsync(
            Endpoints.SkipToPrevious(deviceId),
            ModifyAccessToken,
            cancellationToken
        );

    public Task SeekToPosition(
        int positionMs,
        string? deviceId,
        CancellationToken cancellationToken
    )
    {
        ArgumentOutOfRangeException.ThrowIfNegative(positionMs);

        return webAPIClient.PutAsync(
            Endpoints.SeekToPosition(positionMs, deviceId),
            ModifyAccessToken,
            cancellationToken
        );
    }

    public Task SetRepeatMode(
        RepeatState state,
        string? deviceId,
        CancellationToken cancellationToken
    ) =>
        webAPIClient.PutAsync(
            Endpoints.SetRepeatMode(state, deviceId),
            ModifyAccessToken,
            cancellationToken
        );

    public Task SetPlaybackVolume(
        int volumePercent,
        string? deviceId,
        CancellationToken cancellationToken
    )
    {
        ArgumentOutOfRangeException.ThrowIfNegative(volumePercent);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(volumePercent, 100);

        return webAPIClient.PutAsync(
            Endpoints.SetPlaybackVolume(volumePercent, deviceId),
            ModifyAccessToken,
            cancellationToken
        );
    }

    public Task SetPlaybackShuffle(
        bool state,
        string? deviceId,
        CancellationToken cancellationToken
    ) =>
        webAPIClient.PutAsync(
            Endpoints.TogglePlaybackShuffle(state, deviceId),
            ModifyAccessToken,
            cancellationToken
        );

    public IAsyncEnumerable<PlayHistory> GetRecentlyPlayedTracks(
        DateTimeOffset? after,
        DateTimeOffset? before,
        CancellationToken cancellationToken
    )
    {
        if (after is not null && before is not null)
        {
            throw new ArgumentException($"Give {nameof(after)} or {nameof(before)}, not both.");
        }

        // The next links keep the query's direction (after pages forward, before pages back),
        // so every page honours the bound.
        return webAPIClient.GetCursorPaginated<PlayHistory>(
            Endpoints.GetRecentlyPlayedTracks(after, before),
            authorizationService.GetAccessToken(AuthorizationScope.UserReadRecentlyPlayed),
            cancellationToken
        );
    }

    public Task<PlaybackQueue> GetQueue(CancellationToken cancellationToken) =>
        webAPIClient.GetAsync<PlaybackQueue>(
            Endpoints.GetUserQueue(),
            authorizationService.GetAccessToken(
                AuthorizationScope.UserReadCurrentlyPlaying,
                AuthorizationScope.UserReadPlaybackState
            ),
            cancellationToken
        );

    public Task AddItemToQueue(string uri, string? deviceId, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(uri);

        return webAPIClient.PostAsync(
            Endpoints.AddItemToPlaybackQueue(uri, deviceId),
            ModifyAccessToken,
            cancellationToken
        );
    }

    private Task StartPlayback(
        string? contextUri,
        IEnumerable<string>? uris,
        int? offsetPosition,
        string? offsetUri,
        int? positionMs,
        string? deviceId,
        CancellationToken cancellationToken
    )
    {
        if ((contextUri is null) == (uris is null))
        {
            throw new ArgumentException($"Give either {nameof(contextUri)} or {nameof(uris)}.");
        }

        object? offset = (offsetPosition, offsetUri) switch
        {
            (null, null) => null,
            ({ } position, null) => new { position },
            (null, { } uri) => new { uri },
            _ => throw new ArgumentException(
                $"Give the offset as {nameof(offsetPosition)} or {nameof(offsetUri)}, not both."
            ),
        };

        ThrowIfNegative(offsetPosition);
        ThrowIfNegative(positionMs);

        // Enumerated once, so the URIs checked are the ones sent.
        uris = uris is null ? null : NonNullUris(uris as ICollection<string> ?? uris.ToList());

        return webAPIClient.PutAsync(
            Endpoints.StartResumePlayback(deviceId),
            new
            {
                context_uri = contextUri,
                uris,
                offset,
                position_ms = positionMs,
            },
            ModifyAccessToken,
            cancellationToken
        );
    }

    private AccessTokenProvider ModifyAccessToken =>
        authorizationService.GetAccessToken(AuthorizationScope.UserModifyPlaybackState);
}
