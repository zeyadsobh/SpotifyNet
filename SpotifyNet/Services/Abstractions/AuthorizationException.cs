using System;

namespace SpotifyNet.Services.Abstractions;

public class AuthorizationException : Exception
{
    public AuthorizationException(string message)
        : base(message) { }

    public AuthorizationException(string message, Exception innerException)
        : base(message, innerException) { }
}
