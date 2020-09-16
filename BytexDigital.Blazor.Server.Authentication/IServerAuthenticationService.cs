using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace BytexDigital.Blazor.Server.Authentication
{
    public interface IServerAuthenticationService
    {
        bool IsSignedIn();

        Task SignInAsAsync(string userId, bool setRememberCookie = true, CancellationToken cancellationToken = default);

        Task SignOutAsync(CancellationToken cancellationToken = default);

        Task InitializeFromCookiesAsync(CancellationToken cancellationToken = default);

        string GetSignedInIdOrDefault();

        ClaimsPrincipal GetSignedInPrincipalOrDefault();

    }
}
