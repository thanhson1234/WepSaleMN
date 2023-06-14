using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.Entities;
using WebApp.Helper;
using WebApp.Models;

namespace WebApp.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        private readonly ILogger<ProductController> _logger;
        private readonly IConfiguration _config;

        BaseCoreDataContext baseCoreDataContext = new BaseCoreDataContext();
        public ProductController(ILogger<ProductController> logger, IConfiguration configuration)
        {
            _logger = logger;
            this._config = configuration;
        }

        public IActionResult Index(string searchByName = "")
        {
            int pageSize = Convert.ToInt32(_config["AppSetting:PageSize"]);
            int currentPage = 1;
            var LstProduct = new List<Product>();
            ViewBag.searchByName = searchByName;
            if (string.IsNullOrEmpty(searchByName))
            {
                var ShowProduct = baseCoreDataContext.Products.Where(n => n.Deleted != 1).OrderByDescending(n => n.Id).ToList();
                LstProduct = ShowProduct.Skip(pageSize * (currentPage - 1)).Take(pageSize).ToList();
                int totalRecored = ShowProduct.Count(); //  toán tử count để đếm tổng bản ghi
                ViewBag.TotalRecored = totalRecored;
                ViewBag.TotalPage = (totalRecored / pageSize) + ((totalRecored % pageSize) > 0 ? 1 : 0);
                ViewBag.RecoredFrom = 1;
                ViewBag.CurrentPage = currentPage;
                ViewBag.RecoredTo = ViewBag.TotalPage == 1 ? totalRecored : pageSize;
            }
            else
            {
                var ShowProduct = baseCoreDataContext.Products.ToList();
                LstProduct = ShowProduct.Skip(pageSize * (currentPage - 1)).Take(pageSize).ToList().Where(n => n.Name.ToLower().Contains(searchByName.ToString().ToLower())).Where(n => n.Deleted != 1).OrderByDescending(n => n.Id).ToList();
                int totalRecored = ShowProduct.Count(); //  toán tử count để đếm tổng bản ghi
                ViewBag.TotalRecored = totalRecored;
                ViewBag.TotalPage = (totalRecored / pageSize) + ((totalRecored % pageSize) > 0 ? 1 : 0);
                ViewBag.RecoredFrom = 1;
                ViewBag.CurrentPage = currentPage;
                ViewBag.RecoredTo = ViewBag.TotalPage == 1 ? totalRecored : pageSize;
            }
            return View("~/Views/Product/Index.cshtml", LstProduct);
        }
        public async Task<IActionResult> ProductPaging(int currentPage = 0, String searchByName = "")
        {
            var pageSize = Convert.ToInt32(_config["AppSetting:PageSize"]);
            var LstProduct = new List<Product>();
            var totalRecored = 0;
            var result = "";
            if (string.IsNullOrEmpty(searchByName))
            {
                var ShowProduct = baseCoreDataContext.Products.ToList();
                LstProduct = ShowProduct.Skip(pageSize * (currentPage - 1)).Take(pageSize).Where(n => n.Deleted != 1).OrderByDescending(n => n.Id).ToList();
                totalRecored = ShowProduct.Count(); //  toán tử count để đếm tổng bản ghi
                ViewBag.TotalRecored = totalRecored;
                ViewBag.TotalPage = (totalRecored / pageSize) + ((totalRecored % pageSize) > 0 ? 1 : 0);
                ViewBag.RecoredFrom = 1;
                ViewBag.CurrentPage = currentPage;
                ViewBag.RecoredTo = ViewBag.TotalPage == 1 ? totalRecored : pageSize;
                ViewBag.Stt = (currentPage - 1) * pageSize;
                result = await BaseExtensions.RenderViewToStringAsync(this, "~/Views/Product/Ajax/ProductPaging.cshtml", LstProduct);
            }
            else
            {

                var ShowProduct = baseCoreDataContext.Products.ToList();
                LstProduct = ShowProduct.Skip(pageSize * (currentPage - 1)).Take(pageSize).ToList().Where(n => n.Name.ToLower().Contains(searchByName.ToString().ToLower())).Where(n => n.Deleted != 1).OrderByDescending(n => n.Id).ToList();
                totalRecored = ShowProduct.Count(); //  toán tử count để đếm tổng bản ghi
                ViewBag.TotalRecored = totalRecored;
                ViewBag.TotalPage = (totalRecored / pageSize) + ((totalRecored % pageSize) > 0 ? 1 : 0);
                ViewBag.RecoredFrom = 1;
                ViewBag.CurrentPage = currentPage;
                ViewBag.RecoredTo = ViewBag.TotalPage == 1 ? totalRecored : pageSize;
                ViewBag.Stt = (currentPage - 1) * pageSize;
                result = await BaseExtensions.RenderViewToStringAsync(this, "~/Views/Product/Ajax/ProductPaging.cshtml", LstProduct);
            }
            return Content(result);
        }
        [HttpGet]
        public IActionResult Add()
        {
            try
            {
                var lstCate = new List<CategoryProduct>();
                var lstBr = new List<Brand>();
                var lstCompany = new List<Company>();
                using (var db = new BaseCoreDataContext())
                {
                    lstCate = db.CategoryProducts.ToList();
                    lstBr = db.Brands.ToList();
                    lstCompany = db.Companies.ToList();
                }
                ViewBag.Cate = lstCate;
                ViewBag.lstBr = lstBr;
                ViewBag.lstCpn = lstCompany;
                return View();
            }
            catch (Exception e)
            {
                return View();
            }

        }
        [HttpPost]
        public IActionResult Add(ProductViewModel item)
        {
            try
            {
                var now = DateTime.Now;
                var mapper = MapperConfig.InitializeAutomapper();
                var empDTO2 = mapper.Map<ProductViewModel, Product>(item);
                //item.CreatedUid = Guid.Parse(userManager.GetUserId(User));
                //ViewBag.UidName = User.Identity.Name;
                item.Deleted = 0;
                item.UpdatedDateUtc = DateTime.Now;
                var code = baseCoreDataContext.Products.Where(n => n.Code == item.Code).Count();
                if (code > 0)
                {
                    ViewBag.ErrorCode = 1;
                    ViewBag.ErrorMsg = "Mã đã tồn tại trong hệ thống";
                    return View(item);
                };
                empDTO2.Avatar = item.Avatar == null ? "" : item.Avatar;
                empDTO2.CreatedUid = 1;
                Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<Product> _role = baseCoreDataContext.Products.Add(empDTO2);
                baseCoreDataContext.SaveChanges();
                var prid = _role.Entity.Id;
                if (prid > 0)
                {
                    //lưu ảnh ở đây IMGproduct
                    return RedirectToAction("Index", "Product");
                }
                return View();
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
                ProductViewModel pr = new ProductViewModel();
                var objProduct = new Product();
                var lstImage = new List<ImgProduct>();
                var lstCate = new List<CategoryProduct>();
                var lstBr = new List<Brand>();
                var lstCompany = new List<Company>();
                using (var db = new BaseCoreDataContext())
                {
                    objProduct = db.Products.FirstOrDefault(n => n.Id == Id);
                    if (objProduct != null)
                    {
                        pr.Id = objProduct.Id;
                        pr.Quantity = objProduct.Quantity;
                        pr.Code = objProduct.Code == null ? "" : objProduct.Code;
                        pr.Name = objProduct.Name == null ? "" : objProduct.Name;
                        pr.ThuongHieu = objProduct.ThuongHieu == null ? 1 : objProduct.ThuongHieu;
                        pr.LoaiSanPham = objProduct.LoaiSanPham == null ? 1 : objProduct.LoaiSanPham;
                        pr.NhaCungCap = objProduct.NhaCungCap == null ? 1 : objProduct.NhaCungCap;
                        pr.Price = objProduct.Price == null ? 0 : objProduct.Price;
                        pr.Avatar = objProduct.Avatar == null ? "" : objProduct.Avatar;
                        pr.ShowInHome = objProduct.ShowInHome == null ? 0 : objProduct.ShowInHome;
                        lstImage = db.ImgProducts.Where(n => n.IdProduct == Id).ToList();
                        if (lstImage.Count() > 1)
                        {
                            pr.file_anh1 = lstImage[0].IMG == null ? "" : lstImage[0].IMG;
                            pr.file_anh2 = lstImage[1].IMG == null ? "" : lstImage[1].IMG;
                        }
                        lstCate = db.CategoryProducts.ToList();
                        lstBr = db.Brands.ToList();
                        lstCompany = db.Companies.ToList();
                    }
                    ViewBag.Cate = lstCate;
                    ViewBag.lstBr = lstBr;
                    ViewBag.lstCpn = lstCompany;
                }
                return View(pr);
            }
            catch (Exception e)
            {
                return View();
            }

        }

        [HttpPost]
        public IActionResult Edit(ProductViewModel item)
        {
            try
            {
                BaseCoreDataContext db = new BaseCoreDataContext();
                var obj = db.Products.AsNoTracking().Where(n => n.Id == item.Id).FirstOrDefault();
                if (obj != null)
                {
                    var mapper = MapperConfig.InitializeAutomapper();
                    var product = mapper.Map<ProductViewModel, Product>(item);
                    //product.UpdatedUid = Guid.Parse(userManager.GetUserId(User));
                    obj.LoaiSanPham = product.LoaiSanPham;
                    obj.Price = product.Price;
                    obj.Quantity = product.Quantity;
                    obj.UpdatedDateUtc = DateTime.Now;
                    obj.ShowInHome = product.ShowInHome;
                    db.Entry(obj).State = EntityState.Modified;
                    db.SaveChanges();
                    //continue image
                    return RedirectToAction("Index", "Product");
                }
                else
                {
                    return RedirectToAction("Edit", "Product", new { Id = item.Id });
                }

            }
            catch (Exception e)
            {
                return View(item);
            }

        }

        public IActionResult Delete(int Id)
        {
            try
            {
                using (var db = new BaseCoreDataContext())
                {
                    var DeleteId = db.Products.FirstOrDefault(n => n.Id == Id);
                    if (DeleteId != null)
                    {
                        DeleteId.Deleted = 1;
                        db.Entry(DeleteId).State = EntityState.Modified;
                        db.SaveChanges();
						
                    }
                }
				return Json(new { status = true, message = "Bạn đã xóa thành công" });
			}
            catch(Exception e)
            {
				return Json(new { status = true, message = "Bạn đã xóa thất bại" });
			}
        }


        public IActionResult DeleteAll(string code)
        {
            var db = new BaseCoreDataContext();
            if (string.IsNullOrWhiteSpace(code))
            {
                return Json(new { name = "Bạn chưa chọn dữ liệu cần xóa!" });
            }
            else
            {
                var strtemp = code.Split(',').ToList();
                foreach (var item in strtemp)
                {
                    var objProduct = db.Products.FirstOrDefault(n => n.Id == Convert.ToInt32(item));
                    if (objProduct != null)
                    {
                        objProduct.Deleted = 1;
                        db.Entry(objProduct).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                }
                return Json(new { status = true, message = "Bạn đã xóa thành công" });
            }

        }
    }

}
