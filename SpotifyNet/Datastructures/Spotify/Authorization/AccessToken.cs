using System.Text;
using System.Text.Json.Serialization;

namespace SpotifyNet.Datastructures.Spotify.Authorization;

/// <summary>
/// Spotify's answer to a token request: the access token, how long it lasts and, usually, a refresh
/// token. Its <c>ToString</c> leaves the tokens out, so logging it doesn't leak them.
/// </summary>
public sealed record AccessToken
{
    /// <summary>The access token, to send as a bearer token with Web API requests.</summary>
    [JsonPropertyName("access_token")]
    public required string Token { get; init; }

    /// <summary>How the token is used; <c>Bearer</c>.</summary>
    [JsonPropertyName("token_type")]
    public required string TokenType { get; init; }

    /// <summary>
    /// The scopes the token was granted, separated by spaces; when omitted, the requested ones
    /// count as granted.
    /// </summary>
    [JsonPropertyName("scope")]
    public string? Scope { get; init; }

    /// <summary>How long the access token is valid for, in seconds.</summary>
    [JsonPropertyName("expires_in")]
    public required int ExpiresIn { get; init; }

    /// <summary>Left out of refresh responses when the current refresh token stays valid.</summary>
    [JsonPropertyName("refresh_token")]
    public string? RefreshToken { get; init; }

    // Keeps the tokens out of ToString, so logging this doesn't leak them.
    private bool PrintMembers(StringBuilder builder)
    {
        builder.Append($"TokenType = {TokenType}, Scope = {Scope}, ExpiresIn = {ExpiresIn}");
        return true;
    }
}
