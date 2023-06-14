using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.Entities;
using WebApp.Helper;
using WebApp.Models;

namespace WebApp.Controllers
{
	public class UserController : Controller
	{
		private readonly ILogger<UserController> _logger;
		private readonly IConfiguration _config;
		public UserController(ILogger<UserController> logger, IConfiguration configuration)
		{
			_logger = logger;
			this._config = configuration;
		}
		public IActionResult Show( string searchByUserName = "")
		{
			int pageSize = Convert.ToInt32(_config["AppSetting:PageSize"]);
			int currentPage = 1;
			var lstUser = new List<AspNetUsers>();
			var db = new BaseCoreDataContext();
			ViewBag.searchByUserName = searchByUserName;
			if (string.IsNullOrEmpty(searchByUserName))
			{
				var ShowUser = db.AspNetUsers.Where(n => n.DeletedBy != 1).ToList();
				lstUser = ShowUser.Skip(pageSize * (currentPage - 1)).Take(pageSize).ToList();
				int totalRecored = ShowUser.Count();
				ViewBag.TotalRecored = totalRecored;
				ViewBag.TotalPage = (totalRecored / pageSize) + ((totalRecored % pageSize) > 0 ? 1 : 0);
				ViewBag.CurrentPage = currentPage;
				ViewBag.RecoredFrom = 1;
				ViewBag.RecoredTo = ViewBag.TotalPage == 1 ? totalRecored : pageSize;
			}
			else
			{
				var ShowUser = db.AspNetUsers.ToList().Where(n => n.UserName.ToLower().Contains(searchByUserName.ToLower().ToString().ToLower())).Where(n => n.DeletedBy != 1).ToList();
				lstUser = ShowUser.Skip(pageSize * (currentPage - 1)).Take(pageSize).ToList();
				int totalRecored = ShowUser.Count();
				ViewBag.TotalRecored = totalRecored;
				ViewBag.TotalPage = (totalRecored / pageSize) + ((totalRecored % pageSize) > 0 ? 1 : 0);
				ViewBag.CurrentPage = currentPage;
				ViewBag.RecoredFrom = 1;
				ViewBag.RecoredTo = ViewBag.TotalPage == 1 ? totalRecored : pageSize;
			}
			return View("~/Views/User/Show.cshtml", lstUser);
		
		}

        public async Task<IActionResult> UserPaging(int currentPage = 0, String searchByUserName = "")
        {
            BaseCoreDataContext db = new BaseCoreDataContext();
            var pageSize = Convert.ToInt32(_config["AppSetting:PageSize"]);
            var LstUser = new List<AspNetUsers>();	
            var totalRecored = 0;
            var result = "";
            if (string.IsNullOrEmpty(searchByUserName))
            {
                var ShowUser = db.AspNetUsers.ToList();
                LstUser = ShowUser.Skip(pageSize * (currentPage - 1)).Take(pageSize).Where(n => n.DeletedBy != 1).ToList();
                totalRecored = ShowUser.Count(); //  toán tử count để đếm tổng bản ghi
                ViewBag.TotalRecored = totalRecored;
                ViewBag.TotalPage = (totalRecored / pageSize) + ((totalRecored % pageSize) > 0 ? 1 : 0);
                ViewBag.RecoredFrom = 1;
                ViewBag.CurrentPage = currentPage;
                ViewBag.RecoredTo = ViewBag.TotalPage == 1 ? totalRecored : pageSize;
                ViewBag.Stt = (currentPage - 1) * pageSize;
                result = await BaseExtensions.RenderViewToStringAsync(this, "~/Views/User/Ajax/UserPaging.cshtml", LstUser);
            }
            else
            {
                var ShowUnits = db.AspNetUsers.ToList();
                LstUser = ShowUnits.Skip(pageSize * (currentPage - 1)).Take(pageSize).ToList().Where(n => n.UserName.ToLower().Contains(searchByUserName.ToLower().ToString().ToLower())).ToList();
                totalRecored = ShowUnits.Count(); //  toán tử count để đếm tổng bản ghi
                ViewBag.TotalRecored = totalRecored;
                ViewBag.TotalPage = (totalRecored / pageSize) + ((totalRecored % pageSize) > 0 ? 1 : 0);
                ViewBag.RecoredFrom = 1;
                ViewBag.CurrentPage = currentPage;
                ViewBag.RecoredTo = ViewBag.TotalPage == 1 ? totalRecored : pageSize;
                ViewBag.Stt = (currentPage - 1) * pageSize;
                result = await BaseExtensions.RenderViewToStringAsync(this, "~/Views/User/Ajax/UserPaging.cshtml", LstUser);
            }
            return Content(result);
        }
		

        [HttpGet]
        public IActionResult Edit(int Id)
        {
            try
            {
                UserViewModel User = new UserViewModel();
                var ObjUser = new AspNetUsers();
                using (var db = new BaseCoreDataContext())
                {
                    ObjUser = db.AspNetUsers.FirstOrDefault(n => n.Id == Id);
                    if (ObjUser != null)
                    {
                        User.Id = ObjUser.Id;
                        User.UserName = ObjUser.UserName == null ? "" : ObjUser.UserName;
                        User.Email = ObjUser.Email == null ? "" : ObjUser.Email;
                        User.Status = ObjUser.Status == null ? 0 : ObjUser.Status;
                        User.Avatar = ObjUser.Avatar == null ? "" : ObjUser.Avatar;

					}
              
                }
                return View(User);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException.Message);
            }
            return View();

        }

        [HttpPost]
        public IActionResult Edit(UserViewModel item)
        {
			{
				try
				{
					var db = new BaseCoreDataContext();
					var obj = db.AspNetUsers.AsNoTracking().Where(n => n.Id == item.Id).FirstOrDefault();
					if (obj != null)
					{
						var now = DateTime.Now;
						var mapper = MapperConfig.InitializeAutomapperUser();
						var User = mapper.Map<UserViewModel, AspNetUsers>(item);
						obj.UserName = User.UserName;
						obj.Email = User.Email;
						obj.Status = User.Status;
						obj.Avatar = User.Avatar;
						obj.UpdatedDateUtc = now;

						db.Entry(obj).State = EntityState.Modified;
						db.SaveChanges();
						return RedirectToAction("Show", "User");
					}
					else
					{
						return RedirectToAction("Edit", "User", new { Id = item.Id });
					}

				}
				catch (Exception e)
				{
					return View(item);
				}
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
						var objUser = db.AspNetUsers.FirstOrDefault(n => n.Id == Convert.ToInt32(item));
						if (objUser != null)
						{
							objUser.DeletedBy = 1;
							db.Entry(objUser).State = EntityState.Modified;
							db.SaveChanges();
						}
					}
				}
				return Json(new { status = true, message = "bạn đã xóa thành công" });
			}
		}

		public IActionResult Delete(int Id)
		{
			try
			{
				using (var db = new BaseCoreDataContext())
				{
					var DeleteId = db.AspNetUsers.FirstOrDefault(n => n.Id == Id);
					if (DeleteId != null)
					{
						DeleteId.DeletedBy = 1;
						db.Entry(DeleteId).State = EntityState.Modified;
						db.SaveChanges();

					}
				}
				return Json(new { status = true, message = "Bạn đã xóa thành công" });
			}
			catch (Exception e)
			{
				return Json(new { status = true, message = "Bạn đã xóa thất bại" });
			}
		}
	}
}
