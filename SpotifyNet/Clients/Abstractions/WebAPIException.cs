using System;
using System.Net;

namespace SpotifyNet.Clients.Abstractions;

/// <summary>
/// A Web API or token request failed: Spotify answered with an error status, or with a body that
/// can't be parsed.
/// </summary>
public class WebAPIException : Exception
{
    /// <summary>Creates the exception for a response with an error status.</summary>
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

    /// <summary>Creates the exception for a response whose body couldn't be parsed.</summary>
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

    /// <summary>The URI the failed request was sent to.</summary>
    public string RequestUri { get; }

    /// <summary>The response's HTTP status code.</summary>
    public HttpStatusCode StatusCode { get; }

    /// <summary>Spotify's error message, if the response body carried one.</summary>
    public string? ErrorMessage { get; }

    /// <summary>
    /// The response body; withheld when a token response couldn't be parsed, as it may hold tokens.
    /// </summary>
    public string Content { get; }

    /// <summary>The <c>Retry-After</c> delay, if the response had one (e.g. on 429).</summary>
    public TimeSpan? RetryAfter { get; }
}
