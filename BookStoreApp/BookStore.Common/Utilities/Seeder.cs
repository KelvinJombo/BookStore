using BookStore.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace BookStore.Common.Utilities
{
    public class Seeder
    {
        public static async Task SeedRolesAndAdmin(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();

            // Seed roles
            await EnsureRoleExists(roleManager, "Admin");
            await EnsureRoleExists(roleManager, "User");

            // Seed super admin user
            await EnsureAdminUserExists(userManager);
        }

        private static async Task EnsureRoleExists(RoleManager<IdentityRole> roleManager, string roleName)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                var role = new IdentityRole(roleName);
                await roleManager.CreateAsync(role);
            }
        }

        private static async Task EnsureAdminUserExists(UserManager<AppUser> userManager)
        {
            var adminUserName = "Admin";
            var adminEmail = "admin@gmail.com";
            var adminPassword = "Password@123";

            var adminUser = await userManager.FindByNameAsync(adminUserName);
            if (adminUser == null)
            {
                var user = new AppUser
                {
                    UserName = adminUserName,
                    Email = adminEmail,
                    EmailConfirmed = true,
                    FirstName = adminUserName,
                    CreatedAt = DateTime.UtcNow
                };

                var result = await userManager.CreateAsync(user, adminPassword);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "Admin");
                }
            }
        }

    }
}