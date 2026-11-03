using System;
using System.Collections.Immutable;
using System.Text;

namespace SpotifyNet.Services.Abstractions;

/// <summary>
/// An access token with the scopes it was granted, when it expires and the refresh token that
/// renews it. Its <c>ToString</c> leaves the tokens out, so logging it doesn't leak them.
/// </summary>
public sealed record AccessTokenMetadata
{
    /// <summary>The scopes the token was granted.</summary>
    public required ImmutableArray<string> AuthorizationScopes { get; init; }

    /// <summary>The access token, to send as a bearer token.</summary>
    public required string Token { get; init; }

    /// <summary>When the access token expires.</summary>
    public required DateTimeOffset ExpiresAtUtc { get; init; }

    /// <summary>The refresh token that gets a new access token once this one expires.</summary>
    public required string RefreshToken { get; init; }

    /// <summary>Whether the access token has expired, or will within a minute.</summary>
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
