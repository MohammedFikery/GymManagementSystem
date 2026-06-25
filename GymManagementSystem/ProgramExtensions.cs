using GymManagementSystem.DAL.DataSeading;
using GymManagementSystem.DAL.DataSeeding;
using GymManagementSystem.DAL.Models.Enums;
using GymManagementSystem.DbContexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GymManagementSystem.PL;

public static class ProgramExtensions
{
    public static async Task MigrateAndSeedDatabaseAsync(this WebApplication app)
    {

        using var scope = app.Services.CreateScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<GymDbContext>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();

        var pendingMigrations = await dbContext.Database.GetPendingMigrationsAsync();

        if (pendingMigrations.Any())
        {
            logger.LogInformation($"Applying {pendingMigrations.Count()} Pending Migrations");
            await dbContext.Database.MigrateAsync();

        }
        var seedFolderPath = Path.Combine(app.Environment.ContentRootPath, "wwwroot", "Files");
        await GymDataSeeding.SeedAsync(dbContext, seedFolderPath, logger);
        await IdentityDataSeeding.SeedIdentityDataAsync(roleManager, userManager, logger);
    }
}
