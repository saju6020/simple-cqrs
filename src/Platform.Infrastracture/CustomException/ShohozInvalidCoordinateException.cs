namespace Platform.Infrastructure.CustomException
{
    using System;

    /// <summary>
    /// Exception for Invalid Geo Coordinates.
    /// <seealso cref="System.Exception" />
    /// </summary>
    public class InvalidCoordinateException : BaseException
    {
        public InvalidCoordinateException(string message)
            : base(message)
        {
        }

        public InvalidCoordinateException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
