using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authentication.Cookies;
using Tailwind;
using BlazorSpark.Example.Startup;
using BlazorSpark.Library.Settings;
using BlazorSpark.Library.Logging;
using Serilog;

Env.Load();
LogManager.Setup();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.RegisterServices();

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

app.Run();
