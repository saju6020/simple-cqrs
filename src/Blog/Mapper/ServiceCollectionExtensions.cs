using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using SimpleCQRS.Blog.Mapper;

namespace Blog.Mapper
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMapperProfiles(this IServiceCollection services)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<CreatePostCommandToDbModelProfile>();    
                cfg.AddProfile<BlogDtoToCreatePostCommandProfile>();
            });

            IMapper mapper = config.CreateMapper();
            services.AddSingleton(mapper);

            return services;
        }
    }
}
