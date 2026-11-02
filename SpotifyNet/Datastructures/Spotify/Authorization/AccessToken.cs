using System.Text;
using System.Text.Json.Serialization;

namespace SpotifyNet.Datastructures.Spotify.Authorization;

public sealed record AccessToken
{
    [JsonPropertyName("access_token")]
    public required string Token { get; init; }

    [JsonPropertyName("token_type")]
    public required string TokenType { get; init; }

    [JsonPropertyName("scope")]
    public string? Scope { get; init; }

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
