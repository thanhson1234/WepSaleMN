using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApp.Entities;
using WebApp.Helper;
using WebApp.Models;

namespace WebApp.Controllers
{
	public class RolesController : Controller
	{
		private readonly RoleManager<AspNetRoles> _roleManager;
		private readonly ILogger<RolesController> _logger;
		private readonly IConfiguration _config;
		public RolesController(
			RoleManager<AspNetRoles> roleManager,
			ILogger<RolesController> logger,
			IConfiguration configuration)
		{
			this._roleManager = roleManager;
			this._logger = logger;
			this._config = configuration;
		}
		public IActionResult Show(string SearchByRoles = "")
		{
			var db = new BaseCoreDataContext();
			int pageSize = Convert.ToInt32(_config["AppSetting:PageSize"]);
			int currentPage = 1;
			var LstRoles = new List<AspNetRoles>();
			ViewBag.SearchByRoles = SearchByRoles;
			if (string.IsNullOrEmpty(SearchByRoles))
			{
				var ShowRoles = db.AspNetRoles.Where(n => n.Deleted != 1).ToList();
				LstRoles = ShowRoles.Skip(pageSize * (currentPage - 1)).Take(pageSize).ToList();
				int totalRecored = ShowRoles.Count(); //  toán tử count để đếm tổng bản ghi
				ViewBag.TotalRecored = totalRecored;
				ViewBag.TotalPage = (totalRecored / pageSize) + ((totalRecored % pageSize) > 0 ? 1 : 0);
				ViewBag.RecoredFrom = 1;
				ViewBag.CurrentPage = currentPage;
				ViewBag.RecoredTo = ViewBag.TotalPage == 1 ? totalRecored : pageSize;
			}
			else
			{
				var ShowRoles = db.AspNetRoles.ToList().Where(n => n.Name.ToLower().Contains(SearchByRoles.ToString().ToLower())).Where(n => n.Deleted != 1).ToList();
				LstRoles = ShowRoles.Skip(pageSize * (currentPage - 1)).Take(pageSize).ToList();
				int totalRecored = ShowRoles.Count(); //  toán tử count để đếm tổng bản ghi
				ViewBag.TotalRecored = totalRecored;
				ViewBag.TotalPage = (totalRecored / pageSize) + ((totalRecored % pageSize) > 0 ? 1 : 0);
				ViewBag.RecoredFrom = 1;
				ViewBag.CurrentPage = currentPage;
				ViewBag.RecoredTo = ViewBag.TotalPage == 1 ? totalRecored : pageSize;
			}
			return View("~/Views/Roles/Show.cshtml", LstRoles);
		}

		public async Task<IActionResult> RolesPaging(int currentPage = 0, string SearchByRoles = "")
		{
			var db = new BaseCoreDataContext();
			var pageSize = Convert.ToInt32(_config["AppSetting:PageSize"]);   // quy định 1 size có bao nhiêu bản ghi cấu hình trong setting
			var LstRoles = new List<AspNetRoles>();
			var totalRecored = 0;
			var result = "";
			if (string.IsNullOrEmpty(SearchByRoles))
			{
				var showRoles = db.AspNetRoles.ToList();
				LstRoles = showRoles.Skip(pageSize * (currentPage - 1)).Take(pageSize).ToList();
				totalRecored = showRoles.Count();
				ViewBag.TotalRecored = totalRecored;                                        // lấy ra tổng bản ghi
				ViewBag.TotalPage = (totalRecored / pageSize) + ((totalRecored % pageSize) > 0 ? 1 : 0);      // lấy ra tổng size page
				ViewBag.CurrentPage = currentPage;                                      // page giện tại
				ViewBag.RecoredFrom = 1;                // bắt đầu từ 1 
				ViewBag.RecoredTo = ViewBag.TotalPage == 1 ? totalRecored : pageSize;           // page tiếp theo
				ViewBag.Stt = (currentPage - 1) * pageSize;              // ID
				result = await BaseExtensions.RenderViewToStringAsync(this, "~/Views/Roles/Ajax/RolesPaging.cshtml", LstRoles);
			}
			else
			{
				var ShowRoles = db.AspNetRoles.ToList().Where(n => n.Name.ToLower().Contains(SearchByRoles.ToLower().ToString().ToLower())).Where(n => n.Deleted != 1).ToList();
				LstRoles = ShowRoles.Skip(pageSize * (currentPage - 1)).Take(pageSize).ToList();
				totalRecored = ShowRoles.Count();
				ViewBag.TotalRecored = totalRecored;
				ViewBag.TotalPage = (totalRecored / pageSize) + ((totalRecored % pageSize) > 0 ? 1 : 0);
				ViewBag.CurrentPage = currentPage;
				ViewBag.RecoredFrom = 1;
				ViewBag.RecoredTo = ViewBag.TotalPage == 1 ? totalRecored : pageSize;
				ViewBag.Stt = (currentPage - 1) * pageSize;
				result = await BaseExtensions.RenderViewToStringAsync(this, "~/Views/Roles/Ajax/RolesPaging.cshtml", LstRoles);
			}

			return Content(result);
		}


		[HttpGet]
		public IActionResult Add()
		{
			return View();
		}


		[HttpPost]
		public async Task<IActionResult> Add(AspNetRoles item)
		{
			var now = DateTime.Now;
			item.CreatedDateUtc = now;
			var result = await _roleManager.CreateAsync(item);
			if (result.Succeeded)
			{
				return RedirectToAction("Show", "Roles");
			}
			return RedirectToAction("Add", "Roles");
		}

		[HttpGet]
		public IActionResult Edit(int Id)
		{
			RolesViewModel roles = new RolesViewModel();
			var objRoles = new AspNetRoles();
			using (var db = new BaseCoreDataContext())
			{
				objRoles = db.AspNetRoles.FirstOrDefault(n => n.Id == Id);
				if (objRoles != null)
				{
					roles.Id = objRoles.Id;
					roles.Name = objRoles.Name == null ? "" : objRoles.Name;
					roles.Stt = objRoles.Stt == null ? 0 : objRoles.Stt;
				}
			}
			return View(roles);
		}

		[HttpPost]
		public async Task<IActionResult> Edit(RolesViewModel item)
		{
			try
			{
				var db = new BaseCoreDataContext();
				var now = DateTime.Now;
				var mapper = MapperConfig.InitializeAutomapperRoles();
				var empDTO2 = mapper.Map<RolesViewModel, AspNetRoles>(item);
				item.UpdatedDateUtc = now;
				Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<AspNetRoles> _role = db.AspNetRoles.Add(empDTO2);
				db.SaveChanges();
				var prid = _role.Entity.Id;
				return RedirectToAction("Show", "Roles");
			}
			catch (Exception e)
			{
				Console.WriteLine(e.InnerException.Message);
			}
			return View();
		}
	}
}
