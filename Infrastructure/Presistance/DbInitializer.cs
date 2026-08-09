using Domain.Contracts;
using Domain.Models.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Persistence
{
    public class DbInitializer : IDbInitializer
    {
        private readonly ApplicationDbContext _dbContext;

        public DbInitializer(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task InitializeAsync()
        {
            // Apply Migrations
            if ((await _dbContext.Database.GetPendingMigrationsAsync()).Any())
            {
                await _dbContext.Database.MigrateAsync();
            }

            // Seed Grades
            if (!await _dbContext.Grades.AnyAsync())
            {
                var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "DataSeeding", "grades.json");

                if (!File.Exists(filePath))
                {
                    throw new FileNotFoundException(
                        $"Seed file not found: {filePath}");
                }

                var gradesData = await File.ReadAllTextAsync(filePath);

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var grades = JsonSerializer.Deserialize<List<Grade>>(
                    gradesData,
                    options);

                if (grades is null || grades.Count == 0)
                    throw new Exception("No grades found in grades.json");

                await _dbContext.Grades.AddRangeAsync(grades);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
