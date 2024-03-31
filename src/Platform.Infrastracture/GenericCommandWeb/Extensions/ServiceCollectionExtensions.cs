using Platform.Infrastructure.Authentication;
using Platform.Infrastructure.Common.Security;
using Platform.Infrastructure.Core;
using Platform.Infrastructure.Core.Accessors;
using Platform.Infrastructure.Repository.MongoDb;

namespace GenericCommandWeb.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            services.AddScoped<UserContext, UserContext>();
            services.AddSingleton<IUserContextProvider, HttpUserContextProvider>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IRepository, Repository>();

            return services;
           
        }
    }
}
