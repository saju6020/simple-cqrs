namespace Platform.Infrastructure.Core.Validation
{
    /// <summary>Validation Error Model.</summary>
    public class ValidationError
    {
        public ValidationError()
        {
        }

        public ValidationError(string errorCode, string errorMessage)
        {
            this.ErrorMessage = errorMessage;
            this.ErrorCode = errorCode;
        }

        public string ErrorCode { get; set; }

        public string ErrorMessage { get; set; }

        public string PropertyName { get; set; }

        public string ResourceName { get; set; }
    }
}
