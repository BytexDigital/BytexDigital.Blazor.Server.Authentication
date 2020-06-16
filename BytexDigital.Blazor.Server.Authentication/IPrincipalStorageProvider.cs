using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace BytexDigital.Blazor.Server.Authentication
{
    public interface IPrincipalStorageProvider
    {
        Task ClearClaimsPrinipalAsync(bool clearCookie = true, CancellationToken cancellationToken = default);
        ClaimsPrincipal GetClaimsPrincipalOrDefault();
        Task SetClaimsPrincipalAsync(ClaimsPrincipal claimsPrincipal, bool setCookie = true, CancellationToken cancellationToken = default);
    }
}
