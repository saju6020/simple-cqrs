using Platform.Infrastructure.Authentication;
using Platform.Infrastructure.Common.Security;
using Platform.Infrastructure.Core.Accessors;
using Platform.Infrastructure.Host.WebApi;

namespace ProductExample.CommandWebHost.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            services.AddScoped<UserContext, UserContext>();
            services.AddSingleton<IUserContextProvider, HttpUserContextProvider>();

            return services;
           
        }
    }
}
