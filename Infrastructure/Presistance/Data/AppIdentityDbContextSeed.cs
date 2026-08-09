using Domain.Models.Identity;
using Microsoft.AspNetCore.Identity;
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
        public static async Task SeedRolesAndUsersAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            if (!await roleManager.RoleExistsAsync(UserRoles.Admin))
                await roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));

            if (!await roleManager.RoleExistsAsync(UserRoles.Teacher))
                await roleManager.CreateAsync(new IdentityRole(UserRoles.Teacher));

            if (!await roleManager.RoleExistsAsync(UserRoles.Student))
                await roleManager.CreateAsync(new IdentityRole(UserRoles.Student));

            if (!await roleManager.RoleExistsAsync(UserRoles.Assistant))
                await roleManager.CreateAsync(new IdentityRole(UserRoles.Assistant));

            var teacherEmail = "teacher@eduplatform.com";
            var defaultTeacher = await userManager.FindByEmailAsync(teacherEmail);

            if (defaultTeacher is null)
            {
                var teacher = new ApplicationUser
                {
                    FullName = "Mr. Saad Mostafa",
                    Email = teacherEmail,
                    UserName = teacherEmail,
                    PhoneNumber = "01234567890",
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(teacher, "P@ssword123!");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(teacher, UserRoles.Teacher);
                }
            }
        }
    }
}
