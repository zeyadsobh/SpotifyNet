using System;
using System.Buffers.Text;
using System.Security.Cryptography;
using System.Text;

namespace SpotifyNet.Clients;

public static class PKCE
{
    private const int MinimumCodeVerifierLength = 43;
    private const int MaximumCodeVerifierLength = 128;

    // The characters RFC 7636 allows in a code verifier.
    private const string VerifierCharacters =
        "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789-._~";

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
