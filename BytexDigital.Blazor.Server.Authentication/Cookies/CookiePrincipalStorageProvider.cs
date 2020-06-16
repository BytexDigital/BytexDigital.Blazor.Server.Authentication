using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.JSInterop;

using System;
using System.Security.Claims;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace BytexDigital.Blazor.Server.Authentication.Cookies
{
    public class CookiePrincipalStorageProvider : IPrincipalStorageProvider
    {
        private readonly IDataProtectionProvider _dataProtectionProvider;
        private readonly IDataProtector _dataProtector;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IJSRuntime _jsRuntime;
        private readonly CookiePrincipalStorageOptions _options;
        private readonly TicketSerializer _ticketSerializer;
        private ClaimsPrincipal _cachedClaimsPrincipal = default;
        private bool _ignoreCookieValue = false;

        private const string _jsSetMethod = "BytexDigitalAuthCookies.setUserIdentifier";
        private const string _jsRemoveMethod = "BytexDigitalAuthCookies.removeUserIdentifier";

        public CookiePrincipalStorageProvider(
            IDataProtectionProvider dataProtectionProvider,
            IHttpContextAccessor httpContextAccessor,
            IJSRuntime jsRuntime,
            CookiePrincipalStorageOptions options)
        {
            _dataProtectionProvider = dataProtectionProvider ?? throw new ArgumentNullException(nameof(dataProtectionProvider), "A data protection provider must be available.");
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor), "A IHttpContextAccessor must be available.");
            _jsRuntime = jsRuntime;
            _options = options;

            _dataProtector = _dataProtectionProvider.CreateProtector("Blazor.Authentication.Cookie");

            _ticketSerializer = new TicketSerializer();
        }

        public ClaimsPrincipal GetClaimsPrincipalOrDefault()
        {
            if (_cachedClaimsPrincipal != null && !_cachedClaimsPrincipal.Equals(default))
            {
                return _cachedClaimsPrincipal;
            }

            if (!_ignoreCookieValue)
            {
                if (_httpContextAccessor.HttpContext.Request.Cookies.TryGetValue(_options.CookieName, out string userJson))
                {
                    try
                    {
                        var principalData = JsonSerializer.Deserialize<byte[]>(_dataProtector.Unprotect(userJson));
                        var ticket = _ticketSerializer.Deserialize(principalData);

                        return ticket.Principal;
                    }
                    catch
                    {
                    }
                }
            }

            return default;
        }

        public async Task SetClaimsPrincipalAsync(ClaimsPrincipal claimsPrincipal, bool setCookie = true, CancellationToken cancellationToken = default)
        {
            if (setCookie)
            {
                try
                {
                    byte[] data = _ticketSerializer.Serialize(new AuthenticationTicket(claimsPrincipal, "Cookie"));

                    string protectedUserJson = _dataProtector.Protect(JsonSerializer.Serialize(data));

                    // Set cookie and cached user identifier
                    await _jsRuntime.InvokeVoidAsync(_jsSetMethod, cancellationToken, CreateCookieString(protectedUserJson));

                    _cachedClaimsPrincipal = claimsPrincipal;
                }
                catch
                {

                }
            }

            _cachedClaimsPrincipal = claimsPrincipal;
        }

        public async Task ClearClaimsPrinipalAsync(bool clearCookie = true, CancellationToken cancellationToken = default)
        {
            if (clearCookie)
            {
                try
                {
                    await _jsRuntime.InvokeVoidAsync(_jsRemoveMethod, cancellationToken, _options.CookieName);
                }
                catch
                {

                }
            }

            _cachedClaimsPrincipal = default;
            _ignoreCookieValue = true;
        }

        protected virtual string CreateCookieString(string value)
        {
            string cookieString = $"{_options.CookieName}={value}";
            cookieString += $"; samesite={_options.CookieSameSite}";

            if (_options.CookieMaxAge != default) cookieString += $"; max-age={(int)_options.CookieMaxAge.Value.TotalSeconds}";
            if (!string.IsNullOrEmpty(_options.CookieDomain)) cookieString += $"; domain={_options.CookieDomain}";
            if (!string.IsNullOrEmpty(_options.CookiePath)) cookieString += $"; path={_options.CookiePath}";
            if (_options.CookieHttpOnly) cookieString += "; HttpOnly";
            if (_options.CookieSecure) cookieString += "; Secure";
            if (_options.CookieExpires != default) cookieString += $"; expires={_options.CookieExpires.Value:r}";

            return cookieString;
        }
    }
}
