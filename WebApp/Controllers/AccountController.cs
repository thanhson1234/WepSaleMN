using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.Text.Encodings.Web;
using System.Text;
using WebApp.Entities;
using Microsoft.AspNetCore.Authentication;

namespace WebApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<AspNetUsers> _signInManager;
        private readonly UserManager<AspNetUsers> _userManager;
        private readonly ILogger<AccountController> _logger;
        public AccountController(
            SignInManager<AspNetUsers> signInManager,
            ILogger<AccountController> logger,
            UserManager<AspNetUsers> userManager)
        {
            this._signInManager = signInManager;
            this._logger = logger;
            this._userManager = userManager;
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(AspNetUsers item)
        {
            var db = new BaseCoreDataContext();
            //if (!ModelState.IsValid)
            //{
            var obj = db.AspNetUsers.Where(n => n.UserName == item.UserName && n.Password == item.Password).FirstOrDefault();
            if (obj != null)
            {
                var useridentity = await _userManager.FindByNameAsync(item.UserName);
                if (useridentity != null)
                {
                    var result = await _signInManager.PasswordSignInAsync(useridentity, item.Password, false, true);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index", "Product");
                    }
                    else
                    {
                        return RedirectToAction("Login", "Account", new AspNetUsers { UserName = item.UserName, Password = item.Password });
                    }
                }
                else
                {
                    return RedirectToAction("Login", "Account", new AspNetUsers { UserName = item.UserName, Password = item.Password });
                }
            }
            else
            {
                return RedirectToAction("Login", "Account", new AspNetUsers { UserName = item.UserName, Password = item.Password });
            }



            //}
            //var user = new AspNetUsers { };
            //var result = await _userManager.CreateAsync(user, user.Password);
            //if (result.Succeeded)
            //{
            //    _logger.LogInformation("User created a new account with password.");

            //    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            //    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            //    var callbackUrl = Url.Page(
            //        "/Account/ConfirmEmail",
            //        pageHandler: null,
            //        values: new { area = "Identity", userId = user.Id, code = code },
            //        protocol: Request.Scheme);



            //    if (_userManager.Options.SignIn.RequireConfirmedAccount)
            //    {

            //    }
            //    else
            //    {

            //    }
            //}
            //foreach (var error in result.Errors)
            //{
            //    ModelState.AddModelError(string.Empty, error.Description);
            //}
            //return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(AspNetUsers item)
        {
            var now = DateTime.Now;
            item.CreatedDateUtc = now;
            var result = await _userManager.CreateAsync(item, item.Password); // sử lý sác nhận thông tin
            if (result.Succeeded)
            {
                _logger.LogInformation("bạn đã tạo tài khoản thành công.");
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(item);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code)); // tạo tocken cho email
                var callbackUrl = Url.Page(
                 "/Account/ConfirmEmail",
                   pageHandler: null,
                values: new { area = "Identity", userId = item.Id, code = code },
                    protocol: Request.Scheme);
                return RedirectToAction("Login", "Account");
            }
            else
            {
                return RedirectToAction("Register", "Account");
            }
           
        }

    }
}
