using Domain.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Shared.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Data
{
    public static class AppIdentityDbContextSeed
    {
        public static async Task SeedRolesAndUsersAsync(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration)
        {
            if (!await roleManager.RoleExistsAsync(UserRoles.Admin))
                await roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));

            if (!await roleManager.RoleExistsAsync(UserRoles.Teacher))
                await roleManager.CreateAsync(new IdentityRole(UserRoles.Teacher));

            if (!await roleManager.RoleExistsAsync(UserRoles.Student))
                await roleManager.CreateAsync(new IdentityRole(UserRoles.Student));

            if (!await roleManager.RoleExistsAsync(UserRoles.Assistant))
                await roleManager.CreateAsync(new IdentityRole(UserRoles.Assistant));

            var adminEmail = configuration["SeedData:AdminUser:Email"];
            var adminPassword = configuration["SeedData:AdminUser:Password"];
            var adminName = configuration["SeedData:AdminUser:FullName"];

            if (!string.IsNullOrEmpty(adminEmail) && !string.IsNullOrEmpty(adminPassword))
            {
                var defaultAdmin = await userManager.FindByEmailAsync(adminEmail);
                if (defaultAdmin is null)
                {
                    var admin = new ApplicationUser
                    {
                        FullName = adminName ?? "System Admin",
                        Email = adminEmail,
                        UserName = adminEmail,
                        EmailConfirmed = true
                    };

                    var result = await userManager.CreateAsync(admin, adminPassword);
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(admin, UserRoles.Admin);
                    }
                }
            }

            var teacherEmail = configuration["SeedData:TeacherUser:Email"];
            var teacherPassword = configuration["SeedData:TeacherUser:Password"];
            var teacherName = configuration["SeedData:TeacherUser:FullName"];
            var teacherPhone = configuration["SeedData:TeacherUser:PhoneNumber"];

            if (!string.IsNullOrEmpty(teacherEmail) && !string.IsNullOrEmpty(teacherPassword))
            {
                var defaultTeacher = await userManager.FindByEmailAsync(teacherEmail);
                if (defaultTeacher is null)
                {
                    var teacher = new ApplicationUser
                    {
                        FullName = teacherName ?? "Default Teacher",
                        Email = teacherEmail,
                        UserName = teacherEmail,
                        PhoneNumber = teacherPhone,
                        EmailConfirmed = true
                    };

                    var result = await userManager.CreateAsync(teacher, teacherPassword);
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(teacher, UserRoles.Teacher);
                    }
                }
            }

        }
    }
}
