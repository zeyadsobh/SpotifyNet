namespace SpotifyNet.Datastructures.Spotify;

/// <summary>
/// A kind of top item for the current user: <see cref="Artists.Artist"/> or <see cref="Tracks.Track"/>.
/// </summary>
public interface ITopItem
{
    /// <summary>The <c>{type}</c> of <c>/me/top/{type}</c> that returns this kind.</summary>
    static abstract string TopItemType { get; }
}
