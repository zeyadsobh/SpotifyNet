using System;
using System.Net.Http;
using System.Threading;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SpotifyNet.Clients;
using SpotifyNet.Clients.Abstractions;
using SpotifyNet.Clients.Options;
using SpotifyNet.Services;
using SpotifyNet.Services.Abstractions;
using SpotifyNet.Services.Abstractions.WebAPI;
using SpotifyNet.Services.WebAPI;

namespace SpotifyNet.DependencyInjection;

/// <summary>Extensions that register SpotifyNet on an <see cref="IServiceCollection"/>.</summary>
public static class ServiceCollectionExtensions
{
    private const string HttpClientName = "SpotifyNet";

    private static IServiceCollection AddAuthorization(this IServiceCollection services)
    {
        services
            .AddOptions<AuthorizationClientOptions>()
            .BindConfiguration(AuthorizationClientOptions.SectionName);

        return services
            .AddSingleton<IAuthorizationClient>(p => new AuthorizationClient(
                p.GetRequiredService<IOptions<AuthorizationClientOptions>>(),
                HttpClient(p)
            ))
            .AddSingleton<IAuthorizationRepository, AuthorizationRepository>()
            .AddSingleton<IAuthorizationService, AuthorizationService>();
    }

    private static IServiceCollection AddTokenAcquirer(this IServiceCollection services) =>
        services.AddSingleton<ITokenAcquirerService>(p => new TokenAcquirerService(
            p.GetRequiredService<IAuthorizationRepository>(),
            p.GetRequiredService<IOptions<AuthorizationClientOptions>>().Value.AppRedirectUri
        ));

    private static IServiceCollection AddWebAPI(this IServiceCollection services)
    {
        return services
            .AddSingleton<IWebAPIClient>(p => new WebAPIClient(HttpClient(p)))
            .AddSingleton<IWebAPIService, WebAPIService>();
    }

    /// <summary>
    /// Registers the Web API services, sign-in and HTTP clients. The Spotify app's settings are read
    /// from the <see cref="AuthorizationClientOptions.SectionName"/> configuration section.
    /// </summary>
    public static IServiceCollection AddSpotifyNet(this IServiceCollection services)
    {
        // A client of its own, so the app's other clients keep their settings. The singletons
        // keep it for good, so recycle its connections rather than its handler, to pick up DNS
        // changes.
        services
            .AddHttpClient(HttpClientName)
            .UseSocketsHttpHandler(
                (handler, _) => handler.PooledConnectionLifetime = TimeSpan.FromMinutes(2)
            )
            .SetHandlerLifetime(Timeout.InfiniteTimeSpan);

        return services.AddAuthorization().AddTokenAcquirer().AddWebAPI();
    }

    private static HttpClient HttpClient(IServiceProvider provider) =>
        provider.GetRequiredService<IHttpClientFactory>().CreateClient(HttpClientName);
}
