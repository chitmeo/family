using System.Reflection;

using Dev.Common;
using Dev.Domain.Interfaces;
using Dev.Infrastructure;
using Dev.Mediator;
using Dev.Services;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Dev;

public static class DependencyInjection
{
    public static void AddDev(this IServiceCollection services, WebApplicationBuilder builder)
    {
        //create default file provider
        CommonHelper.DefaultFileProvider = new DevFileProvider(builder.Environment);
        services.AddDevDataProtection();

        string connectionString = builder.Configuration.GetConnectionString("DefaultConnection")!;
        services.AddScoped<ICoreDbContext, ICoreDbContext>(provider =>
        {
            var optionsBuilder = new DbContextOptionsBuilder<CoreDbContext>();
            optionsBuilder.UseSqlServer(connectionString);
            return new CoreDbContext(optionsBuilder.Options);
        });

        services.AddScoped<IDevFileProvider, DevFileProvider>();
        //register type finder
        var typeFinder = new TypeFinder();
        services.AddSingleton<ITypeFinder>(typeFinder);

        services.AddScoped<IEncryptionService, EncryptionService>();
        services.AddScoped<IWebHelper, WebHelper>();
    }

    public static IServiceCollection AddMediator(
        this IServiceCollection services)
    {
        services.TryAddScoped<IMediator, DevMediator>();
        string modulePrefix = "Dev.Module.";
        var assemblies = Directory.GetFiles(AppContext.BaseDirectory, $"{modulePrefix}*.dll")
                .Select(Assembly.LoadFrom)
                .ToArray();

        foreach (var assembly in assemblies)
        {
            var handlerTypes = assembly.GetTypes()
                .Where(t => !t.IsAbstract && !t.IsInterface)
                .SelectMany(t => t.GetInterfaces()
                    .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IRequestHandler<,>))
                    .Select(i => new { Service = i, Implementation = t }));

            foreach (var handler in handlerTypes)
            {
                services.AddScoped(handler.Service, handler.Implementation);
            }
        }
        return services;
    }


    public static void UseDev(this IApplicationBuilder app)
    {

    }

    private static void AddDevDataProtection(this IServiceCollection services)
    {

        var dataProtectionKeysPath = CommonHelper.DefaultFileProvider!.MapPath("~/App_Data/DataProtectionKeys");
        var dataProtectionKeysFolder = new DirectoryInfo(dataProtectionKeysPath);

        //configure the data protection system to persist keys to the specified directory
        services.AddDataProtection().PersistKeysToFileSystem(dataProtectionKeysFolder);
    }
}
