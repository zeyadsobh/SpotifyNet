using System.Threading;
using System.Threading.Tasks;

namespace SpotifyNet.Clients.Abstractions;

public interface IWebAPIClient
{
    Task<TResponse> GetAsync<TResponse>(
        string uri,
        AccessTokenProvider accessToken,
        CancellationToken cancellationToken = default
    );

    /// <summary>Like <see cref="GetAsync{TResponse}"/>, but returns <see langword="null"/> on 204 No Content.</summary>
    Task<TResponse?> GetOrDefaultAsync<TResponse>(
        string uri,
        AccessTokenProvider accessToken,
        CancellationToken cancellationToken = default
    )
        where TResponse : class;

    Task PutAsync(
        string uri,
        AccessTokenProvider accessToken,
        CancellationToken cancellationToken = default
    );

    Task PutAsync<TBody>(
        string uri,
        TBody body,
        AccessTokenProvider accessToken,
        CancellationToken cancellationToken = default
    );

    Task<TResponse> PutAsync<TBody, TResponse>(
        string uri,
        TBody body,
        AccessTokenProvider accessToken,
        CancellationToken cancellationToken = default
    );

    Task PutAsync(
        string uri,
        string body,
        string contentType,
        AccessTokenProvider accessToken,
        CancellationToken cancellationToken = default
    );

    Task PostAsync(
        string uri,
        AccessTokenProvider accessToken,
        CancellationToken cancellationToken = default
    );

    Task<TResponse> PostAsync<TBody, TResponse>(
        string uri,
        TBody body,
        AccessTokenProvider accessToken,
        CancellationToken cancellationToken = default
    );

    Task DeleteAsync(
        string uri,
        AccessTokenProvider accessToken,
        CancellationToken cancellationToken = default
    );

    Task<TResponse> DeleteAsync<TBody, TResponse>(
        string uri,
        TBody body,
        AccessTokenProvider accessToken,
        CancellationToken cancellationToken = default
    );
}
