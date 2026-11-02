using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using SpotifyNet.Datastructures.Spotify.Authorization;

namespace SpotifyNet.Tests.Datastructures;

public class AuthorizationScopeTests
{
    [Test]
    public async Task ValidScopesListsEveryScopeConstant()
    {
        var constants = typeof(AuthorizationScope)
            .GetFields(BindingFlags.Public | BindingFlags.Static)
            .Where(field => field.IsLiteral)
            .Select(field => (string)field.GetRawConstantValue()!);

        await Assert.That(constants.Except(AuthorizationScope.ValidScopes)).IsEmpty();
    }
}
