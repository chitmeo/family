using System.Configuration;

using Dev.Common;
using Dev.Configuration.Options;
using Dev.Module.Bible.Application.Persistence;
using Dev.Module.Bible.Infrastructure.Persistence;

using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Dev.Module.Bible;

public static class DependencyInjection
{

    public static void AddBibleModule(this IServiceCollection services, WebApplicationBuilder builder)
    {
        var dataConnectionOptions = builder.Services.BuildServiceProvider().GetRequiredService<DataConnectionOptions>();

        var provider = dataConnectionOptions.Provider;
        var connectionString = dataConnectionOptions.ConnectionString;

        switch (provider)
        {
            case DataProvider.MariaDB:
            case DataProvider.MySQL:
                services.AddScoped<IBibleDbContext, BibleDbContext>(provider =>
                {
                    var optionsBuilder = new DbContextOptionsBuilder<BibleDbContext>();
                    optionsBuilder.UseMySQL(connectionString);
                    return new BibleDbContext(optionsBuilder.Options);
                });
                break;
            case DataProvider.SqlServer:
                services.AddScoped<IBibleDbContext, BibleDbContext>(provider =>
                {
                    var optionsBuilder = new DbContextOptionsBuilder<BibleDbContext>();
                    optionsBuilder.UseSqlServer(connectionString);
                    return new BibleDbContext(optionsBuilder.Options);
                });
                break;

            default:
                throw new ConfigurationErrorsException("Unsupported database provider");
        }
    }

    public static void UseBibleModule(this IApplicationBuilder app)
    {
    }
}
