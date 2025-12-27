using Microsoft.EntityFrameworkCore;
using ProjectDataManager.ApiConfigurations;
using ProjectDataManager.Contracts.Settings;
using ProjectDataManager.Middleware;

namespace ProjectDataManager;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        var connectionString = builder.Configuration.GetConnectionString("ProjectDataManagerConnection");

        builder.Services.ConfigureApiDbContext(connectionString);

        builder.Services.ConfigureApiIdentity();
        builder.Services.ConfigureApiJwtBearer(builder.Configuration);

        builder.Services.AddControllersWithViews();
        builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));

        builder.Services.ConfigureApiDIServices();
        builder.Services.ConfigureApiDIRepositories();
        builder.Services.ConfigureApiDIHandlers();

        builder.Services.AddMemoryCache();

        var app = builder.Build();

        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }

        await app.InitializeApiDb();

        app.UseHttpsRedirection();
        app.UseDefaultFiles();
        app.UseStaticFiles();

        app.UseMiddleware<ApiExceptionsMiddleware>();
        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();
        app.MapFallbackToFile("index.html");

        app.Run();
    }
}
