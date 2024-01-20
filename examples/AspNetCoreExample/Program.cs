// ==================================================
// Take a look at the csproj and appsettings files :)
// ==================================================

using AspNetCoreExample;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJavaScriptModule("appsettings.js");
builder.Configuration.AddJavaScriptModule($"appsettings.{builder.Environment.EnvironmentName}.js");

builder.Services.Configure<DatabasesOptions>(builder.Configuration.GetSection("Databases"));

var app = builder.Build();

app.MapGet("/movies", (IOptions<DatabasesOptions> dbOptions) =>
{
    // Do some database queries
    return dbOptions.Value;
});

app.Run();