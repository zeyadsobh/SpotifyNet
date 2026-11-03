using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using SpotifyNet.Clients.Abstractions;
using SpotifyNet.Datastructures.Spotify;

namespace SpotifyNet.Clients;

/// <summary>
/// The <see cref="IWebAPIClient"/> over an <see cref="HttpClient"/>. Null members are left out of
/// JSON bodies, and a rate-limited request (429) is retried after its <c>Retry-After</c> delay, up
/// to 3 times, if that delay is 60 seconds or less.
/// </summary>
public sealed class WebAPIClient(HttpClient httpClient) : IWebAPIClient
{
    private const int MaxRetries = 3;

    private static readonly TimeSpan MaxRetryAfter = TimeSpan.FromSeconds(60);

    // Optional body fields are left out rather than sent as null.
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web)
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
    };

    /// <inheritdoc />
    public Task<TResponse> GetAsync<TResponse>(
        string uri,
        AccessTokenProvider accessToken,
        CancellationToken cancellationToken
    ) => SendAsync<TResponse>(HttpMethod.Get, uri, null, accessToken, cancellationToken);

    /// <inheritdoc />
    public async Task<TResponse?> GetOrDefaultAsync<TResponse>(
        string uri,
        AccessTokenProvider accessToken,
        CancellationToken cancellationToken
    )
        where TResponse : class
    {
        using var response = await RequestAsync(
            HttpMethod.Get,
            uri,
            null,
            accessToken,
            cancellationToken
        );

        return response.StatusCode == HttpStatusCode.NoContent
            ? null
            : await ReadAsync<TResponse>(uri, response, cancellationToken);
    }

    /// <inheritdoc />
    public Task PutAsync(
        string uri,
        AccessTokenProvider accessToken,
        CancellationToken cancellationToken
    ) => SendAsync(HttpMethod.Put, uri, null, accessToken, cancellationToken);

    /// <inheritdoc />
    public Task PutAsync<TBody>(
        string uri,
        TBody body,
        AccessTokenProvider accessToken,
        CancellationToken cancellationToken
    ) => SendAsync(HttpMethod.Put, uri, Json(body), accessToken, cancellationToken);

    /// <inheritdoc />
    public Task<TResponse> PutAsync<TBody, TResponse>(
        string uri,
        TBody body,
        AccessTokenProvider accessToken,
        CancellationToken cancellationToken
    ) => SendAsync<TResponse>(HttpMethod.Put, uri, Json(body), accessToken, cancellationToken);

    /// <inheritdoc />
    public Task PutAsync(
        string uri,
        string body,
        string contentType,
        AccessTokenProvider accessToken,
        CancellationToken cancellationToken
    ) =>
        SendAsync(
            HttpMethod.Put,
            uri,
            () => new StringContent(body, Encoding.UTF8, contentType),
            accessToken,
            cancellationToken
        );

    /// <inheritdoc />
    public Task PostAsync(
        string uri,
        AccessTokenProvider accessToken,
        CancellationToken cancellationToken
    ) => SendAsync(HttpMethod.Post, uri, null, accessToken, cancellationToken);

    /// <inheritdoc />
    public Task<TResponse> PostAsync<TBody, TResponse>(
        string uri,
        TBody body,
        AccessTokenProvider accessToken,
        CancellationToken cancellationToken
    ) => SendAsync<TResponse>(HttpMethod.Post, uri, Json(body), accessToken, cancellationToken);

    /// <inheritdoc />
    public Task DeleteAsync(
        string uri,
        AccessTokenProvider accessToken,
        CancellationToken cancellationToken
    ) => SendAsync(HttpMethod.Delete, uri, null, accessToken, cancellationToken);

    /// <inheritdoc />
    public Task<TResponse> DeleteAsync<TBody, TResponse>(
        string uri,
        TBody body,
        AccessTokenProvider accessToken,
        CancellationToken cancellationToken
    ) => SendAsync<TResponse>(HttpMethod.Delete, uri, Json(body), accessToken, cancellationToken);

    // Serialized once, up front, so every attempt sends the body as passed, even if the caller
    // changes it while a request waits.
    private static Func<HttpContent> Json<TBody>(TBody body)
    {
        var json = JsonSerializer.SerializeToUtf8Bytes(body, JsonOptions);

        return () =>
            new ByteArrayContent(json)
            {
                Headers = { ContentType = new("application/json", "utf-8") },
            };
    }

    private async Task SendAsync(
        HttpMethod method,
        string uri,
        Func<HttpContent>? content,
        AccessTokenProvider accessToken,
        CancellationToken cancellationToken
    )
    {
        using var response = await RequestAsync(
            method,
            uri,
            content,
            accessToken,
            cancellationToken
        );
    }

    private async Task<TResponse> SendAsync<TResponse>(
        HttpMethod method,
        string uri,
        Func<HttpContent>? content,
        AccessTokenProvider accessToken,
        CancellationToken cancellationToken
    )
    {
        using var response = await RequestAsync(
            method,
            uri,
            content,
            accessToken,
            cancellationToken
        );

        return await ReadAsync<TResponse>(uri, response, cancellationToken);
    }

    // Takes a content factory because a request can only be sent once, and 429s are re-sent.
    private async Task<HttpResponseMessage> RequestAsync(
        HttpMethod method,
        string uri,
        Func<HttpContent>? content,
        AccessTokenProvider accessToken,
        CancellationToken cancellationToken
    )
    {
        for (var retries = 0; ; retries++)
        {
            using var request = new HttpRequestMessage(method, uri) { Content = content?.Invoke() };
            request.Headers.Authorization = new("Bearer", await accessToken(cancellationToken));

            var response = await httpClient.SendAsync(request, cancellationToken);
            if (response.IsSuccessStatusCode)
            {
                return response;
            }

            using (response)
            {
                var retryAfter = response.Headers.RetryAfter?.Delta;
                if (
                    response.StatusCode == HttpStatusCode.TooManyRequests
                    && retries < MaxRetries
                    && retryAfter is { } delay
                    && delay <= MaxRetryAfter
                )
                {
                    await Task.Delay(delay, cancellationToken);
                    continue;
                }

                var body = await response.Content.ReadAsStringAsync(cancellationToken);

                throw new WebAPIException(
                    uri,
                    response.StatusCode,
                    ErrorMessage(body),
                    body,
                    retryAfter
                );
            }
        }
    }

    // Errors are documented as {"error": ErrorObject}, except /me/library's 400s: a bare ErrorObject.
    private static string? ErrorMessage(string body)
    {
        try
        {
            using var document = JsonDocument.Parse(body);
            var root = document.RootElement;
            var error =
                root.ValueKind == JsonValueKind.Object
                && root.TryGetProperty("error", out var inner)
                    ? inner
                    : root;

            return error.Deserialize<Error>(JsonOptions)?.Message;
        }
        catch (JsonException)
        {
            return null;
        }
    }

    private static async Task<TResponse> ReadAsync<TResponse>(
        string uri,
        HttpResponseMessage response,
        CancellationToken cancellationToken
    )
    {
        try
        {
            var result = await response.Content.ReadFromJsonAsync<TResponse>(
                JsonOptions,
                cancellationToken
            );

            return result ?? throw new JsonException("The response body is null.");
        }
        catch (JsonException e)
        {
            var content = await response.Content.ReadAsStringAsync(cancellationToken);

            throw new WebAPIException(uri, response.StatusCode, content, e);
        }
    }
}
