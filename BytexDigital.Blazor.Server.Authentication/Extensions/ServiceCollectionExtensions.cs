
using BytexDigital.Blazor.Server.Authentication;
using BytexDigital.Blazor.Server.Authentication.Cookies;

using Microsoft.AspNetCore.Components.Authorization;

using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCookiePrincipalStorage(this IServiceCollection services)
            => AddCookiePrincipalStorage(services, options => { });

        public static IServiceCollection AddCookiePrincipalStorage(this IServiceCollection services, Action<CookiePrincipalStorageOptions> configure)
        {
            var options = new CookiePrincipalStorageOptions();
            configure.Invoke(options);

            services.AddSingleton(options);

            AddPrincipalStorage<CookiePrincipalStorageProvider>(services);

            return services;
        }

        public static IServiceCollection AddAuthenticationService(this IServiceCollection services)
        {
            services
                .AddScoped<IServerAuthenticationService, ServerAuthenticationService>()
                .AddScoped<DelayedAuthenticationStateProvider>()
                .AddScoped(provider => provider.GetRequiredService<DelayedAuthenticationStateProvider>() as AuthenticationStateProvider);

            return services;
        }

        public static IServiceCollection AddPrincipalStorage<TProvider>(this IServiceCollection services) where TProvider : class, IPrincipalStorageProvider
        {
            services.AddScoped<IPrincipalStorageProvider, TProvider>();

            return services;
        }

        public static IServiceCollection AddPrincipalProvider<TProvider>(this IServiceCollection services) where TProvider : class, IPrincipalProvider
        {
            services.AddScoped<IPrincipalProvider, TProvider>();

            return services;
        }
    }
}
