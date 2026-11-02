using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SpotifyNet.Services.Abstractions;

namespace SpotifyNet.Services;

public sealed class AuthorizationService(IAuthorizationRepository authorizationRepository)
    : IAuthorizationService
{
    public async Task<string> GetAccessToken(
        IEnumerable<string> scopes,
        CancellationToken cancellationToken
    )
    {
        ArgumentNullException.ThrowIfNull(scopes);

        var token = await authorizationRepository.GetAccessToken(cancellationToken);

        var missingScopes = scopes.Except(token.AuthorizationScopes).ToList();

        if (missingScopes.Count != 0)
        {
            throw new AuthorizationException(
                $"The current access token does not have the required authorization scopes `{string.Join(", ", missingScopes)}`."
            );
        }

        return token.Token;
    }
}
