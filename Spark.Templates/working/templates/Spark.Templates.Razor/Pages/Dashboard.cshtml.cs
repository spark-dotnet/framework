using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Spark.Templates.Razor.Pages;

[Authorize]
public class Dashboard : PageModel
{
}
