using Microsoft.EntityFrameworkCore;
using ProjectDataManager.BusinessLogic.EmployeeHandlers;
using ProjectDataManager.BusinessLogic.ProjectHandlers;
using ProjectDataManager.Contracts.IRepositories;
using ProjectDataManager.Contracts.IUnitOfWork;
using ProjectDataManager.DataAccess;
using ProjectDataManager.DataAccess.Repositories;
using ProjectDataManager.DataAccess.UnitOfWork;

namespace ProjectDataManager.AppConfigurations;

public static class DependenciesConfiguration
{
    public static void ConfigureAppDIRepositories(this IServiceCollection services)
    {
        services.AddTransient<IEmployeesRepository, EmployeesRepository>();
        services.AddTransient<IProjectsRepository, ProjectsRepository>();
    }

    public static void ConfigureApDIServices(this IServiceCollection services)
    {
        services.AddScoped<DbContext>(provider => provider.GetRequiredService<ProjectDataManagerDbContext>());
        services.AddTransient<IUnitOfWork, UnitOfWork>();
    }

    public static void ConfigureAppDIHandlers(this IServiceCollection services)
    {
        services.AddTransient<GetEmployeeHandle>();
        services.AddTransient<CreateEmployeeHandler>();
        services.AddTransient<UpdateEmployeeHandler>();
        services.AddTransient<DeleteEmployeeHandler>();

        services.AddTransient<GetProjectHandler>();
        services.AddTransient<CreateProjectHandler>();
        services.AddTransient<UpdateProjectHandler>();
        services.AddTransient<DeleteProjectHandler>();

        services.AddTransient<AddEmployeeToProjectHandler>();
        services.AddTransient<RemoveEmployeeFromProjectHandler>();
    }
}
