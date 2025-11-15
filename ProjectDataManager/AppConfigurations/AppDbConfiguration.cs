using Microsoft.EntityFrameworkCore;
using ProjectDataManager.DataAccess;

namespace ProjectDataManager.AppConfigurations;

public static class AppDbConfiguration
{
    public static void ConfigureAppDbContext(this IServiceCollection services, string? connectionString)
    {
        ArgumentNullException.ThrowIfNull(connectionString);

        services.AddDbContext<ProjectDataManagerDbContext>(options =>
        {
            options
                .UseSqlServer(connectionString)
                .UseLazyLoadingProxies();
        }, ServiceLifetime.Scoped, ServiceLifetime.Transient);
    }

    public static void InitializeAppDb(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        try
        {
            var db = scope.ServiceProvider.GetRequiredService<ProjectDataManagerDbContext>();
            db.Database.Migrate();
        }
        catch (Exception ex)
        {
            var logger = app.Services.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "An error occurred when creating the database.");

            throw;
        }

    }
}
