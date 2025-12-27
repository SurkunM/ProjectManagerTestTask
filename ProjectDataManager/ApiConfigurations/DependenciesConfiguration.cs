using Microsoft.EntityFrameworkCore;
using ProjectDataManager.BusinessLogic.EmployeeHandlers;
using ProjectDataManager.BusinessLogic.IdentityHandlers;
using ProjectDataManager.BusinessLogic.ProjectHandlers;
using ProjectDataManager.BusinessLogic.ProjectTaskHandler;
using ProjectDataManager.BusinessLogic.Services;
using ProjectDataManager.Contracts.IRepositories;
using ProjectDataManager.Contracts.IServices;
using ProjectDataManager.Contracts.IUnitOfWork;
using ProjectDataManager.DataAccess;
using ProjectDataManager.DataAccess.Repositories;
using ProjectDataManager.DataAccess.Services;
using ProjectDataManager.DataAccess.UnitOfWork;

namespace ProjectDataManager.ApiConfigurations;

public static class DependenciesConfiguration
{
    public static void ConfigureApiDIRepositories(this IServiceCollection services)
    {
        services.AddTransient<IProjectsRepository, ProjectsRepository>();
        services.AddTransient<IProjectTasksRepository, ProjectTasksRepository>();
    }

    public static void ConfigureApiDIServices(this IServiceCollection services)
    {
        services.AddScoped<DbContext>(provider => provider.GetRequiredService<ProjectDataManagerDbContext>());
        services.AddScoped<IJwtBlacklistService, JwtBlacklistService>();

        services.AddTransient<IUnitOfWork, UnitOfWork>();
        services.AddTransient<IEmployeeService, EmployeeService>();
    }

    public static void ConfigureApiDIHandlers(this IServiceCollection services)
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

        services.AddTransient<EmployeeAuthenticationHandler>();
        services.AddTransient<EmployeeAuthorizationHandler>();

        services.AddTransient<CreateProjectTaskHandler>();
        services.AddTransient<DeleteProjectTaskHandler>();
        services.AddTransient<GetProjectTasksHandler>();
        services.AddTransient<UpdateProjectTaskHandler>();

        services.AddTransient<IJwtGenerationService, JwtGenerationService>();
    }
}
