using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TalentSphere.Repositories.Interfaces;
using TalentSphere.Models;
using TalentSphere.Enums;

namespace TalentSphere.Utilities
{
    public static class DataSeeder
    {
        public static async Task SeedAsync(IServiceProvider services)
        {
            var config = services.GetRequiredService<IConfiguration>();
            var roleRepo = services.GetRequiredService<IRoleRepository>();
            var userRepo = services.GetRequiredService<IUserRepository>();
            var userRoleRepo = services.GetRequiredService<IUserRoleRepository>();

            // 1. Ensure roles exist
            var existingRoles = (await roleRepo.GetAllAsync()).ToList();
            if (!existingRoles.Any())
            {
                foreach (RoleName rn in Enum.GetValues(typeof(RoleName)))
                {
                    var role = new Role
                    {
                        Name = rn,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    };
                    await roleRepo.AddAsync(role);
                }
                await roleRepo.SaveChangesAsync();
                existingRoles = (await roleRepo.GetAllAsync()).ToList();
            }

            // 2. Seed admin user if configured
            var seedEmail = config["SeedAdmin:Email"];
            var seedPassword = config["SeedAdmin:Password"];

            if (string.IsNullOrWhiteSpace(seedEmail) || string.IsNullOrWhiteSpace(seedPassword))
            {
                return; // nothing to do
            }

            var user = await userRepo.GetByEmailAsync(seedEmail);
            if (user == null)
            {
                var userEntity = new User
                {
                    Name = "Master Admin",
                    Email = seedEmail,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(seedPassword),
                    Phone = null,
                    Status = UserStatus.Active,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    IsDeleted = false
                };

                await userRepo.AddAsync(userEntity);
                await userRepo.SaveChangesAsync();
                user = userEntity;
            }

            // 3. Ensure admin role link
            var adminRole = existingRoles.FirstOrDefault(r => r.Name == RoleName.Admin)
                            ?? (await roleRepo.GetByNameAsync(RoleName.Admin.ToString()));

            if (adminRole != null)
            {
                var existingUserRole = await userRoleRepo.GetByUserIdAsync(user.UserID);
                if (existingUserRole == null || existingUserRole.RoleId != adminRole.RoleID)
                {
                    var ur = new UserRole
                    {
                        UserId = user.UserID,
                        RoleId = adminRole.RoleID,
                        CreatedAt = DateTime.UtcNow,
                        IsDeleted = false
                    };
                    await userRoleRepo.AddAsync(ur);
                    await userRoleRepo.SaveChangesAsync();
                }
            }
        }
    }
}
