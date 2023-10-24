namespace Platform.Infrastructure.Host.WebApi.Middlewares
{
    using System.Net;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;

    internal class VerticalIdHeaderMiddleware
    {
        private const string VerticalIdHeaderName = "vertical-id";

        private static readonly string VerticalIdHeaderMissingExceptionMessage = $"{VerticalIdHeaderName} header is mandatory";

        private readonly RequestDelegate next;

        public VerticalIdHeaderMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public Task InvokeAsync(HttpContext context)
        {
            var verticalIdHeader = context.Request.Headers[VerticalIdHeaderName];

            if (verticalIdHeader.Count == 0)
            {
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return context.Response.WriteAsync(VerticalIdHeaderMissingExceptionMessage);
            }

            return this.next(context);
        }
    }
}
