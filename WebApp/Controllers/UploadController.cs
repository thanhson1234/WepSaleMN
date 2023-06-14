using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System.IO;
using System;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Configuration;
using System.Threading.Tasks;

namespace WebApp.Controllers
{
    public class UploadController : BaseController
    {
        private IWebHostEnvironment Environment;
        //IConfiguration _iconfiguration;
        private readonly IConfiguration _config;
        //
        // GET: /Upload/
        public UploadController(IWebHostEnvironment _environment, IConfiguration configuration, IHttpContextAccessor httpContextAccessor) 
        {
            Environment = _environment;
            _config = configuration;
           
        }

        [HttpPost] 
        public async Task<JsonResult> UploadFiles(string folder)
        {
            var now = DateTime.Now;
            var files = HttpContext.Request.Form.Files;
            if (HttpContext.Request.Form.Files.Count <= 0) return Json(new { status = false, msg = "Bạn chưa chọn file." });
            var lstFiles = new List<string>();
            var lstFilesName = new List<string>();
            var virtualPath = string.Format((_config["AppSetting:TestUrl"]).ToString() + "{0}//{1}//{2}//{3}", "img","AnhSanPham", folder, DateTime.Now.Date.ToString("ddMMyyyy"));
            string webRootPath = Environment.WebRootPath;
            var path = Path.Combine(webRootPath, virtualPath);
            // nếu file chưa tồn tại thì tạo file theo đường dẫn 
            if (!Directory.Exists(webRootPath + path))
            {
                Directory.CreateDirectory(webRootPath + path);
            }

            foreach (var fileItem in files)
            {
                string filename = fileItem.FileName;
                using (var fileStream = new FileStream(Path.Combine(webRootPath + path, filename), FileMode.Create))
                {
                    await fileItem.CopyToAsync(fileStream);
                    lstFiles.Add(string.Format("{0}\\{1}", path, filename));
                    lstFilesName.Add(filename);
                }
            }
            return Json(new { status = true, msg = "Upload thành công", files = lstFiles, names = lstFilesName });
        }
       

    }
}
