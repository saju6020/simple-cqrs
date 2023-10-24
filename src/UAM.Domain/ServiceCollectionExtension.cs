using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Platform.Infrastructure.Core.Commands;
using SimpleCQRS.UAM.Domain.CommandHandlers;
using SimpleCQRS.UAM.Domain.Commands;
using SimpleCQRS.UAM.Domain.Validators;

namespace SimpleCQRS.UAM.Domain.Extension
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDomainComponents(this IServiceCollection services)
        {
            services.AddScoped<ICommandHandlerAsync<CreateUserCommand>, CreateUserCommandHandler>();
            services.AddScoped<IValidator<CreateUserCommand>, CreateUserCommandValidator>();
            return services;
        }
    }
}
