using Domain.Exceptions;

namespace EduPlatform.WebApi.Middleware
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                context.Response.ContentType = "application/json";

                var response = new ErrorResponse
                {
                    Message = ex.Message
                };

                response.StatusCode = ex switch
                {
                    NotFoundException => StatusCodes.Status404NotFound,
                    BadRequestException => StatusCodes.Status400BadRequest,
                    UnauthorizedException => StatusCodes.Status401Unauthorized,

                    _ => StatusCodes.Status500InternalServerError
                };

                if (response.StatusCode == StatusCodes.Status500InternalServerError)
                {
                    response.Message = "An unexpected error occurred. Please try again later.";
                }

                context.Response.StatusCode = response.StatusCode;
                await context.Response.WriteAsJsonAsync(response);
            }
        }
    }
}
