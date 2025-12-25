using Dev.WebHost.Extensions;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
builder.ConfigureWebApplication();

WebApplication app = builder.Build();
await app.ConfigureRequestPipelineAsync();