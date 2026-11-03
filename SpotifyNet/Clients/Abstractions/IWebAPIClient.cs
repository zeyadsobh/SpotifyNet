using System.Threading;
using System.Threading.Tasks;

namespace SpotifyNet.Clients.Abstractions;

/// <summary>
/// Sends authorized requests to the Spotify Web API, with JSON bodies and responses.
/// </summary>
/// <remarks>
/// Every attempt asks the <see cref="AccessTokenProvider"/> for a token. Failures throw
/// <see cref="WebAPIException"/>: an error status, or a response that can't be deserialized.
/// </remarks>
public interface IWebAPIClient
{
    /// <summary>Sends a GET request and deserializes the response.</summary>
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

    /// <summary>Sends a PUT request without a body.</summary>
    Task PutAsync(
        string uri,
        AccessTokenProvider accessToken,
        CancellationToken cancellationToken = default
    );

    /// <summary>Sends a PUT request with <paramref name="body"/> as JSON.</summary>
    Task PutAsync<TBody>(
        string uri,
        TBody body,
        AccessTokenProvider accessToken,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Sends a PUT request with <paramref name="body"/> as JSON and deserializes the response.
    /// </summary>
    Task<TResponse> PutAsync<TBody, TResponse>(
        string uri,
        TBody body,
        AccessTokenProvider accessToken,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Sends a PUT request with the text <paramref name="body"/> as <paramref name="contentType"/>.
    /// </summary>
    Task PutAsync(
        string uri,
        string body,
        string contentType,
        AccessTokenProvider accessToken,
        CancellationToken cancellationToken = default
    );

    /// <summary>Sends a POST request without a body.</summary>
    Task PostAsync(
        string uri,
        AccessTokenProvider accessToken,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Sends a POST request with <paramref name="body"/> as JSON and deserializes the response.
    /// </summary>
    Task<TResponse> PostAsync<TBody, TResponse>(
        string uri,
        TBody body,
        AccessTokenProvider accessToken,
        CancellationToken cancellationToken = default
    );

    /// <summary>Sends a DELETE request without a body.</summary>
    Task DeleteAsync(
        string uri,
        AccessTokenProvider accessToken,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Sends a DELETE request with <paramref name="body"/> as JSON and deserializes the response.
    /// </summary>
    Task<TResponse> DeleteAsync<TBody, TResponse>(
        string uri,
        TBody body,
        AccessTokenProvider accessToken,
        CancellationToken cancellationToken = default
    );
}
