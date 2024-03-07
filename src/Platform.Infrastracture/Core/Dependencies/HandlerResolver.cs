namespace Platform.Infrastructure.Core.Dependencies
{
    using System;
    using System.Linq;
    
    /// <summary>HandlerResolver class.</summary>
    /// <seealso cref="Platform.Infrastructure.Core.Dependencies.IHandlerResolver" />
    public class HandlerResolver : IHandlerResolver
    {
        private readonly IResolver resolver;

        public HandlerResolver(IResolver resolver)
        {
            this.resolver = resolver;
        }

        public THandler ResolveHandler<THandler>()
        {
            var handler = this.resolver.Resolve<THandler>();

            if (handler == null)
            {
                throw new Exception($"No handler found that implements '{typeof(THandler).FullName}'");
            }

            return handler;
        }

        public object ResolveHandler(Type handlerType)
        {
            var handler = this.resolver.Resolve(handlerType);

            return handler;
        }

        public object ResolveHandler(object param, Type type)
        {
            var paramType = param.GetType();
            var handlerType = type.MakeGenericType(paramType);
            return this.ResolveHandler(handlerType);
        }

        public object ResolveQueryHandler(object query, Type type)
        {
            var queryType = query.GetType();
            var queryInterface = queryType.GetInterfaces()[0];
            var resultType = queryInterface.GetGenericArguments().FirstOrDefault();
            var handlerType = type.MakeGenericType(queryType, resultType);
            return this.ResolveHandler(handlerType);
        }
    }
}