using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using SimpleCQRS.UAM.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCQRS.UAM.Mapper
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMapperProfiles(this IServiceCollection services)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<UserDetailsDtoToCreateUserCommandProfile>();
                cfg.AddProfile<CreateUserCommandToUserProfile>();
            });

            IMapper mapper = config.CreateMapper();
            services.AddSingleton(mapper);

            return services;
        }
    }
}
