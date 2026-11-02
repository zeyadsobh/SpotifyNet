using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using SpotifyNet.Clients.Abstractions;
using SpotifyNet.Datastructures.Spotify;

namespace SpotifyNet.Services.WebAPI;

internal static class WebAPIClientExtensions
{
    public static IAsyncEnumerable<T> GetOffsetPaginated<T>(
        this IWebAPIClient webAPIClient,
        string uri,
        AccessTokenProvider accessToken,
        CancellationToken cancellationToken
    ) =>
        webAPIClient.Paginate<OffsetPaginationWrapper<T>, T>(
            uri,
            accessToken,
            page => (page.Items, page.Next),
            cancellationToken
        );

    public static IAsyncEnumerable<T> GetCursorPaginated<T>(
        this IWebAPIClient webAPIClient,
        string uri,
        AccessTokenProvider accessToken,
        CancellationToken cancellationToken
    ) =>
        webAPIClient.Paginate<CursorPaginationWrapper<T>, T>(
            uri,
            accessToken,
            page => (page.Items ?? [], page.Next),
            cancellationToken
        );

    // Follows the next links, fetching each page only once the previous one is consumed.
    public static async IAsyncEnumerable<T> Paginate<TPage, T>(
        this IWebAPIClient webAPIClient,
        string uri,
        AccessTokenProvider accessToken,
        Func<TPage, (IEnumerable<T> Items, string? Next)> read,
        [EnumeratorCancellation] CancellationToken cancellationToken
    )
    {
        for (string? next = uri; next is not null; )
        {
            (var items, next) = read(
                await webAPIClient.GetAsync<TPage>(next, accessToken, cancellationToken)
            );

            foreach (var item in items)
            {
                cancellationToken.ThrowIfCancellationRequested();
                yield return item;
            }
        }
    }

    // Spotify caps the URIs per request; checking first gives a clearer error than its 400.
    public static ICollection<string> Uris(IEnumerable<string> uris, int minimum, int maximum)
    {
        ArgumentNullException.ThrowIfNull(uris);

        var collection = uris as ICollection<string> ?? uris.ToList();
        if (collection.Count < minimum || collection.Count > maximum)
        {
            throw new ArgumentOutOfRangeException(
                nameof(uris),
                collection.Count,
                $"Give between {minimum} and {maximum} URIs."
            );
        }

        return NonNullUris(collection);
    }

    // A null URI would be sent as JSON null, or break building the query.
    public static T NonNullUris<T>(T uris)
        where T : IEnumerable<string>
    {
        if (uris.Any(uri => uri is null))
        {
            throw new ArgumentException("The URIs can't include null.", nameof(uris));
        }

        return uris;
    }

    // ArgumentOutOfRangeException.ThrowIfNegative, for optional values too.
    public static void ThrowIfNegative(
        int? value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null
    )
    {
        if (value is { } number)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(number, paramName);
        }
    }
}
