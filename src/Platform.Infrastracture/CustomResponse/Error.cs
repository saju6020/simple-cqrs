namespace Platform.Infrastructure.CustomResponse
{
    /// <summary>
    /// Custom error class.
    /// </summary>
    public class Error
    {
        public string ErrorMessage { get; set; }

        public string ErrorCode { get; set; }

        public string PropertyName { get; set; }
    }
}