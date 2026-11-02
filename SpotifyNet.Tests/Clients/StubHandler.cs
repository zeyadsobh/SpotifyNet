using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SpotifyNet.Tests.Clients;

/// <summary>Replies with the given responses in order and records each request's body and token.</summary>
internal sealed class StubHandler(params HttpResponseMessage[] responses) : HttpMessageHandler
{
    private readonly Queue<HttpResponseMessage> _responses = new(responses);

    public List<string?> Bodies { get; } = [];

    public List<string?> Tokens { get; } = [];

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken
    )
    {
        Tokens.Add(request.Headers.Authorization?.Parameter);
        Bodies.Add(
            request.Content is null
                ? null
                : await request.Content.ReadAsStringAsync(cancellationToken)
        );

        return _responses.Dequeue();
    }
}
