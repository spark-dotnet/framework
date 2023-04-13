using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authentication.Cookies;
using Tailwind;
using BlazorSpark.Example.Startup;
using BlazorSpark.Library.Environment;
using BlazorSpark.Library.Logging;
using Serilog;
using Coravel;

EnvManager.Setup();
LogManager.Setup();

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddEnvironmentVariables();
var test = builder.Configuration.GetValue<string>("Coravel:Mail:Host");

// Add services to the container.
builder.Services.RegisterServices();
builder.Services.AddScheduler();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

if (app.Environment.IsDevelopment())
{
	app.RunTailwind("tailwind", "./");
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Services.UseScheduler(scheduler =>
{
	scheduler.Schedule(
		() => Console.WriteLine("runn this job now")
	).EveryMinute();
});

app.Run();
