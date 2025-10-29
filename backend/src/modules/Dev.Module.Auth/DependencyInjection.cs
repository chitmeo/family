using System.Configuration;
using System.Reflection;
using System.Text;

using Dev.Common;
using Dev.Configuration.Options;
using Dev.Mediator;
using Dev.Module.Auth.Application.Interfaces.Persistence;
using Dev.Module.Auth.Application.Interfaces.Security;
using Dev.Module.Auth.Domain.Services;
using Dev.Module.Auth.Infrastructure.Identity;
using Dev.Module.Auth.Infrastructure.Persistence;
using Dev.Module.Auth.Infrastructure.Services.Security;


using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Dev.Module.Auth;

public static class DependencyInjection
{
    public static void AddAuthModule(this IServiceCollection services, WebApplicationBuilder builder)
    {
        ConfigJwtAuthenticate(services, builder);

        // Resolve DataConnectionOptions
        var dataConnectionOptions = builder.Services.BuildServiceProvider().GetRequiredService<DataConnectionOptions>();

        var provider = dataConnectionOptions.Provider;
        var connectionString = dataConnectionOptions.ConnectionString;

        switch (provider)
        {
            case DataProvider.MariaDB:
            case DataProvider.MySQL:
                services.AddScoped<IAuthDbContext, AuthDbContext>(provider =>
                {
                    var optionsBuilder = new DbContextOptionsBuilder<AuthDbContext>();
                    optionsBuilder.UseMySQL(connectionString);
                    return new AuthDbContext(optionsBuilder.Options);
                });
                break;
            case DataProvider.SqlServer:
                services.AddScoped<IAuthDbContext, AuthDbContext>(provider =>
                {
                    var optionsBuilder = new DbContextOptionsBuilder<AuthDbContext>();
                    optionsBuilder.UseSqlServer(connectionString);
                    return new AuthDbContext(optionsBuilder.Options);
                });
                break;

            default:
                throw new ConfigurationErrorsException("Unsupported database provider");
        }
        // Register Services
        services.AddServices();        
    }

    private static void AddServices(this IServiceCollection services)
    {
        services.AddScoped<IJwtService, JwtService>();
        services.AddScoped<IPasswordHasher, PasswordHasherService>();
    }

    private static void ConfigJwtAuthenticate(IServiceCollection services, WebApplicationBuilder builder)
    {
        var jwtConfig = builder.Configuration.GetSection("JwtConfig").Get<JwtConfig>();
        if (jwtConfig == null)
        {
            throw new ConfigurationErrorsException("JwtConfig not found in appsettings.json");
        }
        services.AddSingleton<JwtConfig>(jwtConfig);

        builder.Services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtConfig.Issuer,
                        ValidAudience = jwtConfig.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.Key))
                    };
                });
        builder.Services.AddAuthorization();
    }

    public static void UseAuthModule(this IApplicationBuilder app)
    {
        app.UseAuthentication();
        app.UseAuthorization();
    }
}
