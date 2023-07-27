using Spark.Library.Config;
using Spark.Library.Environment;
using Spark.Templates.Api.Application.Startup;

EnvManager.LoadConfig();

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.SetupSparkConfig();

builder.Services.AddAppServices(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Services.RegisterScheduledJobs();
app.Services.RegisterEvents();

app.Run();
