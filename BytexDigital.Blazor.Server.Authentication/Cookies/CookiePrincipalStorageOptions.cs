using Microsoft.AspNetCore.Http;

using System;

namespace BytexDigital.Blazor.Server.Authentication.Cookies
{
    public class CookiePrincipalStorageOptions
    {
        public string CookieName { get; set; } = ".AspNet.User";
        public string CookieDomain { get; set; }
        public bool CookieHttpOnly { get; set; } = false;
        public bool CookieIsEssential { get; set; } = true;
        public TimeSpan? CookieMaxAge { get; set; } = TimeSpan.FromSeconds(int.MaxValue);
        public DateTime? CookieExpires { get; set; } = DateTime.UtcNow.AddYears(100);
        public string CookiePath { get; set; } = "/";
        public SameSiteMode CookieSameSite { get; set; } = SameSiteMode.Strict;
        public bool CookieSecure { get; set; } = true;
    }
}
