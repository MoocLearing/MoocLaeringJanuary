using Mooc.Data.Context;
using Mooc.Data.Entities;
using Mooc.Data.Mongo;
using Mooc.Utils;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mooc.Web.Controllers
{
    public class RegisterController : Controller
    {

        private readonly DataContext _dataContext;

        public RegisterController(DataContext data)
        {
            _dataContext = data;
        }
        // GET: Register
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Reg()
        {
            return View();
        }

        public ActionResult Show(string filename)
        {
            try
            {
                if (string.IsNullOrEmpty(filename))
                    return Content("参数错误");

                var bytes = MongoDBHelper.down(filename);
                if (bytes == null || bytes.Length == 0)
                    return Content("图片不存在");
                return File(bytes, "image/jpeg", filename);
            }
            catch (Exception e)
            {
                return Content("出错：" + e.Message);
            }
        }

        [HttpPost]
        public JsonResult Save(User user)
        {
            if (user != null)
            {
                user.PassWord = MD5Help.MD5Encoding(user.PassWord, ConfigurationManager.AppSettings["sKey"].ToString());
                _dataContext.Users.Add(user);
                _dataContext.SaveChanges();
                return Json(new { code = 0 });
            }
            return Json(new { code = 1, msg = "出错" });

        }


        [HttpPost]
        public JsonResult UploadImg()
        {
            HttpFileCollection Files = System.Web.HttpContext.Current.Request.Files;
            if (Files.Count > 0)
            {
                try
                {
                    //多个for循环
                    HttpPostedFile file = Files[0];
                    string fileExtension = Path.GetExtension(file.FileName);
                    string[] filetype = { ".jpg", ".jpeg", ".gif", ".png" }; //文件允许格式jpg、jpeg、gif、png
                    bool checkType = Array.IndexOf(filetype, fileExtension) == -1;
                    if (checkType)
                    {
                        return Json(new { code = 1, msg = "图片格式错误" });
                    }

                    if (file.ContentLength >= 50 * 1024 * 1024)
                    {
                        return Json(new { code = 1, msg = "上传视频大小不能超过50MB" });

                    }

                    string fileName = $"v_{Guid.NewGuid().ToString("N")}{fileExtension}";

                    string uploadId = MongoDBHelper.Upload(fileName, file.InputStream);
                    if (string.IsNullOrEmpty(uploadId))
                        return Json(new { code = 1, msg = "图片不存在" });
                    return Json(new { code = 0, msg = "上传成功", fileName, objectId = uploadId });
                }
                catch (Exception e)
                {
                    return Json(new { code = 1, msg = e.Message });
                }

            }

            return Json(new { code = 1, msg = "请选择图片" });

        }
    }
}