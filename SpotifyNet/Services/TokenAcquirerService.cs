using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using SpotifyNet.Clients.Abstractions;
using SpotifyNet.Services.Abstractions;

namespace SpotifyNet.Services;

/// <summary>
/// The <see cref="ITokenAcquirerService"/> that opens the authorize URI in the browser, and listens
/// on the redirect URI with an <see cref="HttpListener"/> for the user's answer. One sign-in runs
/// at a time.
/// </summary>
/// <remarks>
/// Creating it throws <see cref="ArgumentException"/> unless the redirect URI is an http(s) URI
/// without a query or fragment.
/// </remarks>
public sealed class TokenAcquirerService(
    IAuthorizationRepository authorizationRepository,
    string redirectUri
) : ITokenAcquirerService
{
    private readonly string _listenerPrefix = ListenerPrefix(redirectUri);

    // Sign-ins share the redirect URI and the pending authorization, so run one at a time.
    private readonly SemaphoreSlim _signInLock = new(1, 1);

    /// <inheritdoc />
    public async Task EnsureTokenExists(
        IEnumerable<string> scopes,
        bool forceGenerate,
        CancellationToken cancellationToken
    )
    {
        var requested = scopes as ICollection<string> ?? scopes.ToList();

        await _signInLock.WaitAsync(cancellationToken);
        try
        {
            var stored = forceGenerate ? null : await StoredToken(cancellationToken);
            // Checked after refreshing, as Spotify may grant fewer scopes then.
            var current = stored is null ? null : await RefreshedToken(cancellationToken);

            if (current is null || requested.Except(current.AuthorizationScopes).Any())
            {
                // Ask for the granted scopes too, so the new token doesn't lose them.
                await GenerateToken(
                    [.. requested.Union(stored?.AuthorizationScopes ?? [])],
                    cancellationToken
                );
            }
        }
        finally
        {
            _signInLock.Release();
        }
    }

    // The stored token, unrefreshed; null if there's none or it's unreadable.
    private async Task<AccessTokenMetadata?> StoredToken(CancellationToken cancellationToken)
    {
        try
        {
            return await authorizationRepository.GetStoredAccessToken(cancellationToken);
        }
        catch (JsonException)
        {
            return null;
        }
    }

    // The stored token, refreshed if it's expiring; null if Spotify refused (e.g. revoked), as only
    // a new sign-in helps then.
    private async Task<AccessTokenMetadata?> RefreshedToken(CancellationToken cancellationToken)
    {
        try
        {
            return await authorizationRepository.GetAccessToken(cancellationToken);
        }
        catch (WebAPIException e) when (e.StatusCode == HttpStatusCode.BadRequest)
        {
            return null;
        }
    }

    private async Task GenerateToken(
        ICollection<string> scopes,
        CancellationToken cancellationToken
    )
    {
        var uri = await authorizationRepository.GetUserAuthorizeUri(scopes, cancellationToken);

        // A listener per sign-in, as one that failed to start can't be restarted.
        using var listener = new HttpListener { Prefixes = { _listenerPrefix } };

        // Listen before opening the browser, so the redirect can't arrive first.
        listener.Start();
        Process.Start(new ProcessStartInfo { FileName = uri, UseShellExecute = true })?.Dispose();

        var context = await listener.GetContextAsync().WaitAsync(cancellationToken);
        var query = context.Request.QueryString;

        try
        {
            if (query["error"] is { } error)
            {
                throw new AuthorizationException($"Spotify authorization failed: `{error}`.");
            }

            var code =
                query["code"]
                ?? throw new AuthorizationException("The authorization callback has no code.");
            var state =
                query["state"]
                ?? throw new AuthorizationException("The authorization callback has no state.");

            await authorizationRepository.GetNewAccessToken(code, state, cancellationToken);
        }
        catch
        {
            Respond(context.Response, "Sign-in failed. You can close this tab.");
            throw;
        }

        Respond(context.Response, "Signed in. You can close this tab.");
    }

    // HttpListener takes http(s) prefixes ending in a slash, and still receives the redirect
    // without one.
    private static string ListenerPrefix(string redirectUri)
    {
        if (
            !Uri.TryCreate(redirectUri, UriKind.Absolute, out var uri)
            || uri.Scheme is not ("http" or "https")
            || uri.Query.Length > 0
            || uri.Fragment.Length > 0
        )
        {
            throw new ArgumentException(
                "The redirect URI must be an http(s) URI without a query or fragment.",
                nameof(redirectUri)
            );
        }

        return redirectUri.EndsWith('/') ? redirectUri : redirectUri + '/';
    }

    // Best effort: the tab may be gone, and the sign-in's own outcome is what matters.
    private static void Respond(HttpListenerResponse response, string message)
    {
        try
        {
            response.ContentType = "text/plain; charset=utf-8";
            response.Close(Encoding.UTF8.GetBytes(message), willBlock: true);
        }
        catch (Exception e)
            when (e is HttpListenerException or IOException or ObjectDisposedException) { }
    }
}
