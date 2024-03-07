namespace Platform.Infrastructure.Core.Dependencies
{
    using System;

    /// <summary>HandlerResolver interface.</summary>
    public interface IHandlerResolver
    {
        THandler ResolveHandler<THandler>();

        object ResolveHandler(Type handlerType);

        object ResolveHandler(object param, Type type);

        object ResolveQueryHandler(object query, Type type);
    }
}
