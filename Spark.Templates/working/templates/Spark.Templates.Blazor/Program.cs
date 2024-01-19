using Microsoft.EntityFrameworkCore;
using Serilog;
using Spark.Library.Config;
using Spark.Library.Environment;
using Spark.Library.Routing;
using Spark.Templates.Blazor.Application.Database;
using Spark.Templates.Blazor.Application.Startup;
using Spark.Templates.Blazor.Pages;
using Vite.AspNetCore.Extensions;


EnvManager.LoadConfig();

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.SetupAppConfig();
builder.Services.AddAppServices(builder.Configuration);

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration).CreateLogger();
builder.Host.UseSerilog();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
    db.Database.Migrate();
}

if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}

if (app.Environment.IsDevelopment())
{
    app.UseViteDevMiddleware();
}

app.UseSerilogRequestLogging();
app.UseStatusCodePagesWithRedirects("/status-code/{0}");
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseStaticFiles();
app.UseSession();
app.UseAntiforgery();
app.MapRazorComponents<PageRoutes>();
app.MapMinimalApis<Program>();

app.Services.SetupScheduledJobs();
app.Services.SetupEvents();

app.Run();
