using Spark.Templates.Blazor.Application.Models;

namespace Spark.Templates.Blazor.Pages.Components;

public class PageState
{
	public User User { get; set; } = new();
	public string AppName { get; set; }
}