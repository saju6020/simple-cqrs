namespace Platform.Infrastructure.Host.WebApi.Middlewares
{
    using System.Net;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;

    internal class TenantIdHeaderMiddleware
    {
        private const string TenantIdHeaderName = "tenant-id";

        private static readonly string VerticalIdHeaderMissingExceptionMessage = $"{TenantIdHeaderName} header is mandatory";

        private readonly RequestDelegate next;

        public TenantIdHeaderMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public Task InvokeAsync(HttpContext context)
        {
            var verticalIdHeader = context.Request.Headers[TenantIdHeaderName];

            if (verticalIdHeader.Count == 0)
            {
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return context.Response.WriteAsync(VerticalIdHeaderMissingExceptionMessage);
            }

            return this.next(context);
        }
    }
}
