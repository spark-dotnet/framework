using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Spark.Templates.Mvc.Application.Database;
using Spark.Templates.Mvc.Application.ViewModels;
using System.Diagnostics;
using ILogger = Spark.Library.Logging.ILogger;

namespace Spark.Templates.Mvc.Application.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger _logger;
		private readonly DatabaseContext _db;

		public HomeController(ILogger logger, DatabaseContext db)
        {
            _logger = logger;
			_db = db;
        }

        [Route("")]
        public IActionResult Index()
		{
			return View();
        }

		[Authorize]
		[Route("dashboard")]
        public IActionResult Dashboard()
        {
            return View();
		}
    }
}