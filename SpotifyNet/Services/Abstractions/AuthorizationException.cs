using System;

namespace SpotifyNet.Services.Abstractions;

/// <summary>
/// A request couldn't be authorized: there's no access token yet, it lacks a scope, or signing in
/// failed.
/// </summary>
public class AuthorizationException : Exception
{
    /// <summary>Creates the exception with an error message.</summary>
    public AuthorizationException(string message)
        : base(message) { }

    /// <summary>
    /// Creates the exception with an error message and the exception that caused it.
    /// </summary>
    public AuthorizationException(string message, Exception innerException)
        : base(message, innerException) { }
}
