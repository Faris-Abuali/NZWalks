using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace NZWalks.API.Middlewares
{
    public class ExceptionHandlerMiddleware
    {
        private readonly ILogger<ExceptionHandlerMiddleware> logger;
        private readonly RequestDelegate next;

        /**
          * RequestDelegate is a function that can process http request.
          * It returns a task that represents the completion of request processing.
          **/

        public ExceptionHandlerMiddleware(ILogger<ExceptionHandlerMiddleware> logger,
            RequestDelegate next)
        {
            this.logger = logger;
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await next(httpContext);
            }
            catch (Exception ex)
            {
                var errorId = Guid.NewGuid();

                // Log this Exception
                logger.LogError(ex, $"{errorId}: {ex.Message}");

                // Return a Custom Error Response
                httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                httpContext.Response.ContentType = "application/json";

                var error = new
                {
                    Id = errorId,
                    ErrorMessage = "Something went wrong",
                };

                await httpContext.Response.WriteAsJsonAsync(error); // Writes the error as JSON to the response body
            }
        }
    }
}
