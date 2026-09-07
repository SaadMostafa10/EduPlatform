using Domain.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

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
                    DbUpdateException dbEx when IsUniqueConstraintViolation(dbEx) => StatusCodes.Status400BadRequest,
                    _ => StatusCodes.Status500InternalServerError
                };

                if (ex is DbUpdateException dbUpdateEx && IsUniqueConstraintViolation(dbUpdateEx))
                {
                    response.Message = "A record with the same unique value already exists.";
                }
                else if (response.StatusCode == StatusCodes.Status500InternalServerError)
                {
                    response.Message = "An unexpected error occurred. Please try again later.";
                }

                context.Response.StatusCode = response.StatusCode;
                await context.Response.WriteAsJsonAsync(response);
            }
        }

        private static bool IsUniqueConstraintViolation(DbUpdateException ex)
        {
            if (ex.InnerException is SqlException sqlEx)
            {
                return sqlEx.Number == 2601 || sqlEx.Number == 2627;
            }

            return ex.InnerException?.Message.Contains("duplicate key", StringComparison.OrdinalIgnoreCase) == true
                || ex.InnerException?.Message.Contains("unique constraint", StringComparison.OrdinalIgnoreCase) == true;
        }
    }
}
