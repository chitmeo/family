using Dev.WebHost.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.ConfigureWebApplication();

var app = builder.Build();
await app.ConfigureRequestPipelineAsync();