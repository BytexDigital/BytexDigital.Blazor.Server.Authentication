using Microsoft.AspNetCore.Identity;

using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace BytexDigital.Blazor.Server.Authentication
{
    public class IdentityPrincipalProvider<TUser> : IPrincipalProvider where TUser : class
    {
        private readonly UserManager<TUser> _userManager;

        public IdentityPrincipalProvider(UserManager<TUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<ClaimsPrincipal> CreateClaimsPrincipalAsync(string userId, CancellationToken cancellationToken = default)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var username = await _userManager.GetUserNameAsync(user);
            var id = await _userManager.GetUserIdAsync(user);
            var roles = await _userManager.GetRolesAsync(user);
            var claims = await _userManager.GetClaimsAsync(user);

            var identity = new ClaimsIdentity("Cookies", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, id));
            identity.AddClaim(new Claim(ClaimTypes.Name, username));
            identity.AddClaim(new Claim("http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider", "ASP.NET Identity", ClaimValueTypes.String));

            foreach (var role in roles)
            {
                identity.AddClaim(new Claim(ClaimsIdentity.DefaultRoleClaimType, role, ClaimValueTypes.String));
            }

            foreach (var claim in claims)
            {
                identity.AddClaim(claim);
            }

            return new ClaimsPrincipal(identity);
        }
    }
}
