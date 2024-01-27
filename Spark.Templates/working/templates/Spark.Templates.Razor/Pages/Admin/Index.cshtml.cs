using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Spark.Templates.Razor.Application.Models;

namespace Spark.Templates.Razor.Pages.Admin;

[Authorize(Roles = CustomRoles.Admin)]
public class Index : PageModel
{
}