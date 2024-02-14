namespace Platform.Infrastructure.NoSql.Repository.Contracts.Models
{
    /// <summary>Command response model.</summary>
    public class ExecutedCommandResponse
    {
        /// <summary>Gets or sets the code.</summary>
        /// <value>The code.</value>
        public int Code { get; set; }

        /// <summary>Gets or sets a value indicating whether this instance is success.</summary>
        /// <value>
        /// <c>true</c> if this instance is success; otherwise, <c>false</c>.</value>
        public bool IsSuccess { get; set; }

        /// <summary>Gets or sets the message.</summary>
        /// <value>The message.</value>
        public string Message { get; set; }

        /// <summary>Gets or sets the error.</summary>
        /// <value>The error.</value>
        public string Error { get; set; }

        /// <summary>Gets or sets the command.</summary>
        /// <value>The command.</value>
        public string Command { get; set; }
    }
}
