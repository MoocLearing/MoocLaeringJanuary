using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Common.CommandTrees;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;
using System.Web.Mvc;
using Mooc.Data.Context;
using Mooc.Data.Entities;
using Mooc.Data.Mongo;
using Mooc.Data.ViewModels;
using Mooc.Utils;
using Mooc.Web.Models;

namespace Mooc.Web.Controllers
{
    public class UserCenterController : Controller
    {
        private readonly DataContext _dataContext;

        public UserCenterController(DataContext dataContext)
        {
            _dataContext = dataContext ?? throw new ArgumentNullException(nameof(dataContext));
        }

        // GET: UserCenter
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Person(long? id)
        {
            var currentUser = _dataContext.Users.Find(id);

            if (currentUser != null)
            {
                var model = AutoMapper.Mapper.Map<UserViewModel>(currentUser);
                return View(model);
            }

            return HttpNotFound();

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
        
        public ActionResult ShowCourse(long? id)
        {

            if (id > 0)
            {
                var appliedCourseList = _dataContext.CourseApplies.Where(x => x.UserId == id).ToList();

                HashSet<long> hs = new HashSet<long>();

                foreach (var item in appliedCourseList)
                {
                    hs.Add(item.CourseId);
                }

                List<Course> appliedCourses = new List<Course>();

                foreach (var num in hs)
                {
                  var onecourse =  _dataContext.Courses.Find(num);
                  appliedCourses.Add(onecourse);
                }

                return PartialView(appliedCourses);
            }

            return HttpNotFound();

        }

        [HttpPost]
        public ActionResult MyCourse(long id)
        {

            var list = (from a in _dataContext.CourseApplies
                    join b in _dataContext.Courses on a.CourseId equals b.ID
                    join c in _dataContext.Categorys on b.CategoryId equals c.ID
                    join d in _dataContext.Teachers on b.TeacherId equals d.ID
                    where a.UserId == id
                    
                    select new UserCenterCourseView()
                    {
                        CourseId = a.CourseId,
                        CourseDetail = b.CourseDetail,
                        CourseCover = "/Api/VideoApi/Image?id=" + b.CoverPic,
                        CourseName = b.CourseName,
                        CategoryName = c.CategoryName,
                        TeacherName = d.TeacherName,
                        StudyStatus = "正在学习"

 
                    }
                );

            return PartialView(list);
        }

        [HttpPost]
        public JsonResult GetStatus(long id)
        {
            var coursePerChapterNum = _dataContext.Chapters.Where(x => x.CourseId == id).Count();

            var studyedChapterPerCourse = _dataContext.Studies.Where(x => x.CourseId == id && x.UserId==LoginHelper.UserId).Count();

            if (studyedChapterPerCourse >= coursePerChapterNum)
            {
                return Json(new {code = 0});
            }

            return Json(new {code = 1});
        }

        public ActionResult MyDetail()
        {
            var curuser = _dataContext.Users.Find(LoginHelper.UserId);

            if (curuser != null)
            {
                ViewBag.RoleList = SelectOptions.GetRoleSelectOptions();
                ViewBag.GenderList = SelectOptions.GetGenderSelectOptions();
                var viewmodel = AutoMapper.Mapper.Map<UserViewModel>(curuser);
                return PartialView(viewmodel);
            }

            return HttpNotFound();
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
                    return Json(new { code = 0, msg = "上传成功", fileName = fileName, objectId = uploadId });
                }
                catch (Exception e)
                {
                    return Json(new { code = 1, msg = e.Message });
                }

            }

            return Json(new { code = 1, msg = "请选择图片" });

        }


        [HttpPost]
        public JsonResult AjSaveEdit(User user)
        {
            var exist = _dataContext.Users.Find(user.ID);
            if (user != null && exist != null)
            {
                //String EncryptPass(String pass)
                //{
                //    System.Security.Cryptography.MD5CryptoServiceProvider md5CSP = new System.Security.Cryptography.MD5CryptoServiceProvider();
                //    byte[] passEncrypt = System.Text.Encoding.Unicode.GetBytes(pass);
                //    byte[] resultEncrypt = md5CSP.ComputeHash(passEncrypt);
                //    string passResult = System.Text.Encoding.Unicode.GetString(resultEncrypt);
                //    return passResult;
                //}

                //处理加密
                //exist.PassWord = EncryptPass(user.PassWord);
                exist.UserName = user.UserName;
                exist.Email = user.Email;
                exist.StudentNo = user.StudentNo;
                exist.TeacherId = user.TeacherId;
                exist.Gender = user.Gender;
                exist.UserState = user.UserState;
                exist.RoleType = user.RoleType;
                exist.ImgGuid = user.ImgGuid;

                _dataContext.SaveChanges();
                return Json(new { code = 0 });


            }
            return Json(new { code = 1, msg = "出错" });

        }

    }
}

