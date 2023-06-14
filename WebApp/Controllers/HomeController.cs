using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebApp.Entities;
using WebApp.Models;

namespace WebApp.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        BaseCoreDataContext baseCoreDataContext = new BaseCoreDataContext();
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult HeaderPartial()
        {
            return PartialView();
        }
		public IActionResult NavPartial()
		{
			return PartialView(baseCoreDataContext.AspModules.Where(n=>n.Status == 1).ToList());
		}
	}
}