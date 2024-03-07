namespace Platform.Infrastructure.Core.Queries
{
    using Platform.Infrastructure.Core.Validation;

    /// <summary>Command response model.</summary>
    public class QueryResponse<TResult>
    {
        public QueryResponse()
        {
        }

        public QueryResponse(ValidationResponse validationResult, TResult result)
        {
            this.ValidationResult = validationResult;
            this.Result = result;
        }

        public ValidationResponse ValidationResult { get; set; }

        public TResult Result { get; set; }
    }
}
