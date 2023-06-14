using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.Entities;
using WebApp.Helper;
using WebApp.Models;

namespace WebApp.Controllers
{
    [Authorize]
    public class BrandController : Controller
    {
        private readonly ILogger<BrandController> _logger;
        BaseCoreDataContext baseCoreDataContext = new BaseCoreDataContext();
        private readonly IConfiguration _config;
        public BrandController(ILogger<BrandController> logger, IConfiguration configuration)
        {
            _logger = logger;
            this._config = configuration;
        }
        public IActionResult ShowBrand(string searchByName = "")
        {
            int pageSize = Convert.ToInt32(_config["AppSetting:PageSize"]);
            int currentPage = 1;
            var LstBrand = new List<Brand>();
            ViewBag.searchByName = searchByName;
            if (string.IsNullOrEmpty(searchByName))
            {
                var ShowBrand = baseCoreDataContext.Brands.ToList();
                LstBrand = ShowBrand.Skip(pageSize * (currentPage - 1)).Take(pageSize).ToList();
                int totalRecored = ShowBrand.Count(); //  toán tử count để đếm tổng bản ghi
                ViewBag.TotalRecored = totalRecored;
                ViewBag.TotalPage = (totalRecored / pageSize) + ((totalRecored % pageSize) > 0 ? 1 : 0);
                ViewBag.RecoredFrom = 1;
                ViewBag.CurrentPage = currentPage;
                ViewBag.RecoredTo = ViewBag.TotalPage == 1 ? totalRecored : pageSize;
            }
            else
            {
                var ShowBrand = baseCoreDataContext.Brands.ToList();
                LstBrand = ShowBrand.Skip(pageSize * (currentPage - 1)).Take(pageSize).ToList().Where(n => n.Name.ToLower().Contains(searchByName.ToString().ToLower())).ToList();
                int totalRecored = ShowBrand.Count(); //  toán tử count để đếm tổng bản ghi
                ViewBag.TotalRecored = totalRecored;
                ViewBag.TotalPage = (totalRecored / pageSize) + ((totalRecored % pageSize) > 0 ? 1 : 0);
                ViewBag.RecoredFrom = 1;
                ViewBag.CurrentPage = currentPage;
                ViewBag.RecoredTo = ViewBag.TotalPage == 1 ? totalRecored : pageSize;
            }
            return View("~/Views/Brand/ShowBrand.cshtml", LstBrand);
        }
        public async Task<IActionResult> BrandPaging(int currentPage = 0, String searchByName = "")
        {
            var pageSize = Convert.ToInt32(_config["AppSetting:PageSize"]);
            var LstBrand = new List<Brand>();
            var totalRecored = 0;
            var result = "";
            if (string.IsNullOrEmpty(searchByName))
            {
                var ShowBrand = baseCoreDataContext.Brands.ToList();
                LstBrand = ShowBrand.Skip(pageSize * (currentPage - 1)).Take(pageSize).Where(n => n.Deleted != 1).ToList();
                totalRecored = ShowBrand.Count(); //  toán tử count để đếm tổng bản ghi
                ViewBag.TotalRecored = totalRecored;
                ViewBag.TotalPage = (totalRecored / pageSize) + ((totalRecored % pageSize) > 0 ? 1 : 0);
                ViewBag.RecoredFrom = 1;
                ViewBag.CurrentPage = currentPage;
                ViewBag.RecoredTo = ViewBag.TotalPage == 1 ? totalRecored : pageSize;
                ViewBag.Stt = (currentPage - 1) * pageSize;
                result = await BaseExtensions.RenderViewToStringAsync(this, "~/Views/Brand/Ajax/BrandPaging.cshtml", LstBrand);
            }
            else
            {
                var ShowBrand = baseCoreDataContext.Brands.ToList();
                LstBrand = ShowBrand.Skip(pageSize * (currentPage - 1)).Take(pageSize).ToList().Where(n => n.Name.ToLower().Contains(searchByName.ToString().ToLower())).Where(n => n.Deleted != 1).ToList();
                totalRecored = ShowBrand.Count(); //  toán tử count để đếm tổng bản ghi
                ViewBag.TotalRecored = totalRecored;
                ViewBag.TotalPage = (totalRecored / pageSize) + ((totalRecored % pageSize) > 0 ? 1 : 0);
                ViewBag.RecoredFrom = 1;
                ViewBag.CurrentPage = currentPage;
                ViewBag.RecoredTo = ViewBag.TotalPage == 1 ? totalRecored : pageSize;
                ViewBag.Stt = (currentPage - 1) * pageSize;
                result = await BaseExtensions.RenderViewToStringAsync(this, "~/Views/Brand/Ajax/BrandPaging.cshtml", LstBrand);
            }
            return Content(result);
        }

        [HttpGet]
        public IActionResult Add()
        {
            try
            {
                var lstProvince = new List<Province>();
                using (var db = new BaseCoreDataContext())
                {
                    lstProvince = db.Provinces.ToList();
                }
                ViewBag.lstProvince = lstProvince;
                return View();
            }
            catch
            {
                return View();
            }
        }
        [HttpPost]
        public IActionResult Add(BrandViewModel item)
        {
            try
            {
                var now = DateTime.Now;
                var mapper = MapperConfig.InitializeAutomapperBrand();
                var empDTO = mapper.Map<BrandViewModel, Brand>(item);
                item.Deleted = 0;
                empDTO.CreatedDateUtc = now;
                empDTO.CreatedUid = 1;
                Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<Brand> role = baseCoreDataContext.Brands.Add(empDTO);
                baseCoreDataContext.SaveChanges();
                return RedirectToAction("ShowBrand", "Brand");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException.Message);
            }
            return View();
        }
        [HttpGet]
        public IActionResult Edit(int Id)
        {
            try
            {
                BrandViewModel brand = new BrandViewModel();
                var lstProvince = new List<Province>();
                var objBrand = new Brand();
                using (var db = new BaseCoreDataContext())
                {
                    objBrand = db.Brands.FirstOrDefault(n => n.Id == Id);
                    if (objBrand != null)
                    {
                        brand.Id = objBrand.Id;
                        brand.Name = objBrand.Name == null ? "" : objBrand.Name;
                        brand.Evaluate = objBrand.Evaluate == null ? "" : objBrand.Evaluate;
                        brand.ProvinceId = objBrand.ProvinceId == 0 ? 0 : objBrand.ProvinceId;
                        brand.FoundedTime = objBrand.FoundedTime == null ? DateTime.MinValue : objBrand.FoundedTime;
                        brand.Status = objBrand.Status == null ? 0 : objBrand.Status;
                    }
                    lstProvince = db.Provinces.ToList();
                }
                ViewBag.LstProvince= lstProvince;
                    return View(brand);
            }
            catch (Exception e)
            {
                return View();
            }
        }
		[HttpPost]
		public IActionResult Edit(BrandViewModel item)
		{
			try
			{
				BaseCoreDataContext db = new BaseCoreDataContext();
				var obj = db.Brands.AsNoTracking().Where(n => n.Id == item.Id).FirstOrDefault();
				if (obj != null)
				{
					var mapper = MapperConfig.InitializeAutomapperBrand();
					var Brand = mapper.Map<BrandViewModel, Brand>(item);
					obj.Name = Brand.Name;
					obj.Evaluate = Brand.Evaluate;
					obj.ProvinceId = Brand.ProvinceId;
					obj.FoundedTime = Brand.FoundedTime;
					obj.Status = Brand.Status;
					db.Entry(obj).State = EntityState.Modified;
					db.SaveChanges();
					return RedirectToAction("ShowBrand", "Brand");
				}
				else
				{
					return RedirectToAction("Edit", "Brand", new { Id = item.Id });
				}

			}
			catch (Exception e)
			{
				return View(item);
			}
		}

        public IActionResult DeleteAll(string code)
        {
            using (var db = new BaseCoreDataContext())
            {
                if (string.IsNullOrWhiteSpace(code))
                {
                    return Json(new { name = " Bạn chưa chọn dữ liệu nào" });
                }
                else
                {
                    var strtemp = code.Split(',').ToList();
                    foreach (var item in strtemp)
                    {
                        var objBr = db.Brands.FirstOrDefault(n => n.Id == Convert.ToInt32(item));
                        if (objBr != null)
                        {
                            objBr.Deleted = 1;
                            db.Entry(objBr).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                    }
                }
                return Json(new { status = true, message = "bạn đã xóa thành công" });
            }
        }
    }
}



