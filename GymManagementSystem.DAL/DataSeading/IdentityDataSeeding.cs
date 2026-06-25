using System.Data;
using GymManagementSystem.DAL.Models.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
namespace GymManagementSystem.DAL.DataSeeding;

public class IdentityDataSeeding
{
    public static async Task SeedIdentityDataAsync(RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager, ILogger logger, CancellationToken ct = default)
    {
        try
        {
            bool hasUsers = await userManager.Users.AnyAsync(ct);
            bool hasRoles = await roleManager.Roles.AnyAsync(ct);
            if (hasRoles && hasUsers) return;

            if (!hasRoles)
            {
                var roles = new List<IdentityRole>()
                {
                    new IdentityRole("SuperAdmin"),
                    new IdentityRole("Admin")
                };
                foreach (var Role in roles)
                {
                    if (!await roleManager.RoleExistsAsync(Role.Name!))
                    {
                        var roleResult = await roleManager.CreateAsync(Role);
                        if (!roleResult.Succeeded)
                        {
                            logger.LogError($"Failed To Create Role {Role.Name}: {string.Join(";", roleResult.Errors.Select(e => e.Description))}");
                        }
                    }
                }
            }

            if (!hasUsers)
            {
                var MainAdmin = new AppUser()
                {
                    FirstName = "Mohammed",
                    LastName = "Fekry",
                    Email = "mf01000009120@gmail.com",
                    UserName = "MF",
                    PhoneNumber = "01000005619",
                    EmailConfirmed = true
                };
                var mainAdminResult = await userManager.CreateAsync(MainAdmin, "P@ssw0rd");
                if (mainAdminResult.Succeeded)
                {
                    await userManager.AddToRoleAsync(MainAdmin, "SuperAdmin");
                }
                else
                {
                    logger.LogError($"Failed To Create User {MainAdmin.UserName}: {string.Join(";", mainAdminResult.Errors.Select(e => e.Description))}");
                }

                var Admin = new AppUser()
                {
                    FirstName = "Mohammed",
                    LastName = "Ahmed",
                    Email = "mf.admin@gmail.com",
                    UserName = "MF_Admin",
                    PhoneNumber = "01000013342",
                    EmailConfirmed = true
                };
                var adminResult = await userManager.CreateAsync(Admin, "P@ssw0rd");
                if (adminResult.Succeeded)
                {
                    await userManager.AddToRoleAsync(Admin, "Admin");
                }
                else
                {
                    logger.LogError($"Failed To Create User {Admin.UserName}: {string.Join(";", adminResult.Errors.Select(e => e.Description))}");
                }

                logger.LogInformation($"Data Seeded");
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Identity Seeding failed");
            return;
        }
    }
}
