using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using SpotifyNet.Clients.Abstractions;
using SpotifyNet.Services;
using SpotifyNet.Services.Abstractions;

namespace SpotifyNet.Tests.Services;

public class TokenAcquirerServiceTests
{
    [Test]
    public async Task AsksForGrantedScopesWithNewOnes()
    {
        var repository = new SignInRecorder(
            ["user-read-private"],
            refreshed: ["user-read-private"]
        );

        await SignIn(repository, ["user-top-read"]);

        await Assert
            .That(repository.SignInScopes!)
            .IsEquivalentTo(["user-top-read", "user-read-private"]);
    }

    [Test]
    public async Task KeepsGrantedScopesWhenTheTokenCantBeRefreshed()
    {
        var repository = new SignInRecorder(
            ["user-read-private", "user-top-read"],
            refreshed: null
        );

        await SignIn(repository, ["user-read-private"]);

        await Assert
            .That(repository.SignInScopes!)
            .IsEquivalentTo(["user-read-private", "user-top-read"]);
    }

    [Test]
    public async Task ChecksScopesAfterRefreshing()
    {
        var repository = new SignInRecorder(
            ["user-read-private", "user-top-read"],
            refreshed: ["user-read-private"]
        );

        await SignIn(repository, ["user-top-read"]);

        await Assert
            .That(repository.SignInScopes!)
            .IsEquivalentTo(["user-top-read", "user-read-private"]);
    }

    private static async Task SignIn(IAuthorizationRepository repository, string[] scopes)
    {
        ITokenAcquirerService acquirer = new TokenAcquirerService(
            repository,
            "http://127.0.0.1:5000/callback"
        );

        await Assert.That(() => acquirer.EnsureTokenExists(scopes)).Throws<SignInStarted>();
    }

    private static AccessTokenMetadata Token(ImmutableArray<string> scopes) =>
        new()
        {
            AuthorizationScopes = scopes,
            Token = "token",
            ExpiresAtUtc = DateTimeOffset.MaxValue,
            RefreshToken = "refresh",
        };

    private sealed class SignInStarted : Exception;

    /// <summary>
    /// Stores a token with <paramref name="granted"/> scopes, which refreshes to
    /// <paramref name="refreshed"/> ones (null: Spotify refuses). Records the scopes a sign-in asks
    /// for, then stops it before the browser opens.
    /// </summary>
    private sealed class SignInRecorder(
        ImmutableArray<string> granted,
        ImmutableArray<string>? refreshed
    ) : IAuthorizationRepository
    {
        public IReadOnlyCollection<string>? SignInScopes { get; private set; }

        public Task<string> GetUserAuthorizeUri(
            IEnumerable<string> scopes,
            CancellationToken cancellationToken
        )
        {
            SignInScopes = [.. scopes];

            throw new SignInStarted();
        }

        public Task<AccessTokenMetadata> GetNewAccessToken(
            string code,
            string state,
            CancellationToken cancellationToken
        ) => throw new NotSupportedException();

        public Task<AccessTokenMetadata> GetAccessToken(CancellationToken cancellationToken) =>
            refreshed is { } scopes
                ? Task.FromResult(Token(scopes))
                : throw new WebAPIException(
                    "https://accounts.spotify.com/api/token",
                    HttpStatusCode.BadRequest,
                    "Refresh token revoked",
                    "{}"
                );

        public Task<AccessTokenMetadata?> GetStoredAccessToken(
            CancellationToken cancellationToken
        ) => Task.FromResult<AccessTokenMetadata?>(Token(granted));
    }
}
