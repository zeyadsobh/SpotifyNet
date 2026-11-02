using System;
using System.Collections.Immutable;
using System.Text;

namespace SpotifyNet.Services.Abstractions;

public sealed record AccessTokenMetadata
{
    public required ImmutableArray<string> AuthorizationScopes { get; init; }

    public required string Token { get; init; }

    public required DateTimeOffset ExpiresAtUtc { get; init; }

    public required string RefreshToken { get; init; }

    public bool IsExpiring => ExpiresAtUtc <= DateTimeOffset.UtcNow.AddMinutes(1);

    // Keeps the tokens out of ToString, so logging this doesn't leak them.
    private bool PrintMembers(StringBuilder builder)
    {
        builder.Append(
            $"AuthorizationScopes = [{string.Join(", ", AuthorizationScopes)}], ExpiresAtUtc = {ExpiresAtUtc}"
        );
        return true;
    }
}
