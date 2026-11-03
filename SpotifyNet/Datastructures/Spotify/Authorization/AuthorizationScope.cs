using System.Collections.Frozen;
using System.Collections.Generic;

namespace SpotifyNet.Datastructures.Spotify.Authorization;

/// <summary>
/// The scopes an app can ask the user for, each deciding what its access token may do.
/// </summary>
public static class AuthorizationScope
{
    // Images
    /// <summary>Write access to user-provided images, such as a custom playlist cover.</summary>
    public const string UgcImageUpload = "ugc-image-upload";

    // Spotify Connect
    /// <summary>
    /// Read access to a user's player state: their playback and Spotify Connect devices.
    /// </summary>
    public const string UserReadPlaybackState = "user-read-playback-state";

    /// <summary>
    /// Write access to a user's playback state: control playback on their devices.
    /// </summary>
    public const string UserModifyPlaybackState = "user-modify-playback-state";

    /// <summary>Read access to a user's currently playing content.</summary>
    public const string UserReadCurrentlyPlaying = "user-read-currently-playing";

    // Playback
    /// <summary>
    /// Remote control playback of Spotify; available to the Spotify iOS and Android SDKs.
    /// </summary>
    public const string AppRemoteControl = "app-remote-control";

    /// <summary>
    /// Control playback of a Spotify track; available to the Web Playback SDK, for Premium users.
    /// </summary>
    public const string Streaming = "streaming";

    // Playlists
    /// <summary>Read access to a user's private playlists.</summary>
    public const string PlaylistReadPrivate = "playlist-read-private";

    /// <summary>Include collaborative playlists when requesting a user's playlists.</summary>
    public const string PlaylistReadCollaborative = "playlist-read-collaborative";

    /// <summary>Write access to a user's private playlists.</summary>
    public const string PlaylistModifyPrivate = "playlist-modify-private";

    /// <summary>Write access to a user's public playlists.</summary>
    public const string PlaylistModifyPublic = "playlist-modify-public";

    // Follow
    /// <summary>
    /// Write and delete access to the list of artists and other users the user follows.
    /// </summary>
    public const string UserFollowModify = "user-follow-modify";

    /// <summary>Read access to the list of artists and other users the user follows.</summary>
    public const string UserFollowRead = "user-follow-read";

    // Listening History
    /// <summary>Read access to a user's playback position in content, such as episodes.</summary>
    public const string UserReadPlaybackPosition = "user-read-playback-position";

    /// <summary>Read access to a user's top artists and tracks.</summary>
    public const string UserTopRead = "user-top-read";

    /// <summary>Read access to a user's recently played tracks.</summary>
    public const string UserReadRecentlyPlayed = "user-read-recently-played";

    // Library
    /// <summary>Write and delete access to a user's <c>Your Music</c> library.</summary>
    public const string UserLibraryModify = "user-library-modify";

    /// <summary>Read access to a user's library.</summary>
    public const string UserLibraryRead = "user-library-read";

    // Users
    /// <summary>Read access to a user's email address.</summary>
    public const string UserReadEmail = "user-read-email";

    /// <summary>Read access to a user's subscription details (type of user account).</summary>
    public const string UserReadPrivate = "user-read-private";

    /// <summary>Get personalized content for the user.</summary>
    public const string UserPersonalized = "user-personalized";

    // Open Access
    /// <summary>Link a partner user account to a Spotify user account (Open Access).</summary>
    public const string UserSoaLink = "user-soa-link";

    /// <summary>Unlink a partner user account from a Spotify account (Open Access).</summary>
    public const string UserSoaUnlink = "user-soa-unlink";

    /// <summary>Modify entitlements for linked users (Open Access).</summary>
    public const string SoaManageEntitlements = "soa-manage-entitlements";

    /// <summary>Update partner information (Open Access).</summary>
    public const string SoaManagePartner = "soa-manage-partner";

    /// <summary>Create new partners; platform partners only (Open Access).</summary>
    public const string SoaCreatePartner = "soa-create-partner";

    /// <summary>Every scope defined here, the only ones that can be requested.</summary>
    public static readonly IReadOnlySet<string> ValidScopes = new[]
    {
        UgcImageUpload,
        UserReadPlaybackState,
        UserModifyPlaybackState,
        UserReadCurrentlyPlaying,
        AppRemoteControl,
        Streaming,
        PlaylistReadPrivate,
        PlaylistReadCollaborative,
        PlaylistModifyPrivate,
        PlaylistModifyPublic,
        UserFollowModify,
        UserFollowRead,
        UserReadPlaybackPosition,
        UserTopRead,
        UserReadRecentlyPlayed,
        UserLibraryModify,
        UserLibraryRead,
        UserReadEmail,
        UserReadPrivate,
        UserPersonalized,
        UserSoaLink,
        UserSoaUnlink,
        SoaManageEntitlements,
        SoaManagePartner,
        SoaCreatePartner,
    }.ToFrozenSet();
}
