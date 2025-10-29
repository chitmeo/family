using System.Configuration;

using Dev.Common;
using Dev.Configuration.Options;
using Dev.Module.Accounting.Application.Interfaces.Persistence;
using Dev.Module.Accounting.Infrastructure.Persistence;

using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Dev.Module.Accounting;

public static class DependencyInjection
{
    public static void AddAccountingModule(this IServiceCollection services, WebApplicationBuilder builder)
    {
        var dataConnectionOptions = builder.Services.BuildServiceProvider().GetRequiredService<DataConnectionOptions>();

        var provider = dataConnectionOptions.Provider;
        var connectionString = dataConnectionOptions.ConnectionString;

        switch (provider)
        {
            case DataProvider.MariaDB:
            case DataProvider.MySQL:
                services.AddScoped<IAccountingDbContext, AccountingDbContext>(provider =>
                {
                    var optionsBuilder = new DbContextOptionsBuilder<AccountingDbContext>();
                    optionsBuilder.UseMySQL(connectionString);
                    return new AccountingDbContext(optionsBuilder.Options);
                });
                break;
            case DataProvider.SqlServer:
                services.AddScoped<IAccountingDbContext, AccountingDbContext>(provider =>
                {
                    var optionsBuilder = new DbContextOptionsBuilder<AccountingDbContext>();
                    optionsBuilder.UseSqlServer(connectionString);
                    return new AccountingDbContext(optionsBuilder.Options);
                });
                break;

            default:
                throw new ConfigurationErrorsException("Unsupported database provider");
        }
        services.AddServices();
    }

    private static void AddServices(this IServiceCollection services)
    {
    }

    public static void UseAccountingModule(this IApplicationBuilder app)
    {
    }
}
