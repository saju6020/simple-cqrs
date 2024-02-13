namespace Platform.Infrastructure.CustomException
{
    /// <summary>Validation Exception Class.</summary>
    /// <seealso cref="Shohoz.Platform.Infrastructure.CustomException.BaseException" />
    public class ValidationException : BaseException
    {
        public ValidationException(string errorMessage)
            : base(errorMessage)
        {
        }
    }
}
