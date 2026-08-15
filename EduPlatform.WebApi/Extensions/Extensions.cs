using Domain.Contracts;
using Domain.Models.Identity;
using EduPlatform.WebApi.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Persistence;
using Persistence.Data;
using Scalar.AspNetCore;
using Services;
using System.Text;

namespace EduPlatform.WebApi.Extensions
{
    public static class Extensions
    {
        public static IServiceCollection RegisterAllServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddIdentityServices(configuration);
            services.AddBuiltInServices();
            services.AddOpenApiServices();
            services.AddInfrastructureServices(configuration);
            services.AddApplicationServices(configuration);
            services.AddMvcConfiguration();

            return services;
        }
        public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddIdentity<ApplicationUser, IdentityRole>()
                    .AddEntityFrameworkStores<ApplicationDbContext>()
                    .AddDefaultTokenProviders();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = configuration["Jwt:Issuer"],

                    ValidateAudience = true,
                    ValidAudience = configuration["Jwt:Audience"],

                    ValidateLifetime = true,

                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(configuration["Jwt:SecretKey"] ?? configuration["Jwt:Key"]!))
                };
            });

            return services;
        }
        private static IServiceCollection AddBuiltInServices(
            this IServiceCollection services)
        {
            services.AddControllers();

            return services;
        }

        private static IServiceCollection AddOpenApiServices(
            this IServiceCollection services)
        {
            services.AddOpenApi(options =>
            {
                options.AddDocumentTransformer<BearerSecuritySchemeTransformer>();
            });

            return services;
        }

        private static IServiceCollection AddMvcConfiguration(
            this IServiceCollection services)
        {
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var errors = context.ModelState
                        .Where(x => x.Value?.Errors.Count > 0)
                        .SelectMany(x => x.Value!.Errors)
                        .Select(x => x.ErrorMessage)
                        .ToList();

                    var response = new ErrorResponse
                    {
                        StatusCode = StatusCodes.Status400BadRequest,
                        Message = string.Join(" | ", errors)
                    };

                    return new BadRequestObjectResult(response);
                };

                return;
            });

            return services;
        }

        public static async Task<WebApplication> ConfigureMiddlewaresAsync(this WebApplication app)
        {
            //Data Seeding / Migrations Scope
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var initializer = services.GetRequiredService<IDbInitializer>();
                    await initializer.InitializeAsync();
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while seeding or migrating the database.");
                }
            }

            // OpenAPI & Scalar UI (Development Only)
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.MapGet("/", () => Results.Redirect("/scalar/v1"));
                app.MapScalarApiReference();
            }

            // Middlewares Pipeline
            app.UseHttpsRedirection();
            app.UseMiddleware<GlobalExceptionMiddleware>();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            return app;
        }
    }
}
