namespace Platform.Infrastructure.Host.WebApi.Middlewares
{
    using System.Net;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;

    internal class ServiceIdHeaderMiddleware
    {
        private const string ServiceIdHeaderName = "x-service-id";

        private static readonly string ServiceIdHeaderMissingExceptionMessage = $"{ServiceIdHeaderName} header is mandatory";

        private readonly RequestDelegate next;

        public ServiceIdHeaderMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public Task InvokeAsync(HttpContext context)
        {
            var serviceIdHeader = context.Request.Headers[ServiceIdHeaderName];

            if (serviceIdHeader.Count == 0)
            {
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return context.Response.WriteAsync(ServiceIdHeaderMissingExceptionMessage);
            }

            return this.next(context);
        }
    }
}
