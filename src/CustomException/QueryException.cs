namespace Platform.Infrastructure.CustomException
{
    using System;

    /// <summary>Query Exception class.</summary>
    /// <seealso cref="Shohoz.Platform.Infrastructure.CustomException.BaseException" />
    public class QueryException : BaseException
    {
        public QueryException(string errorMessage)
            : base(errorMessage)
        {
        }
    }
}
