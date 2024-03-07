namespace Platform.Infrastructure.Core.Dependencies
{
    using System;
    using System.Collections.Generic;

    /// <summary>Resolver Interface.</summary>
    public interface IResolver
    {
        T Resolve<T>();

        IEnumerable<T> ResolveAll<T>();

        object Resolve(Type type);

        IEnumerable<object> ResolveAll(Type type);
    }
}
