using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SpotifyNet.Clients.Abstractions;

namespace SpotifyNet.Services.Abstractions;

/// <summary>Gives requests an access token that has the scopes they need.</summary>
public interface IAuthorizationService
{
    /// <summary>
    /// Gets the access token, refreshing it first if it's expiring, once it's known to have
    /// <paramref name="scopes"/>.
    /// </summary>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="scopes"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="AuthorizationException">
    /// There's no access token yet, or it lacks one of <paramref name="scopes"/>.
    /// </exception>
    /// <exception cref="WebAPIException">Refreshing the access token failed.</exception>
    Task<string> GetAccessToken(
        IEnumerable<string> scopes,
        CancellationToken cancellationToken = default
    );

    /// <summary>A provider that checks the token has <paramref name="scopes"/> on every request.</summary>
    AccessTokenProvider GetAccessToken(params string[] scopes) =>
        cancellationToken => GetAccessToken(scopes, cancellationToken);
}
