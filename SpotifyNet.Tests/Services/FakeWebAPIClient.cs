using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SpotifyNet.Clients.Abstractions;

namespace SpotifyNet.Tests.Services;

/// <summary>
/// Answers from <see cref="Responses"/> by URI and records each request, getting a token for it
/// first like WebAPIClient.
/// </summary>
internal sealed class FakeWebAPIClient : IWebAPIClient
{
    public Dictionary<string, object?> Responses { get; } = [];

    public List<(string Method, string Uri, object? Body)> Requests { get; } = [];

    public Task<TResponse> GetAsync<TResponse>(
        string uri,
        AccessTokenProvider accessToken,
        CancellationToken cancellationToken
    ) => Respond<TResponse>("GET", uri, null, accessToken, cancellationToken);

    public Task<TResponse?> GetOrDefaultAsync<TResponse>(
        string uri,
        AccessTokenProvider accessToken,
        CancellationToken cancellationToken
    )
        where TResponse : class =>
        Respond<TResponse?>("GET", uri, null, accessToken, cancellationToken);

    public Task PutAsync(
        string uri,
        AccessTokenProvider accessToken,
        CancellationToken cancellationToken
    ) => Record("PUT", uri, null, accessToken, cancellationToken);

    public Task PutAsync<TBody>(
        string uri,
        TBody body,
        AccessTokenProvider accessToken,
        CancellationToken cancellationToken
    ) => Record("PUT", uri, body, accessToken, cancellationToken);

    public Task<TResponse> PutAsync<TBody, TResponse>(
        string uri,
        TBody body,
        AccessTokenProvider accessToken,
        CancellationToken cancellationToken
    ) => Respond<TResponse>("PUT", uri, body, accessToken, cancellationToken);

    public Task PutAsync(
        string uri,
        string body,
        string contentType,
        AccessTokenProvider accessToken,
        CancellationToken cancellationToken
    ) => Record("PUT", uri, body, accessToken, cancellationToken);

    public Task PostAsync(
        string uri,
        AccessTokenProvider accessToken,
        CancellationToken cancellationToken
    ) => Record("POST", uri, null, accessToken, cancellationToken);

    public Task<TResponse> PostAsync<TBody, TResponse>(
        string uri,
        TBody body,
        AccessTokenProvider accessToken,
        CancellationToken cancellationToken
    ) => Respond<TResponse>("POST", uri, body, accessToken, cancellationToken);

    public Task DeleteAsync(
        string uri,
        AccessTokenProvider accessToken,
        CancellationToken cancellationToken
    ) => Record("DELETE", uri, null, accessToken, cancellationToken);

    public Task<TResponse> DeleteAsync<TBody, TResponse>(
        string uri,
        TBody body,
        AccessTokenProvider accessToken,
        CancellationToken cancellationToken
    ) => Respond<TResponse>("DELETE", uri, body, accessToken, cancellationToken);

    private async Task Record(
        string method,
        string uri,
        object? body,
        AccessTokenProvider accessToken,
        CancellationToken cancellationToken
    )
    {
        await accessToken(cancellationToken);
        Requests.Add((method, uri, body));
    }

    private async Task<T> Respond<T>(
        string method,
        string uri,
        object? body,
        AccessTokenProvider accessToken,
        CancellationToken cancellationToken
    )
    {
        await Record(method, uri, body, accessToken, cancellationToken);

        return (T)Responses[uri]!;
    }
}
