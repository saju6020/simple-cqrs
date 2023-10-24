namespace Platform.Infrastructure.Core.Dependencies
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Extensions.DependencyInjection;
    using Platform.Infrastructure.Core.Events;    

    /// <summary>Generic resolver to resolve passed template.</summary>
    /// <seealso cref="Platform.Infrastructure.Core.Dependencies.IResolver" />
    public class Resolver : IResolver
    {
        private readonly IServiceProvider serviceProvider;

        public Resolver(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public T Resolve<T>()
        {
            return this.serviceProvider.GetService<T>();
        }

        public IEnumerable<T> ResolveAll<T>()
        {
            try
            {
                var services = this.serviceProvider.GetServices(typeof(IEventHandlerAsync<>));
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return this.serviceProvider.GetServices<T>();
        }

        public object Resolve(Type type)
        {
            return this.serviceProvider.GetService(type);
        }

        public IEnumerable<object> ResolveAll(Type type)
        {
            return this.serviceProvider.GetServices(type);
        }
    }
}
