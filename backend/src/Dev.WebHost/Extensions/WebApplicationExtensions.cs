using Dev.Module.Auth;
using Dev.Module.Bible;
using Dev.Module.HomeLover;

namespace Dev.WebHost.Extensions;

internal static class WebApplicationExtensions
{
    public static async Task ConfigureRequestPipelineAsync(this WebApplication app)
    {
        app.UseDefaultFiles();
        app.MapStaticAssets();

        //Response Compression
        app.UseResponseCompression();

        // Configure the HTTP request pipeline.
        app.UseExceptionHandler(); // Use custom exception handler. GlobalExceptionHandler
        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseCors();

        app.MapControllers();

        //core
        app.UseDev();
        //use modules
        app.UseAuthModule();
        app.UseHomeLoverModule();
        app.UseBibleModule();
        //run
        await app.RunAsync();
    }
}

