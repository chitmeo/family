using System.Configuration;

using Dev.Common;
using Dev.Configuration.Options;
using Dev.Module.HomeLover.Application.Persistence;
using Dev.Module.HomeLover.Infrastructure.Persistence;

using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Dev.Module.HomeLover;

public static class DependencyInjection
{
    public static void AddHomeLoverModule(this IServiceCollection services, WebApplicationBuilder builder)
    {
        var dataConnectionOptions = builder.Services.BuildServiceProvider().GetRequiredService<DataConnectionOptions>();

        var provider = dataConnectionOptions.Provider;
        var connectionString = dataConnectionOptions.ConnectionString;

        switch (provider)
        {
            case DataProvider.MariaDB:
            case DataProvider.MySQL:
                services.AddScoped<IHomeLoverDbContext, HomeLoverDbContext>(provider =>
                {
                    var optionsBuilder = new DbContextOptionsBuilder<HomeLoverDbContext>();
                    optionsBuilder.UseMySQL(connectionString);
                    return new HomeLoverDbContext(optionsBuilder.Options);
                });
                break;
            case DataProvider.SqlServer:
                services.AddScoped<IHomeLoverDbContext, HomeLoverDbContext>(provider =>
                {
                    var optionsBuilder = new DbContextOptionsBuilder<HomeLoverDbContext>();
                    optionsBuilder.UseSqlServer(connectionString);
                    return new HomeLoverDbContext(optionsBuilder.Options);
                });
                break;

            default:
                throw new ConfigurationErrorsException("Unsupported database provider");
        }
    }

    public static void UseHomeLoverModule(this IApplicationBuilder app)
    {
    }
}
