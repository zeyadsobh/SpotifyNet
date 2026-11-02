namespace SpotifyNet.Clients.Options;

public sealed record AuthorizationClientOptions
{
    public const string SectionName = "Spotify:AuthorizationClient";

    public required string AppClientId { get; init; }

    public required string AppRedirectUri { get; init; }
}
