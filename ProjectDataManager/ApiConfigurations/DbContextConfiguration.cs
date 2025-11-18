using Microsoft.EntityFrameworkCore;
using ProjectDataManager.DataAccess;

namespace ProjectDataManager.ApiConfigurations;

public static class DbContextConfiguration
{
    public static void ConfigureApiDbContext(this IServiceCollection services, string? connectionString)
    {
        ArgumentNullException.ThrowIfNull(connectionString);

        services.AddDbContext<ProjectDataManagerDbContext>(options =>
        {
            options
                .UseSqlServer(connectionString)
                .UseLazyLoadingProxies();
        }, ServiceLifetime.Scoped, ServiceLifetime.Transient);
    }

    public static void InitializeApiDb(this WebApplication app)
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
