using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using SpotifyNet.Datastructures.Spotify.Search;
using SpotifyNet.Datastructures.Spotify.Tracks;
using SpotifyNet.Services.Abstractions.WebAPI;
using SpotifyNet.Services.WebAPI;
using YamlDotNet.Serialization;
using Node = System.Collections.Generic.Dictionary<object, object>;

namespace SpotifyNet.Tests.Services;

/// <summary>
/// Checks each IWebAPIService method asks for exactly the scopes its operation's <c>security</c>
/// lists in open-api-schema.yaml.
/// </summary>
public class ScopeTests
{
    private static readonly Node Paths = (Node)
        new DeserializerBuilder()
            .Build()
            .Deserialize<Node>(
                File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "open-api-schema.yaml"))
            )["paths"];

    public static IEnumerable<string> Methods() =>
        typeof(IWebAPIService)
            .GetProperties()
            .SelectMany(area =>
                area.PropertyType.GetMethods().Select(m => $"{area.Name}.{m.Name}")
            );

    [Test]
    [MethodDataSource(nameof(Methods))]
    public async Task RequestsTheSpecScopes(string name)
    {
        var client = new FakeWebAPIClient();
        var authorization = new StubAuthorizationService();
        var (area, method) = Method(name);

        var result = method.Invoke(
            area.GetValue(new WebAPIService(authorization, client)),
            [.. method.GetParameters().Select(Argument)]
        );
        try
        {
            await (
                result switch
                {
                    Task task => task,
                    IAsyncEnumerable<object> items => items.FirstOrDefaultAsync().AsTask(),
                    _ => ((IAsyncEnumerable<bool>)result!).FirstOrDefaultAsync().AsTask(),
                }
            );
        }
        // No responses are set up, so calls that read one stop after their request.
        catch (KeyNotFoundException) { }

        var (verb, uri, _) = client.Requests.Single();
        await Assert
            .That(authorization.Requested.Single())
            .IsEquivalentTo(SpecScopes(verb, new Uri(uri).AbsolutePath));
    }

    private static (PropertyInfo Area, MethodInfo Method) Method(string name)
    {
        var parts = name.Split('.');
        var area = typeof(IWebAPIService).GetProperty(parts[0])!;
        var method = area.PropertyType.GetMethod(parts[1])!;

        // Both kinds of top items share an endpoint.
        return (
            area,
            method.IsGenericMethodDefinition ? method.MakeGenericMethod(typeof(Track)) : method
        );
    }

    // The parameter's default, or else a valid sample of its type.
    private static object? Argument(ParameterInfo parameter) =>
        parameter.HasDefaultValue ? parameter.DefaultValue
        : parameter.ParameterType == typeof(string) ? "a"
        : parameter.ParameterType == typeof(IEnumerable<string>) ? new[] { "a" }
        : parameter.ParameterType == typeof(IEnumerable<SearchType>) ? new[] { SearchType.track }
        : Activator.CreateInstance(parameter.ParameterType);

    // The scopes of the operation with this verb whose path template matches the request's path.
    private static string[] SpecScopes(string verb, string path)
    {
        var segments = path["/v1".Length..].Split('/');
        var operations = (Node)
            Paths.Single(p => Matches(((string)p.Key).Split('/'), segments)).Value;
        var security = (List<object>)((Node)operations[verb.ToLowerInvariant()])["security"];

        return [.. ((List<object>)((Node)security.Single())["oauth_2_0"]).Cast<string>()];
    }

    private static bool Matches(string[] template, string[] segments) =>
        template.Length == segments.Length
        && template.Zip(segments).All(p => p.First.StartsWith('{') || p.First == p.Second);
}
