using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProjectDataManager.DataAccess;
using ProjectDataManager.Model;
using ProjectDataManager.Model.Enums;

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

    public static async Task InitializeApiDb(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var service = scope.ServiceProvider;

        try
        {
            var db = service.GetRequiredService<ProjectDataManagerDbContext>();
            db.Database.Migrate();

            if (!db.Roles.Any())
            {
                await InitializeRolesAsync(service);
            }

            await CrateAdmin(service);
        }
        catch (Exception ex)
        {
            var logger = app.Services.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "An error occurred when creating the database.");

            throw;
        }
    }

    private static async Task CrateAdmin(IServiceProvider serviceProvider)
    {
        var userManager = serviceProvider.GetRequiredService<UserManager<Employee>>();

        const string AdminUserName = "Admin";
        var adminUser = await userManager.FindByNameAsync(AdminUserName);

        if (adminUser is not null)
        {
            return;
        }

        var admin = new Employee
        {
            FirstName = AdminUserName,
            LastName = AdminUserName,
            UserName = AdminUserName,
            Email = AdminUserName
        };

        const string AdminPassword = "Admin123!";
        var result = await userManager.CreateAsync(admin, AdminPassword);

        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            throw new Exception($"Error when creating an application administrator: {errors}");
        }

        result = await userManager.AddToRoleAsync(admin, Roles.Supervisor.ToString());

        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            throw new Exception($"Error when assigning role: {errors}");
        }
    }

    private static async Task InitializeRolesAsync(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole<int>>>();

        var roleNames = Enum.GetNames<Roles>();

        foreach (string roleName in roleNames)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                await roleManager.CreateAsync(new IdentityRole<int> { Name = roleName });
            }
        }
    }
}
