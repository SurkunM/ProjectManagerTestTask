using Microsoft.EntityFrameworkCore;
using ProjectDataManager.BusinessLogic.EmployeeHandlers;
using ProjectDataManager.BusinessLogic.ProjectHandlers;
using ProjectDataManager.Contracts.IRepositories;
using ProjectDataManager.Contracts.IUnitOfWork;
using ProjectDataManager.DataAccess;
using ProjectDataManager.DataAccess.Repositories;
using ProjectDataManager.DataAccess.UnitOfWork;
using ProjectDataManager.Middleware;

namespace ProjectDataManager;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddDbContext<ProjectDataManagerDbContext>(options =>
        {
            options
                .UseSqlServer(builder.Configuration.GetConnectionString("ProjectDataManagerConnection"))
                .UseLazyLoadingProxies();
        }, ServiceLifetime.Scoped, ServiceLifetime.Transient);

        builder.Services.AddControllersWithViews();

        builder.Services.AddScoped<DbContext>(provider => provider.GetRequiredService<ProjectDataManagerDbContext>());
        builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();

        builder.Services.AddTransient<IEmployeesRepository, EmployeesRepository>();
        builder.Services.AddTransient<IProjectsRepository, ProjectsRepository>();

        builder.Services.AddTransient<GetEmployeeHandle>();
        builder.Services.AddTransient<CreateEmployeeHandler>();
        builder.Services.AddTransient<UpdateEmployeeHandler>();
        builder.Services.AddTransient<DeleteEmployeeHandler>();

        builder.Services.AddTransient<GetProjectHandler>();
        builder.Services.AddTransient<CreateProjectHandler>();
        builder.Services.AddTransient<UpdateProjectHandler>();
        builder.Services.AddTransient<DeleteProjectHandler>();

        builder.Services.AddTransient<AddEmployeeToProjectHandler>();
        builder.Services.AddTransient<RemoveEmployeeFromProjectHandler>();

        var app = builder.Build();

        using (var scope = app.Services.CreateScope())
        {
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

        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }

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
