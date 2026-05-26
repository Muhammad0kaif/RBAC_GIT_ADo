using System.Net;
using System.Text.Json;

namespace AdoApi2.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IWebHostEnvironment _env;

        public ExceptionMiddleware(
            RequestDelegate next,
            ILogger<ExceptionMiddleware> logger,
            IWebHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception occurred");

                context.Response.ContentType = "application/json";
                context.Response.StatusCode =
                    (int)HttpStatusCode.InternalServerError;

                var response = new
                {
                    statusCode = context.Response.StatusCode,

                    message = _env.IsDevelopment()
                        ? ex.Message
                        : "Something went wrong. Please try again later.",

                    details = _env.IsDevelopment()
                        ? ex.StackTrace
                        : null
                };

                await context.Response.WriteAsync(
                    JsonSerializer.Serialize(response));
            }
        }
    }
}