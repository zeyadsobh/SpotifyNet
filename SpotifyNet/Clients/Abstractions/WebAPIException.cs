using System;
using System.Net;

namespace SpotifyNet.Clients.Abstractions;

public class WebAPIException : Exception
{
    public WebAPIException(
        string requestUri,
        HttpStatusCode statusCode,
        string? errorMessage,
        string content,
        TimeSpan? retryAfter = null
    )
        : base(
            $"Request to {requestUri} failed with {(int)statusCode} ({statusCode}). {errorMessage ?? content}".TrimEnd()
        )
    {
        RequestUri = requestUri;
        StatusCode = statusCode;
        ErrorMessage = errorMessage;
        Content = content;
        RetryAfter = retryAfter;
    }

    public WebAPIException(
        string requestUri,
        HttpStatusCode statusCode,
        string content,
        Exception innerException
    )
        : base($"Parsing the response from {requestUri} failed: {content}", innerException)
    {
        RequestUri = requestUri;
        StatusCode = statusCode;
        Content = content;
    }

    public string RequestUri { get; }

    public HttpStatusCode StatusCode { get; }

    /// <summary>Spotify's error message, if the response body carried one.</summary>
    public string? ErrorMessage { get; }

    public string Content { get; }

    /// <summary>The <c>Retry-After</c> delay, if the response had one (e.g. on 429).</summary>
    public TimeSpan? RetryAfter { get; }
}
