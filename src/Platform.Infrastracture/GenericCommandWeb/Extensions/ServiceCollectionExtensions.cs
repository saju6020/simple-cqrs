using Platform.Infrastructure.Authentication;
using Platform.Infrastructure.Common.Security;
using Platform.Infrastructure.Core.Accessors;

namespace GenericCommandWeb.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            services.AddScoped<UserContext, UserContext>();
            services.AddSingleton<IUserContextProvider, HttpUserContextProvider>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            return services;
           
        }
    }
}
