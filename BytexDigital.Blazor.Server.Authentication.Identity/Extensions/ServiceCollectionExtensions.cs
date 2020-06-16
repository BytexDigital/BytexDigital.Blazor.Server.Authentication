
using BytexDigital.Blazor.Server.Authentication;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddIdentityPrincipalProvider<TUser>(this IServiceCollection services) where TUser : class
        {
            services.AddPrincipalProvider<IdentityPrincipalProvider<TUser>>();

            return services;
        }
    }
}
