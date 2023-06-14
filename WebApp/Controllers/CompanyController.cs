using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.Entities;
using WebApp.Helper;
using WebApp.Models;

namespace WebApp.Controllers
{
    [Authorize]
    public class CompanyController : Controller
    {
        private readonly ILogger<CompanyController> _logger;
        private readonly IConfiguration _config;
        BaseCoreDataContext baseCoreDataContext = new BaseCoreDataContext();
        public CompanyController(ILogger<CompanyController> logger, IConfiguration configuration)
        {
            _logger = logger;
            this._config = configuration;
        }

        public IActionResult Index(string searchByName = "")
        {
            int pageSize = Convert.ToInt32(_config["AppSetting:PageSize"]);
            int currentPage = 1;
            var lstCompanies = new List<Company>();
            ViewBag.searchByName = searchByName;
            if (string.IsNullOrEmpty(searchByName))
            {
                var ShowCompany = baseCoreDataContext.Companies.Where(n => n.Deleted != 1).ToList();
                lstCompanies = ShowCompany.Skip(pageSize * (currentPage - 1)).Take(pageSize).ToList();
                int totalRecored = ShowCompany.Count();
                ViewBag.TotalRecored = totalRecored;
                ViewBag.TotalPage = (totalRecored / pageSize) + ((totalRecored % pageSize) > 0 ? 1 : 0);
                ViewBag.CurrentPage = currentPage;
                ViewBag.RecoredFrom = 1;
                ViewBag.RecoredTo = ViewBag.TotalPage == 1 ? totalRecored : pageSize;
            }
            else
            {
                var ShowCompany = baseCoreDataContext.Companies.ToList().Where(n => n.Name.ToLower().Contains(searchByName.ToLower().ToString().ToLower())).Where(n => n.Deleted != 1).ToList();
                lstCompanies = ShowCompany.Skip(pageSize * (currentPage - 1)).Take(pageSize).ToList();
                int totalRecored = ShowCompany.Count();
                ViewBag.TotalRecored = totalRecored;
                ViewBag.TotalPage = (totalRecored / pageSize) + ((totalRecored % pageSize) > 0 ? 1 : 0);
                ViewBag.CurrentPage = currentPage;
                ViewBag.RecoredFrom = 1;
                ViewBag.RecoredTo = ViewBag.TotalPage == 1 ? totalRecored : pageSize;
            }
            return View("~/Views/Company/ShowCompany.cshtml", lstCompanies);
        }
        public async Task<IActionResult> CompanyPaging(int currentPage = 0, string searchByName = "")
        {
            var pageSize = Convert.ToInt32(_config["AppSetting:PageSize"]);   // quy định 1 size có bao nhiêu bản ghi cấu hình trong setting
            var lstCompanies = new List<Company>();
            var totalRecored = 0;
            var result = "";
            if (string.IsNullOrEmpty(searchByName))
            {
                var ShowCompany = baseCoreDataContext.Companies.ToList();
                lstCompanies = ShowCompany.Skip(pageSize * (currentPage - 1)).Take(pageSize).ToList();
                totalRecored = ShowCompany.Count();
                ViewBag.TotalRecored = totalRecored;                                        // lấy ra tổng bản ghi
                ViewBag.TotalPage = (totalRecored / pageSize) + ((totalRecored % pageSize) > 0 ? 1 : 0);      // lấy ra tổng size page
                ViewBag.CurrentPage = currentPage;                                      // page giện tại
                ViewBag.RecoredFrom = 1;                // bắt đầu từ 1 
                ViewBag.RecoredTo = ViewBag.TotalPage == 1 ? totalRecored : pageSize;           // page tiếp theo
                ViewBag.Stt = (currentPage - 1) * pageSize;              // ID
                result = await BaseExtensions.RenderViewToStringAsync(this, "~/Views/Company/Ajax/CompanyPaging.cshtml", lstCompanies);
            }
            else
            {
                var ShowCompany = baseCoreDataContext.Companies.ToList().Where(n => n.Name.ToLower().Contains(searchByName.ToLower().ToString().ToLower())).Where(n => n.Deleted != 1).ToList();
                lstCompanies = ShowCompany.Skip(pageSize * (currentPage - 1)).Take(pageSize).ToList();
                totalRecored = ShowCompany.Count();
                ViewBag.TotalRecored = totalRecored;
                ViewBag.TotalPage = (totalRecored / pageSize) + ((totalRecored % pageSize) > 0 ? 1 : 0);
                ViewBag.CurrentPage = currentPage;
                ViewBag.RecoredFrom = 1;
                ViewBag.RecoredTo = ViewBag.TotalPage == 1 ? totalRecored : pageSize;
                ViewBag.Stt = (currentPage - 1) * pageSize;
                result = await BaseExtensions.RenderViewToStringAsync(this, "~/Views/Company/Ajax/CompanyPaging.cshtml", lstCompanies);
            }

            return Content(result);
        }
        [HttpGet]
        public IActionResult Add()
        {
            try
            {
                var lstProvice = new List<Province>();
                using (var db = new BaseCoreDataContext())
                {
                    lstProvice = db.Provinces.ToList();
                }
                ViewBag.LstProvice = lstProvice;
                return View();
            }
            catch
            {

            }
            return View();
        }
        [HttpPost]
        public IActionResult Add(CompanyViewModel item)
        {
            try
            {
                var now = DateTime.Now;
                var mapper = MapperConfig.InitializeAutomapperCompany();
                var empDTO2 = mapper.Map<CompanyViewModel, Company>(item);
                //item.CreatedUid = Guid.Parse(userManager.GetUserId(User));
                //ViewBag.UidName = User.Identity.Name;
                item.Deleted = 0;
                empDTO2.CreatedDateUtc = DateTime.Now;
                empDTO2.CreatedUid = 1;
                Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<Company> _role = baseCoreDataContext.Companies.Add(empDTO2);
                baseCoreDataContext.SaveChanges();
                return RedirectToAction("Index", "Company");
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
                CompanyViewModel Cpn = new CompanyViewModel();
                var lstProvice = new List<Province>();
                var objCompany = new Company();
                using (var db = new BaseCoreDataContext())
                {
                    objCompany = db.Companies.FirstOrDefault(n => n.Id == Id);
                    if (objCompany != null)
                    {
                        Cpn.Id = objCompany.Id;
                        Cpn.Name = objCompany.Name == null ? "" : objCompany.Name;
                        Cpn.Evaluate = objCompany.Evaluate == null ? "" : objCompany.Evaluate;
                        Cpn.ProvinceId = objCompany.ProvinceId == null ? 0 : objCompany.ProvinceId;
                        Cpn.FoundedTime = objCompany.FoundedTime == null ? DateTime.MinValue : objCompany.FoundedTime;
                        Cpn.Status = objCompany.Status == null ? 0 : objCompany.Status;
                        Cpn.Avatar = objCompany.Avatar == null ? "" : objCompany.Avatar;
                    }
                    lstProvice = db.Provinces.ToList();
                }
                ViewBag.LstProvice = lstProvice;
                return View(Cpn);
            }
            catch
            {
                return View();
            }
        }

        [HttpPost]
        public IActionResult Edit(CompanyViewModel item)
        {
            try
            {
                BaseCoreDataContext db = new BaseCoreDataContext();
                var obj = db.Companies.AsNoTracking().Where(n => n.Id == item.Id).FirstOrDefault();
                if (obj != null)
                {
                    var mapper = MapperConfig.InitializeAutomapperCompany();
                    var Company = mapper.Map<CompanyViewModel, Company>(item);
                    obj.Name = Company.Name;
                    obj.Evaluate = Company.Evaluate;
                    obj.ProvinceId = Company.ProvinceId;
                    obj.FoundedTime = Company.FoundedTime;
                    obj.Status = Company.Status;
                    obj.Avatar = Company.Avatar;
                    db.Entry(obj).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index", "Company");
                }
                else
                {
                    return RedirectToAction("Edit", "Company", new { Id = item.Id });
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
                 var strtemp = code.Split(',') .ToList();
                    foreach (var item in strtemp)
                    {
                        var objCPN = db.Companies.FirstOrDefault(n => n.Id == Convert.ToInt32(item));
                        if (objCPN != null)
                        {
                            objCPN.Deleted = 1;
                            db.Entry(objCPN).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                    }
                }
                return Json(new { status = true, message = "bạn đã xóa thành công" });
            }
        }
    }
}



