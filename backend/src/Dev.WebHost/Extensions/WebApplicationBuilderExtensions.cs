
using System.Configuration;

using Dev.Module.Accounting;
using Dev.Module.Auth;
using Dev.Module.Bible;
using Dev.Module.HomeLover;
using Dev.WebHost.Exceptions;
using Dev.WebHost.Services;

using global::Dev.Configuration.Options;

using Microsoft.AspNetCore.Server.Kestrel.Core;

namespace Dev.WebHost.Extensions;

internal static class WebApplicationBuilderExtensions
{
    public static void ConfigureWebApplication(this WebApplicationBuilder builder)
    {

        builder.Services.AddHttpContextAccessor();

        builder.AddTimeouts();

        // Bind DataConnectionOptions from appsettings.json
        var dataConnectionOptions = builder.Configuration
            .GetSection("DataConnection")
            .Get<DataConnectionOptions>();

        if (dataConnectionOptions is null)
        {
            throw new ConfigurationErrorsException("Missing DataConnection configuration");
        }

        // Register DataConnectionOptions as a Singleton
        builder.Services.AddSingleton(dataConnectionOptions);

        //https://learn.microsoft.com/en-us/aspnet/core/fundamentals/error-handling?view=aspnetcore-9.0
        builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
        builder.Services.AddProblemDetails();

        builder.AddCORS();
        builder.Services.AddResponseCompression(opt =>
        {
            opt.EnableForHttps = true;
        });
        //common
        builder.Services.AddDev(builder);
        //modules
        builder.Services.AddAccountingModule(builder);
        builder.Services.AddAuthModule(builder);
        builder.Services.AddBibleModule(builder);
        builder.Services.AddHomeLoverModule(builder);
        builder.Services.AddMediator();
        builder.Services.AddControllers();

        builder.Services.AddHttpClient();
        builder.Services.AddHostedService<KeepAliveService>();
        builder.Services.Configure<KeepAliveOptions>(
        builder.Configuration.GetSection("KeepAlive"));

    }

    private static void AddCORS(this WebApplicationBuilder builder)
    {
        var allowedOrigins = builder.Configuration.GetSection("AllowedOrigins").Get<string[]>();

        if (allowedOrigins == null)
            throw new ConfigurationErrorsException("Missing AllowedOrigins configuration");

        builder.Services.AddCors(options =>
        {
            options.AddDefaultPolicy(
                policy =>
                {
                    policy.WithOrigins(allowedOrigins)
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
        });
    }

    private static void AddTimeouts(this WebApplicationBuilder builder)
    {
        // Get Kestrel configuration section
        var kestrelConfig = builder.Configuration.GetSection("Kestrel:Limits");

        if (kestrelConfig.Exists()) // Check if the section exists
        {
            builder.Services.Configure<KestrelServerOptions>(options =>
            {
                var timeoutSeconds = kestrelConfig.GetValue<int>("KeepAliveTimeout", 120);
                options.Limits.KeepAliveTimeout = TimeSpan.FromSeconds(timeoutSeconds);
            });
        }
    }
}
