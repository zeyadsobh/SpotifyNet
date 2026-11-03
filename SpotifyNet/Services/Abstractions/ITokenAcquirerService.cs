using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SpotifyNet.Clients.Abstractions;
using SpotifyNet.Datastructures.Spotify.Authorization;

namespace SpotifyNet.Services.Abstractions;

/// <summary>Signs the user in through the browser, so there's an access token to use.</summary>
public interface ITokenAcquirerService
{
    /// <summary>
    /// Signs in through the browser unless the stored token has <paramref name="scopes"/>. Scopes
    /// granted earlier are asked for again, so they're kept; <paramref name="forceGenerate"/>
    /// signs in afresh with just <paramref name="scopes"/>.
    /// </summary>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="scopes"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// A scope isn't one of <see cref="AuthorizationScope.ValidScopes"/>.
    /// </exception>
    /// <exception cref="AuthorizationException">
    /// The user denied access, or the redirect back was invalid.
    /// </exception>
    /// <exception cref="WebAPIException">
    /// Spotify refused to exchange the authorization code.
    /// </exception>
    Task EnsureTokenExists(
        IEnumerable<string> scopes,
        bool forceGenerate = false,
        CancellationToken cancellationToken = default
    );
}
