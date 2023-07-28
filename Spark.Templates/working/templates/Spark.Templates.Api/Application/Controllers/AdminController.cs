using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Spark.Templates.Api.Application.Models;

namespace Spark.Templates.Api.Application.Controllers
{
    public class AdminController : Controller
    {
        [HttpGet]
        [Authorize(Policy = CustomRoles.Admin)]
        [Route("admin/dashboard")]
        public IActionResult Dashboard()
        {
            return Ok();
        }
    }
}
