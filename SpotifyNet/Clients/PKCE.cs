using System;
using System.Buffers.Text;
using System.Security.Cryptography;
using System.Text;

namespace SpotifyNet.Clients;

/// <summary>
/// Proof Key for Code Exchange (RFC 7636): the code verifier and challenge the authorization code
/// flow uses in place of a client secret.
/// </summary>
public static class PKCE
{
    private const int MinimumCodeVerifierLength = 43;
    private const int MaximumCodeVerifierLength = 128;

    // The characters RFC 7636 allows in a code verifier.
    private const string VerifierCharacters =
        "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789-._~";

    /// <summary>
    /// Generates a random code verifier, and the S256 challenge to send in its place with the
    /// authorization request.
    /// </summary>
    /// <param name="verifierLength">The verifier's length, 43 to 128.</param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="verifierLength"/> is outside 43 to 128.
    /// </exception>
    public static (string Verifier, string Challenge) GetCodeVerifierAndChallenge(
        int verifierLength
    )
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(verifierLength, MinimumCodeVerifierLength);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(verifierLength, MaximumCodeVerifierLength);

        var verifier = RandomNumberGenerator.GetString(VerifierCharacters, verifierLength);
        var challenge = Base64Url.EncodeToString(
            SHA256.HashData(Encoding.ASCII.GetBytes(verifier))
        );

        return (verifier, challenge);
    }
}
