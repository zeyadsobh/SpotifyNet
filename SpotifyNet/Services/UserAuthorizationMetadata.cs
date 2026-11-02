using System.Collections.Immutable;
using System.Text;

namespace SpotifyNet.Services;

internal sealed record UserAuthorizationMetadata
{
    public required ImmutableArray<string> AuthorizationScopes { get; init; }

    public required string CodeVerifier { get; init; }

    public required string State { get; init; }

    // Keeps the code verifier out of ToString, so logging this doesn't leak it.
    private bool PrintMembers(StringBuilder builder)
    {
        builder.Append(
            $"AuthorizationScopes = [{string.Join(", ", AuthorizationScopes)}], State = {State}"
        );
        return true;
    }
}
