namespace Platform.Infrastructure.CustomResponse
{
    using System.Linq;
    using System.Net;
    using System.Reflection;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Platform.Infrastructure.Core.Commands;
    using Platform.Infrastructure.Core.Validation;

    /// <summary>
    /// Custom attribute for overriding fluent validation response.
    /// </summary>
    public class FluentResponseOverRideAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// Checks if the current request is Get request.
        /// </summary>
        /// <param name="httpContext">HttpContext.</param>
        /// <returns>returns boolean.</returns>
        private bool IsQueryRequest(HttpContext httpContext)
        {
            return httpContext.Request.Method == HttpMethods.Get;
        }

        /// <summary>
        /// Checks if status code is OK.
        /// </summary>
        /// <param name="objectResult">ObjectResult.</param>
        /// <returns>returns boolean,</returns>
        private bool IsOkRequest(ObjectResult objectResult)
        {
            return objectResult.StatusCode == (int)HttpStatusCode.OK;
        }

        private bool IsBadRequest(ObjectResult objectResult)
        {
            return objectResult.StatusCode == (int)HttpStatusCode.BadRequest;
        }

        /// <summary>
        /// Extracts error messages and codes from ValidationResponse.
        /// </summary>
        /// <param name="validationResponse">ValidationResponse.</param>
        /// <returns>Returns ExternalResponse.</returns>
        private ExternalResponse GetErrorResponse(ValidationResponse validationResponse)
        {
            ExternalResponse response = new ExternalResponse();

            validationResponse.Errors.All(error =>
            {
                response.Errors.Add(new Error()
                {
                    ErrorMessage = error.ErrorMessage,
                    ErrorCode = error.ErrorCode,
                    PropertyName = error.PropertyName,
                });
                return true;
            });

            return response;
        }

        /// <summary>
        /// Returns actionResult as OkObjectResult or BadRequestObjectResult.
        /// </summary>
        /// <param name="actionResult">IActionResult.</param>
        /// <returns>ObjectResult.</returns>
        private ObjectResult GetObjectResult(IActionResult actionResult)
        {
            if (actionResult == null)
            {
                return null;
            }
            
            ObjectResult objectResult = actionResult as ObjectResult;

            if (objectResult == null || objectResult.Value == null)
            {
                PropertyInfo statusCodePropertyInfo = actionResult.GetType().GetProperty("StatusCode");
                PropertyInfo valuePropertyInfo = actionResult.GetType().GetProperty("Value");

                object statusCode = statusCodePropertyInfo?.GetValue(actionResult);
                object value = valuePropertyInfo?.GetValue(actionResult);
                objectResult = new ObjectResult(value)
                {                    
                    StatusCode = statusCode != null?(int)statusCode : StatusCodes.Status200OK,
                };
            }

            return objectResult;
        }

        /// <summary>
        /// Extracts ValidationResponse or CommandResponse from ObjectResult based on request type(Get/Post).
        /// </summary>
        /// <param name="objectResult">ObjectResult.</param>
        /// <param name="httpContext">HttpContext.</param>
        /// <returns>ValidationResponse.</returns>
        private ValidationResponse ExtractValidationResponse(ObjectResult objectResult, HttpContext httpContext)
        {
            if (objectResult == null || httpContext == null)
            {
                return null;
            }

            ValidationResponse validationResponse = null;
            CommandResponse commandResponse = objectResult.Value as CommandResponse;
            if (commandResponse == null)
            {
                validationResponse = objectResult.Value as ValidationResponse;
            }
            else
            {
                validationResponse = commandResponse.ValidationResult;
            }

            return validationResponse;
        }

        /// <summary>
        /// OverRiding OnResultExecuting method of ActionFilterAttribute.
        /// </summary>
        /// <param name="context">ResultExecutingContext.</param>
        public override void OnResultExecuting(ResultExecutingContext context)
        {
            if(context.Result is FileStreamResult)
            {
                return;
            }

            ExternalResponse externalResponse = new ExternalResponse();
            HttpContext httpContext = context.HttpContext;

            ObjectResult objectResult = this.GetObjectResult(context.Result);
            if (this.IsBadRequest(objectResult))
            {
                ValidationResponse validationResponse = this.ExtractValidationResponse(objectResult, httpContext);

                if (validationResponse == null)
                {
                    return;
                }

                externalResponse = this.GetErrorResponse(validationResponse);

                context.Result = new JsonResult(externalResponse)
                {
                    StatusCode = objectResult.StatusCode,
                };

                return;
            }

            CommandResponse commandResponse = objectResult.Value as CommandResponse;

            externalResponse.Result = commandResponse == null ? objectResult.Value : commandResponse.Result;

            context.Result = new JsonResult(externalResponse)
            {
                StatusCode = objectResult.StatusCode,
            };
        }
    }
}
