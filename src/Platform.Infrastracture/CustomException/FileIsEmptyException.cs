namespace Platform.Infrastructure.CustomException
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>Exception for empty file.</summary>
    /// <seealso cref="System.Exception" />
    public class FileIsEmptyException : BaseException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FileIsEmptyException"/> class.
        /// </summary>
        /// <param name="message">Exception message.</param>
        public FileIsEmptyException(string message)
            : base(message)
        {
        }
    }
}
