using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Spark.Templates.Mvc.Application.Models;

namespace Spark.Templates.Mvc.Application.Controllers
{
    public class AdminController : Controller
    {
        [HttpGet]
        [Authorize(Policy = CustomRoles.Admin)]
        [Route("admin/dashboard")]
        public IActionResult Dashboard()
        {
            return View();
        }
    }
}
