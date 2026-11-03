using System.Text;

namespace SpotifyNet.Datastructures.Spotify.Authorization;

/// <summary>
/// A sign-in's authorize URI, with the PKCE code verifier and state to keep until Spotify redirects
/// back. Its <c>ToString</c> leaves the code verifier out, so logging it doesn't leak it.
/// </summary>
public sealed record UserAuthorization
{
    /// <summary>The URI to open in a browser, asking the user to authorize the app.</summary>
    public required string AuthorizationUri { get; init; }

    /// <summary>
    /// The PKCE code verifier to give when exchanging the authorization code; keep it secret.
    /// </summary>
    public required string CodeVerifier { get; init; }

    /// <summary>
    /// The random state sent with the request, which the redirect must return unchanged.
    /// </summary>
    public required string State { get; init; }

    // Keeps the code verifier out of ToString, so logging this doesn't leak it.
    private bool PrintMembers(StringBuilder builder)
    {
        builder.Append($"AuthorizationUri = {AuthorizationUri}, State = {State}");
        return true;
    }
}
