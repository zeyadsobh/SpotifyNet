using System.Collections.Frozen;
using System.Collections.Generic;

namespace SpotifyNet.Datastructures.Spotify.Authorization;

public static class AuthorizationScope
{
    // Images
    public const string UgcImageUpload = "ugc-image-upload";

    // Spotify Connect
    public const string UserReadPlaybackState = "user-read-playback-state";
    public const string UserModifyPlaybackState = "user-modify-playback-state";
    public const string UserReadCurrentlyPlaying = "user-read-currently-playing";

    // Playback
    public const string AppRemoteControl = "app-remote-control";
    public const string Streaming = "streaming";

    // Playlists
    public const string PlaylistReadPrivate = "playlist-read-private";
    public const string PlaylistReadCollaborative = "playlist-read-collaborative";
    public const string PlaylistModifyPrivate = "playlist-modify-private";
    public const string PlaylistModifyPublic = "playlist-modify-public";

    // Follow
    public const string UserFollowModify = "user-follow-modify";
    public const string UserFollowRead = "user-follow-read";

    // Listening History
    public const string UserReadPlaybackPosition = "user-read-playback-position";
    public const string UserTopRead = "user-top-read";
    public const string UserReadRecentlyPlayed = "user-read-recently-played";

    // Library
    public const string UserLibraryModify = "user-library-modify";
    public const string UserLibraryRead = "user-library-read";

    // Users
    public const string UserReadEmail = "user-read-email";
    public const string UserReadPrivate = "user-read-private";
    public const string UserPersonalized = "user-personalized";

    // Open Access
    public const string UserSoaLink = "user-soa-link";
    public const string UserSoaUnlink = "user-soa-unlink";
    public const string SoaManageEntitlements = "soa-manage-entitlements";
    public const string SoaManagePartner = "soa-manage-partner";
    public const string SoaCreatePartner = "soa-create-partner";

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
