using System.Text;

namespace SpotifyNet.Datastructures.Spotify.Authorization;

public sealed record UserAuthorization
{
    public required string AuthorizationUri { get; init; }

    public required string CodeVerifier { get; init; }

    public required string State { get; init; }

    // Keeps the code verifier out of ToString, so logging this doesn't leak it.
    private bool PrintMembers(StringBuilder builder)
    {
        builder.Append($"AuthorizationUri = {AuthorizationUri}, State = {State}");
        return true;
    }
}
