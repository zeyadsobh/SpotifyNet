namespace SpotifyNet.Clients.Options;

/// <summary>
/// The Spotify app's settings for signing in, read from the <see cref="SectionName"/> section.
/// </summary>
public sealed record AuthorizationClientOptions
{
    /// <summary>The configuration section these options are read from.</summary>
    public const string SectionName = "Spotify:AuthorizationClient";

    /// <summary>The app's client ID, from its page on the Spotify developer dashboard.</summary>
    public required string AppClientId { get; init; }

    /// <summary>
    /// The redirect URI registered for the app. Signing in through
    /// <see cref="Services.TokenAcquirerService"/> needs an http(s) one with no query or fragment,
    /// as it listens there for the redirect.
    /// </summary>
    public required string AppRedirectUri { get; init; }
}
