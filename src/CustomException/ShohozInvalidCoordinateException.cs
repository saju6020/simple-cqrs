namespace Platform.Infrastructure.CustomException
{
    using System;

    /// <summary>
    /// Exception for Invalid Geo Coordinates.
    /// <seealso cref="System.Exception" />
    /// </summary>
    public class ShohozInvalidCoordinateException : BaseException
    {
        public ShohozInvalidCoordinateException(string message)
            : base(message)
        {
        }

        public ShohozInvalidCoordinateException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
