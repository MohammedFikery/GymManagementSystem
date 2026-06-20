using System.Text.Json;
using GymManagementSystem.DbContexts;
using GymManagementSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GymManagementSystem.DAL.DataSeading;

public static class GymDataSeeding
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public static async Task SeedAsync(GymDbContext dbContext,string seedFolderPath, ILogger logger,CancellationToken ct = default)
    {
        try
        {
            logger.LogInformation("Starting data seeding from {Path}", seedFolderPath);

            await SeedPlansAsync(dbContext, seedFolderPath, logger, ct);
            logger.LogInformation("Data seeding completed");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Gym data seeding failed");
            throw;
        }
    }

    private static async Task SeedPlansAsync( GymDbContext dbContext, string seedFolderPath,ILogger logger, CancellationToken ct)
    {
        if (await dbContext.Plans.AnyAsync(ct))
        {
            logger.LogInformation("Plans already seeded — skipping");
            return;
        }

        var plans = LoadDataFromJsonFile<Plan>(seedFolderPath, "plans.json");

        if (plans.Count == 0)
        {
            logger.LogWarning("plans.json contained no records — nothing to seed");
            return;
        }
        await dbContext.Plans.AddRangeAsync(plans, ct);
        await dbContext.SaveChangesAsync(ct);
        logger.LogInformation("Seeded {Count} plans", plans.Count);
    }

    private static List<T> LoadDataFromJsonFile<T>(string folderPath, string fileName)
    {
        var filePath = Path.Combine(folderPath, fileName);

        if (!File.Exists(filePath))
        { throw new FileNotFoundException($"Seed data file not found: {filePath}"); }

        var json = File.ReadAllText(filePath);
        return JsonSerializer.Deserialize<List<T>>(json, JsonOptions) ?? [];
    }
}
