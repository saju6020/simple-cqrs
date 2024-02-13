namespace Platform.Infrastructure.CustomException
{
    /// <summary>Exception for empty file.</summary>
    /// <seealso cref="System.Exception" />
    public class FailedToBuildAppSettingsException : BaseException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FailedToBuildAppSettingsException"/> class.
        /// </summary>
        /// <param name="message">Exception message.</param>
        public FailedToBuildAppSettingsException(string message)
            : base(message)
        {
        }
    }
}
