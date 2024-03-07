namespace Platform.Infrastructure.CustomException
{
    using System;

    /// <summary>Handler Not Found Exception.</summary>
    /// <seealso cref="Platform.Infrastructure.CustomException.BaseException" />
    public class HandlerNotFoundException : BaseException
    {
        public HandlerNotFoundException(Type handlerType)
            : base(BuildErrorMessage(handlerType))
        {
        }

        private static string BuildErrorMessage(Type handlerType)
        {
            return $"No handler found that implements '{handlerType.FullName}'";
        }
    }
}