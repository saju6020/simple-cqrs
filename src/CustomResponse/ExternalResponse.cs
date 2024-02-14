namespace Platform.Infrastructure.CustomResponse
{
    using System.Collections.Generic;

    /// <summary>
    /// External response class.
    /// </summary>
    public class ExternalResponse
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExternalResponse"/> class.
        /// </summary>
        public ExternalResponse()
        {
            this.Errors = new List<Error>();
        }

        public List<Error> Errors { get; set; }

        public object Result { get; set; }
    }
}
