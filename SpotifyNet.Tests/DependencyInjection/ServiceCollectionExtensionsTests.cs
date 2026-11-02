using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SpotifyNet.DependencyInjection;
using SpotifyNet.Services.Abstractions;
using SpotifyNet.Services.Abstractions.WebAPI;

namespace SpotifyNet.Tests.DependencyInjection;

public class ServiceCollectionExtensionsTests
{
    [Test]
    public async Task ResolvesTheServicesFromConfiguration()
    {
        IConfiguration configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(
                new Dictionary<string, string?>
                {
                    ["Spotify:AuthorizationClient:AppClientId"] = "id",
                    ["Spotify:AuthorizationClient:AppRedirectUri"] =
                        "http://localhost:5000/callback",
                }
            )
            .Build();
        await using var provider = new ServiceCollection()
            .AddSingleton(configuration)
            .AddSpotifyNet()
            .BuildServiceProvider();

        await Assert.That(provider.GetRequiredService<IWebAPIService>()).IsNotNull();
        await Assert.That(provider.GetRequiredService<ITokenAcquirerService>()).IsNotNull();
    }
}
