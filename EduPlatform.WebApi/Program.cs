
using Domain.Contracts;
using Domain.Models.Identity;
using EduPlatform.WebApi.Extensions;
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

            builder.Services.RegisterAllServices(
                builder.Configuration);

            var app = builder.Build();

            await app.ConfigureMiddlewaresAsync();

            await app.RunAsync();
        }
    }

}
