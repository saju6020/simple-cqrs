namespace Platform.Infrastructure.CustomException
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// exception base. This class will identify is the exception is from handled exception scope or from system.
    /// If the exception type is Base that means the exception is from handled scope.
    /// </summary>
    [Serializable]
    public class BaseException : Exception
    {
        /// <summary>Initializes a new instance of the <see cref="BaseException"/> class.</summary>
        public BaseException()
        {
        }

        /// <summary>Initializes a new instance of the <see cref="BaseException"/> class.</summary>
        /// <param name="message">The message that describes the error.</param>
        public BaseException(string message)
            : base(message)
        {
        }

        /// <summary>Initializes a new instance of the <see cref="BaseException"/> class.</summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">
        /// The exception that is the cause of the current exception, or a null reference.
        /// </param>
        public BaseException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>Initializes a new instance of the <see cref="BaseException"/> class.</summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"/> that contains contextual information about the source or destination.</param>
        public BaseException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
