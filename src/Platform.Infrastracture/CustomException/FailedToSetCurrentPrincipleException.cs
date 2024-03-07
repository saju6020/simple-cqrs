namespace Platform.Infrastructure.CustomException
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>Exception for empty file.</summary>
    /// <seealso cref="System.Exception" />
    public class FailedToSetCurrentPrincipleException : BaseException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FailedToSetCurrentPrincipleException"/> class.
        /// </summary>
        /// <param name="message">Exception message.</param>
        public FailedToSetCurrentPrincipleException(string message)
            : base(message)
        {
        }
    }
}
