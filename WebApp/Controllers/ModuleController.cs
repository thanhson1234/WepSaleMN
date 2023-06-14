using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using WebApp.Entities;
using WebApp.Helper;
using WebApp.Models;

namespace WebApp.Controllers
{
	public class ModuleController : Controller
	{
		private readonly ILogger<ModuleController> _logger;
		private readonly IConfiguration _config;

		public ModuleController(ILogger<ModuleController> logger, IConfiguration configuration)
		{
			_logger = logger;
			this._config = configuration;
		}
		public IActionResult Show( string SearchByName = "")
		{
			using ( var db = new BaseCoreDataContext())
			{
				int pageSize = Convert.ToInt32(_config["AppSetting:PageSize"]);
				int currentPage = 1;
				var LstModule = new List<AspModule>();
				ViewBag.SearchByName = SearchByName;
				if (string.IsNullOrEmpty(SearchByName))
				{
					var ShowModule = db.AspModules.ToList();
					LstModule = ShowModule.Skip(pageSize * (currentPage - 1)).Take(pageSize).ToList();
					int totalRecored = ShowModule.Count(); //  toán tử count để đếm tổng bản ghi
					ViewBag.TotalRecored = totalRecored;
					ViewBag.TotalPage = (totalRecored / pageSize) + ((totalRecored % pageSize) > 0 ? 1 : 0);
					ViewBag.RecoredFrom = 1;
					ViewBag.CurrentPage = currentPage;
					ViewBag.RecoredTo = ViewBag.TotalPage == 1 ? totalRecored : pageSize;
				}
				else
				{
					var ShowModule = db.AspModules.ToList().Where(n => n.Name.ToLower().Contains(SearchByName.ToString().ToLower())).ToList();
					LstModule = ShowModule.Skip(pageSize * (currentPage - 1)).Take(pageSize).ToList();
					int totalRecored = ShowModule.Count(); //  toán tử count để đếm tổng bản ghi
					ViewBag.TotalRecored = totalRecored;
					ViewBag.TotalPage = (totalRecored / pageSize) + ((totalRecored % pageSize) > 0 ? 1 : 0);
					ViewBag.RecoredFrom = 1;
					ViewBag.CurrentPage = currentPage;
					ViewBag.RecoredTo = ViewBag.TotalPage == 1 ? totalRecored : pageSize;
				}
				return View("~/Views/Module/Show.cshtml", LstModule);
			}
		}

		public async Task<IActionResult> ModulePaging(int currentPage = 0, string SearchByName = "")
		{
			var db = new BaseCoreDataContext();
			var pageSize = Convert.ToInt32(_config["AppSetting:PageSize"]);   // quy định 1 size có bao nhiêu bản ghi cấu hình trong setting
			var lstModule = new List<AspModule>();
			var totalRecored = 0;
			var result = "";
			if (string.IsNullOrEmpty(SearchByName))
			{
				var ShowModule = db.AspModules.ToList();
				lstModule = ShowModule.Skip(pageSize * (currentPage - 1)).Take(pageSize).ToList();
				totalRecored = ShowModule.Count();
				ViewBag.TotalRecored = totalRecored;                                        // lấy ra tổng bản ghi
				ViewBag.TotalPage = (totalRecored / pageSize) + ((totalRecored % pageSize) > 0 ? 1 : 0);      // lấy ra tổng size page
				ViewBag.CurrentPage = currentPage;                                      // page giện tại
				ViewBag.RecoredFrom = 1;                // bắt đầu từ 1 
				ViewBag.RecoredTo = ViewBag.TotalPage == 1 ? totalRecored : pageSize;           // page tiếp theo
				ViewBag.Stt = (currentPage - 1) * pageSize;              // ID
				result = await BaseExtensions.RenderViewToStringAsync(this, "~/Views/Module/Ajax/ModulePaging.cshtml", lstModule);
			}
			else
			{
				var ShowModule = db.AspModules.ToList().Where(n => n.Name.ToLower().Contains(SearchByName.ToLower().ToString().ToLower())).ToList();
				lstModule = ShowModule.Skip(pageSize * (currentPage - 1)).Take(pageSize).ToList();
				totalRecored = ShowModule.Count();
				ViewBag.TotalRecored = totalRecored;
				ViewBag.TotalPage = (totalRecored / pageSize) + ((totalRecored % pageSize) > 0 ? 1 : 0);
				ViewBag.CurrentPage = currentPage;
				ViewBag.RecoredFrom = 1;
				ViewBag.RecoredTo = ViewBag.TotalPage == 1 ? totalRecored : pageSize;
				ViewBag.Stt = (currentPage - 1) * pageSize;
				result = await BaseExtensions.RenderViewToStringAsync(this, "~/Views/Module/Ajax/ModulePaging.cshtml", lstModule);
			}

			return Content(result);
		}

		[HttpGet]
		public IActionResult Add()
		{
			return View();
		}

		[HttpPost]
		public IActionResult Add(ModelViewModel item)
		{
			try
			{
				using (var db = new BaseCoreDataContext())
				{

					var now = DateTime.Now;
					var mappper = MapperConfig.InitializeAutomapperModule();
					var Dto = mappper.Map<ModelViewModel, AspModule>(item);
					Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<AspModule> _role = db.AspModules.Add(Dto);
					db.SaveChanges();
					var prid = _role.Entity.Id;

					return RedirectToAction("Show", "Module");
				}
			
			}  catch (Exception e)
            {
				Console.WriteLine(e.InnerException.Message);
			}
			return View();
		}
	}
}
