using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.Entities;
using WebApp.Helper;
using WebApp.Models;

namespace WebApp.Controllers
{
    [Authorize]
    public class CategoryController : Controller
    {
        private readonly ILogger<CategoryController> _logger;
        private readonly IConfiguration _config;
        BaseCoreDataContext baseCoreDataContext = new BaseCoreDataContext();
        public CategoryController(ILogger<CategoryController> logger, IConfiguration configuration)
        {
            _logger = logger;
            this._config = configuration;
        }
        public IActionResult ShowCategory(string SearchByName = "")
        {
            int pageSize = Convert.ToInt32(_config["AppSetting:PageSize"]);
            int currentPage = 1;
            var LstCategorys = new List<CategoryProduct>();
            ViewBag.SearchByName = SearchByName;
            if (string.IsNullOrEmpty(SearchByName))
            {
                var ShowCategory = baseCoreDataContext.CategoryProducts.Where(n => n.Deleted != 1).ToList();
                LstCategorys = ShowCategory.Skip(pageSize * (currentPage - 1)).Take(pageSize).ToList();
                int totalRecored = ShowCategory.Count(); //  toán tử count để đếm tổng bản ghi
                ViewBag.TotalRecored = totalRecored;
                ViewBag.TotalPage = (totalRecored / pageSize) + ((totalRecored % pageSize) > 0 ? 1 : 0);
                ViewBag.RecoredFrom = 1;
                ViewBag.CurrentPage = currentPage;
                ViewBag.RecoredTo = ViewBag.TotalPage == 1 ? totalRecored : pageSize;
            }
            else
            {
                var ShowCategory = baseCoreDataContext.CategoryProducts.ToList().Where(n => n.Name.ToLower().Contains(SearchByName.ToString().ToLower())).Where(n => n.Deleted != 1).ToList();
                LstCategorys = ShowCategory.Skip(pageSize * (currentPage - 1)).Take(pageSize).ToList();
                int totalRecored = ShowCategory.Count(); //  toán tử count để đếm tổng bản ghi
                ViewBag.TotalRecored = totalRecored;
                ViewBag.TotalPage = (totalRecored / pageSize) + ((totalRecored % pageSize) > 0 ? 1 : 0);
                ViewBag.RecoredFrom = 1;
                ViewBag.CurrentPage = currentPage;
                ViewBag.RecoredTo = ViewBag.TotalPage == 1 ? totalRecored : pageSize;
            }
            return View("~/Views/Category/ShowCategory.cshtml", LstCategorys);
        }
        public async Task<IActionResult> CategoryPaging(int currentPage = 0, String SearchByName = "")
        {
            var pageSize = Convert.ToInt32(_config["AppSetting:PageSize"]);
            var LstCategorys = new List<CategoryProduct>();
            var totalRecored = 0;
            var result = "";
            if (string.IsNullOrEmpty(SearchByName))
            {
                var ShowCategory = baseCoreDataContext.CategoryProducts.ToList();
                LstCategorys = ShowCategory.Skip(pageSize * (currentPage - 1)).Take(pageSize).Where(n => n.Deleted != 1).ToList();
                totalRecored = ShowCategory.Count(); //  toán tử count để đếm tổng bản ghi
                ViewBag.TotalRecored = totalRecored;
                ViewBag.TotalPage = (totalRecored / pageSize) + ((totalRecored % pageSize) > 0 ? 1 : 0);
                ViewBag.RecoredFrom = 1;
                ViewBag.CurrentPage = currentPage;
                ViewBag.RecoredTo = ViewBag.TotalPage == 1 ? totalRecored : pageSize;
                ViewBag.Stt = (currentPage - 1) * pageSize;
                result = await BaseExtensions.RenderViewToStringAsync(this, "~/Views/Category/Ajax/CategoryPaging.cshtml", LstCategorys);
            }
            else
            {
                var ShowCategory = baseCoreDataContext.CategoryProducts.ToList();
                LstCategorys = ShowCategory.Skip(pageSize * (currentPage - 1)).Take(pageSize).ToList().Where(n => n.Name.ToLower().Contains(SearchByName.ToLower().ToString().ToLower())).ToList();
                totalRecored = ShowCategory.Count(); //  toán tử count để đếm tổng bản ghi
                ViewBag.TotalRecored = totalRecored;
                ViewBag.TotalPage = (totalRecored / pageSize) + ((totalRecored % pageSize) > 0 ? 1 : 0);
                ViewBag.RecoredFrom = 1;
                ViewBag.CurrentPage = currentPage;
                ViewBag.RecoredTo = ViewBag.TotalPage == 1 ? totalRecored : pageSize;
                ViewBag.Stt = (currentPage - 1) * pageSize;
                result = await BaseExtensions.RenderViewToStringAsync(this, "~/Views/Category/Ajax/CategoryPaging.cshtml", LstCategorys);
            }
            return Content(result);
        }
        [HttpGet]
        public IActionResult Add()
        {

            return View();

        }
        [HttpPost]
        public IActionResult Add(CateViewModel item)
        {
            try
            {
                var now = DateTime.Now;
                var mapper = MapperConfig.InitializeAutomapperCate();
                var empDTO2 = mapper.Map<CateViewModel, CategoryProduct>(item);
                //item.CreatedUid = Guid.Parse(userManager.GetUserId(User));
                //ViewBag.UidName = User.Identity.Name;
                item.Deleted = 0;
                empDTO2.CreatedDateUtc = now;
                empDTO2.CreatedUid = 1;
                Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<CategoryProduct> _role = baseCoreDataContext.CategoryProducts.Add(empDTO2);
                baseCoreDataContext.SaveChanges();
                var prid = _role.Entity.Id;
                return RedirectToAction("ShowCategory", "Category");
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
                CateViewModel cate = new CateViewModel();
                var objCate = new CategoryProduct();
                using (var db = new BaseCoreDataContext())
                {
                    objCate = db.CategoryProducts.FirstOrDefault(n => n.Id == Id);
                    if (objCate != null)
                    {
                        cate.Id = objCate.Id;
                        cate.Name = objCate.Name == null ? "" : objCate.Name;
                        cate.Icon = objCate.Icon == null ? "" : objCate.Icon;
                        cate.Status = objCate.Status == null ? 0 : objCate.Status;
                    }
                }
                return View(cate);
            }
            catch
            {
                return View();
            }
        }

        [HttpPost]
        public IActionResult Edit(CateViewModel item)
        {
            try
            {
                BaseCoreDataContext db = new BaseCoreDataContext();
                var obj = db.CategoryProducts.AsNoTracking().Where(n => n.Id == item.Id).FirstOrDefault();
                if (obj != null)
                {
                    var mapper = MapperConfig.InitializeAutomapperCate();
                    var Cate = mapper.Map<CateViewModel, CategoryProduct>(item);
                    obj.Name = Cate.Name;
                    obj.Icon = Cate.Icon;
                    obj.Status = Cate.Status;
                    db.Entry(obj).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("ShowCategory", "Category");
                }
                else
                {
                    return RedirectToAction("Edit", "Category", new { Id = item.Id });
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
                        var objCate = db.CategoryProducts.FirstOrDefault(n => n.Id == Convert.ToInt32(item));
                        if (objCate != null)
                        {
                            objCate.Deleted = 1;
                            db.Entry(objCate).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                    }
                }
                return Json(new { status = true, message = "bạn đã xóa thành công" });
            }
        }

    }
}
