using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;
using WebApp.Entities;
using WebApp.Helper;
using WebApp.Models;

namespace WebApp.Controllers
{
    [Authorize]
    public class UnitsController : Controller
	{
		private readonly ILogger<UnitsController> _logger;
		private readonly IConfiguration _config;
		public UnitsController(ILogger<UnitsController> logger, IConfiguration configuration)
		{
			_logger = logger;
			this._config = configuration;
		}
		public IActionResult Show(string search = "")
		{
			try
			{
				int pageSize = Convert.ToInt32(_config["AppSetting:PageSize"]);
				int currentPage = 1;
				var LstUnits = new List<Units>();
				ViewBag.search = search;
				BaseCoreDataContext db = new BaseCoreDataContext();
				if (string.IsNullOrEmpty(search))
				{
					var ShowUnits = db.Units.Where(n => n.Deleted != 1).ToList();
					LstUnits = ShowUnits.Skip(pageSize * (currentPage - 1)).Take(pageSize).ToList();
					int totalRecored = ShowUnits.Count(); //  toán tử count để đếm tổng bản ghi
					ViewBag.TotalRecored = totalRecored;
					ViewBag.TotalPage = (totalRecored / pageSize) + ((totalRecored % pageSize) > 0 ? 1 : 0);
					ViewBag.RecoredFrom = 1;
					ViewBag.CurrentPage = currentPage;
					ViewBag.RecoredTo = ViewBag.TotalPage == 1 ? totalRecored : pageSize;
				}
				else
				{
					var ShowUnits = db.Units.ToList();
					LstUnits = ShowUnits.Skip(pageSize * (currentPage - 1)).Take(pageSize).ToList().Where(n => n.Name.ToLower().Contains(search.ToString().ToLower())).ToList();
					int totalRecored = ShowUnits.Count(); //  toán tử count để đếm tổng bản ghi
					ViewBag.TotalRecored = totalRecored;
					ViewBag.TotalPage = (totalRecored / pageSize) + ((totalRecored % pageSize) > 0 ? 1 : 0);
					ViewBag.RecoredFrom = 1;
					ViewBag.CurrentPage = currentPage;
					ViewBag.RecoredTo = ViewBag.TotalPage == 1 ? totalRecored : pageSize;
				}
				return View("~/Views/Units/Show.cshtml", LstUnits);
			}
			catch (Exception e)
			{
				return View();
			}


		}

		public async Task<IActionResult> UnitsPaging(int currentPage = 0, String search = "")
		{
			BaseCoreDataContext db = new BaseCoreDataContext();
			var pageSize = Convert.ToInt32(_config["AppSetting:PageSize"]);
			var LstUnit = new List<Units>();
			var totalRecored = 0;
			var result = "";
			if (string.IsNullOrEmpty(search))
			{
				var ShowUnits = db.Units.ToList();
				LstUnit = ShowUnits.Skip(pageSize * (currentPage - 1)).Take(pageSize).Where(n => n.Deleted != 1).ToList();
				totalRecored = ShowUnits.Count(); //  toán tử count để đếm tổng bản ghi
				ViewBag.TotalRecored = totalRecored;
				ViewBag.TotalPage = (totalRecored / pageSize) + ((totalRecored % pageSize) > 0 ? 1 : 0);
				ViewBag.RecoredFrom = 1;
				ViewBag.CurrentPage = currentPage;
				ViewBag.RecoredTo = ViewBag.TotalPage == 1 ? totalRecored : pageSize;
				ViewBag.Stt = (currentPage - 1) * pageSize;
				result = await BaseExtensions.RenderViewToStringAsync(this, "~/Views/Units/Ajax/UnitsPaging.cshtml", LstUnit);
			}
			else
			{
				var ShowUnits = db.Units.ToList();
				LstUnit = ShowUnits.Skip(pageSize * (currentPage - 1)).Take(pageSize).ToList().Where(n => n.Name.ToLower().Contains(search.ToLower().ToString().ToLower())).ToList();
				totalRecored = ShowUnits.Count(); //  toán tử count để đếm tổng bản ghi
				ViewBag.TotalRecored = totalRecored;
				ViewBag.TotalPage = (totalRecored / pageSize) + ((totalRecored % pageSize) > 0 ? 1 : 0);
				ViewBag.RecoredFrom = 1;
				ViewBag.CurrentPage = currentPage;
				ViewBag.RecoredTo = ViewBag.TotalPage == 1 ? totalRecored : pageSize;
				ViewBag.Stt = (currentPage - 1) * pageSize;
				result = await BaseExtensions.RenderViewToStringAsync(this, "~/Views/Category/Ajax/UnitsPaging.cshtml", LstUnit);
			}
			return Content(result);
		}

		[HttpGet]
		public IActionResult Add()
		{
			try
			{
				var lstPr = new List<Product>();
				using (var db = new BaseCoreDataContext())
				{
					lstPr = db.Products.ToList();
				}
				ViewBag.lstproduct = lstPr;
				return View();
			}
			catch (Exception e)
			{

			}
			return View();
		}

		[HttpPost]

		public IActionResult Add(UnitsViewModel item)
		{
			try
			{
				BaseCoreDataContext db = new BaseCoreDataContext();
				var now = DateTime.Now;
				var mapper = MapperConfig.InitializeAutomapperUnits();
				var empDTO = mapper.Map<UnitsViewModel, Units>(item);
				item.Deleted = 0;
				empDTO.CreatedDateUtc = now;
				empDTO.CreatedUid = 1;
				Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<Units> _role = db.Units.Add(empDTO);
				db.SaveChanges();
				return RedirectToAction("Show", "Units");
			}

			catch (Exception e)
			{
				return View(item);
			}
		}

		[HttpGet]
		public IActionResult Edit(int Id)
		{
			try
			{
				UnitsViewModel Unit = new UnitsViewModel();
                var lstPr = new List<Product>();
                var ObjUnit = new Units();
				using (var db = new BaseCoreDataContext())
				{
					ObjUnit = db.Units.FirstOrDefault(n=>n.Id == Id);
					if (ObjUnit != null)
					{
						Unit.Id = ObjUnit.Id;
						Unit.Name = ObjUnit.Name== null ?"" : ObjUnit.Name;
						Unit.Quantity = ObjUnit.Quantity== null ? 0 : ObjUnit.Quantity;
						Unit.Status = ObjUnit.Status== null ? 0 : ObjUnit.Status;
						Unit.ProductId = ObjUnit.ProductId== null ? 0 : ObjUnit.ProductId;

					}
                    lstPr = db.Products.ToList();
                }
                ViewBag.lstproduct = lstPr;
                return View(Unit);
			}
			catch(Exception e)
			{
				Console.WriteLine(e.InnerException.Message);
			}
			return View();

		}

		[HttpPost]
		public IActionResult Edit( UnitsViewModel item)
		{
			try
			{
				using ( var db = new BaseCoreDataContext())
				{
					var obj = db.Units.AsNoTracking().Where(n=>n.Id == item.Id).FirstOrDefault();
					if (obj != null)
					{
						var mapper = MapperConfig.InitializeAutomapperUnits();
						var Dto = mapper.Map<UnitsViewModel, Units>(item);
						obj.Name = Dto.Name;
						obj.Quantity = Dto.Quantity;
						obj.Status = Dto.Status;
						obj.ProductId = Dto.ProductId;
						obj.UpdatedDateUtc = DateTime.Now;
                        db.Entry(obj).State = EntityState.Modified;
                        db.SaveChanges();
						return RedirectToAction("Show", "Units");
					}
					else
					{
                        return RedirectToAction("Edit", "Units" , new { Id = item.Id});
                    }
				}
			}
			catch
			{
				return View(item);
			}
		}

		public IActionResult DeleteAll(string code)
		{
			using ( var db = new BaseCoreDataContext())
			{
				if(string.IsNullOrWhiteSpace(code))
				{
					return Json(new { name = " Bạn chưa chọn dữ liệu" });
				}
				else
				{
					var stremp = code.Split(',').ToList();
					foreach (var item in stremp)
					{
						var objUnits = db.Units.FirstOrDefault(n=>n.Id == Convert.ToInt32(item));
						if(objUnits != null)
						{
							objUnits.Deleted = 1;
                            db.Entry(objUnits).State = EntityState.Modified;
                            db.SaveChanges();
                        }
					}
				}
			}
            return Json(new { status = true, message = "bạn đã xóa thành công" });
        }

    }
}
