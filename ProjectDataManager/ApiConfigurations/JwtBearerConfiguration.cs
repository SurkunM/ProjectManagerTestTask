using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using ProjectDataManager.Contracts.IServices;
using ProjectDataManager.DataAccess;
using ProjectDataManager.Model;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ProjectDataManager.ApiConfigurations;

public static class JwtBearerConfiguration
{
    public static void ConfigureApiJwtBearer(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {

                ValidIssuer = configuration["Jwt:Issuer"],
                ValidAudience = configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:SecretKey"]!)),

                ValidateIssuerSigningKey = true,
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,

                ClockSkew = TimeSpan.FromMinutes(5)
            };

            options.Events = new JwtBearerEvents
            {
                OnTokenValidated = context =>
                {
                    var blacklistService = context.HttpContext.RequestServices.GetRequiredService<IJwtBlacklistService>();
                    var jtiClaim = context.Principal?.FindFirst(JwtRegisteredClaimNames.Jti);

                    if (jtiClaim != null && blacklistService.IsTokenRevoked(jtiClaim.Value))
                    {
                        context.Fail("Token has been revoked");
                    }

                    return Task.CompletedTask;
                }
            };
        });
    }

    public static void ConfigureApiIdentity(this IServiceCollection services)
    {
        services.AddIdentity<Employee, IdentityRole<int>>(options =>
        {
            options.Password.RequiredLength = 6;
            options.Password.RequireDigit = false;
            options.Password.RequireLowercase = true;
            options.Password.RequireUppercase = true;
            options.Password.RequireNonAlphanumeric = false;
        })
        .AddEntityFrameworkStores<ProjectDataManagerDbContext>()
        .AddDefaultTokenProviders();

        services.Configure<IdentityOptions>(options =>
        {
            options.ClaimsIdentity.RoleClaimType = ClaimTypes.Role;
        });
    }
}
