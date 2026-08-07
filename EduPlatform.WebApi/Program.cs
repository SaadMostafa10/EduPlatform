
using Domain.Contracts;
using Domain.Models.Identity;
using EduPlatform.WebApi.Middleware;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Persistence;
using Persistence.Data;
using Scalar.AspNetCore;
using Services;
using Services.Abstractions;
using Services.Mapping;
using Shared.Options;

namespace EduPlatform.WebApi
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();

            builder.Services.Configure<ApiBehaviorOptions>(options =>
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
            });

            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                            .AddEntityFrameworkStores<ApplicationDbContext>()
                            .AddDefaultTokenProviders();

            builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("Jwt"));

            builder.Services.AddAutoMapper(cfg => { }, typeof(MappingProfile));

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<ITokenService, TokenService>();
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.Configure<EmailOptions>(builder.Configuration.GetSection("Email"));
            builder.Services.AddScoped<IEmailService, EmailService>();
            builder.Services.AddScoped<IDbInitializer, DbInitializer>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.MapGet("/", () => Results.Redirect("/scalar/v1"));
                app.MapScalarApiReference();
            }

            app.UseHttpsRedirection();
            app.UseMiddleware<GlobalExceptionMiddleware>();
            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();
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

            await app.RunAsync();
        }
    }
}
