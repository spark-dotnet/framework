using Microsoft.AspNetCore.Mvc.RazorPages;
using Serilog;

namespace Spark.Templates.Razor.Pages;

public class Index : PageModel
{
	public void OnGet()
	{
		Log.Information("This is the homepage.");
	}
}
