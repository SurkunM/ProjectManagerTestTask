using Microsoft.EntityFrameworkCore;
using ProjectDataManager.AppConfigurations;
using ProjectDataManager.Middleware;

namespace ProjectDataManager;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        var connectionString = builder.Configuration.GetConnectionString("ProjectDataManagerConnection");

        builder.Services.ConfigureAppDbContext(connectionString);

        builder.Services.ConfigureAppIdentity();
        builder.Services.ConfigureAppJwtBearer(builder.Configuration);

        builder.Services.AddControllersWithViews();

        builder.Services.ConfigureApDIServices();
        builder.Services.ConfigureAppDIRepositories();
        builder.Services.ConfigureAppDIHandlers();

        var app = builder.Build();

        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }

        app.InitializeAppDb();

        app.UseHttpsRedirection();
        app.UseDefaultFiles();
        app.UseStaticFiles();

        app.UseMiddleware<ExceptionMiddleware>();
        app.UseRouting();

        app.UseAuthorization();

        app.MapControllers();
        app.MapFallbackToFile("index.html");

        app.Run();
    }
}
