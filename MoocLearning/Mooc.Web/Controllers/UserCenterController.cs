using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Core.Common.CommandTrees;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using Mooc.Data.Context;
using Mooc.Data.Entities;
using Mooc.Data.Mongo;
using Mooc.Data.ViewModels;
using Mooc.Utils;
using Mooc.Web.Attribute;
using Mooc.Web.Models;

namespace Mooc.Web.Controllers
{
    [CheckLogin]
    public class UserCenterController : Controller
    {
        private readonly DataContext _dataContext;

        public UserCenterController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        // GET: UserCenter
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Person()
        {
            var currentUser = _dataContext.Users.Find(LoginHelper.UserId);

            if (currentUser == null)
                return HttpNotFound();

            var model = AutoMapper.Mapper.Map<UserViewModel>(currentUser);
            return View(model);
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
                    var onecourse = _dataContext.Courses.Find(num);
                    appliedCourses.Add(onecourse);
                }

                return PartialView(appliedCourses);
            }

            return HttpNotFound();

        }

        [HttpPost]
        public ActionResult MyCourse(int type)
        {

            long userId = LoginHelper.UserId;
            var list = (from a in _dataContext.CourseApplies
                        join b in _dataContext.Courses on a.CourseId equals b.ID
                        join c in _dataContext.Categorys on b.CategoryId equals c.ID
                        join d in _dataContext.Teachers on b.TeacherId equals d.ID
                        where a.UserId == userId
                        select new UserCenterCourseView()
                        {
                            CourseId = a.CourseId,
                            CourseDetail = b.CourseDetail,
                            CourseCover = "/Api/VideoApi/Image?id=" + b.CoverPic,
                            CourseName = b.CourseName,
                            CategoryName = c.CategoryName,
                            TeacherName = d.TeacherName,
                            ScheduleId = a.ScheduleId

                        }
                ).ToList();

            foreach (var item in list)
            {
                item.StudyStatus = CheckCourseStatus(item.CourseId, item.ScheduleId);
            }
            //var newList = list.Where((x, i) => list.FindIndex(z => z.CourseId == x.CourseId) == i).ToList();//去掉重复
            if (type == 2)
            {
                list = list.Where(p => p.StudyStatus == "正在学习").ToList();
            }
            else if (type == 3)
            {
                list = list.Where(p => p.StudyStatus == "已完成").ToList();
            }

            //this._dataContext.Database.ExecuteSqlCommand();
            //this._dataContext.Database.SqlQuery();
            //            var tran= this._dataContext.Database.BeginTransaction();
            //
            //            tran.Commit();
            //            tran.Rollback();
            return PartialView(list);
        }

        public string CheckCourseStatus(long couseId, long scheduleId)
        {
            DateTime now = DateTime.Now;
            long userId = LoginHelper.UserId;
            var currentSchedule = _dataContext.Schedules.Find(scheduleId);
            int count = _dataContext.Chapters.Count(p => p.CourseId == couseId);
            int iCount = _dataContext.Studies.Count(p => p.CourseId == couseId && p.UserId == userId && p.ScheduleId == scheduleId);
            if (iCount > 0 && count == iCount)
            {
                return "已完成";
            }
            //这里少一步逻辑，正在学习的逻辑是：courseApplied.schedule在当前时间内，但study的chapterID ！=course.chapters
            if (currentSchedule != null
                && currentSchedule.StartTime <= now
                && currentSchedule.EndTime >= now
                && iCount >= 0
                && count > iCount)
            {
                return "正在学习";
            }

            return "未学习";

        }

        [HttpPost]
        public JsonResult GetStatus(long id)
        {
            var coursePerChapterNum = _dataContext.Chapters.Where(x => x.CourseId == id).Count();

            var studyedChapterPerCourse = _dataContext.Studies.Where(x => x.CourseId == id && x.UserId == LoginHelper.UserId).Count();

            if (studyedChapterPerCourse >= coursePerChapterNum)
            {
                return Json(new { code = 0 });
            }

            return Json(new { code = 1 });
        }

        public ActionResult MyDetail()
        {
            var curuser = _dataContext.Users.Find(LoginHelper.UserId);

            ViewBag.RoleList = SelectOptions.GetRoleSelectOptions();
            ViewBag.GenderList = SelectOptions.GetGenderSelectOptions();
            var viewmodel = AutoMapper.Mapper.Map<UserViewModel>(curuser);
            return PartialView(viewmodel);
            
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

        [HttpPost]
        public JsonResult CheckPwd(string password)
        {
            //string decrypassword = DecryptStringAES.DecryptByAES(password);
            string pwd = MD5Help.MD5Encoding(password, ConfigurationManager.AppSettings["sKey"].ToString());

            var user = _dataContext.Users.Find(LoginHelper.UserId);

            if (user != null && user.PassWord == pwd)
            {
                return Json(new { code = 0 });
            }

            return Json(new { code = 1 });
        }

        [HttpPost]
        public JsonResult SaveNewPass(string newpass)
        {
            if (newpass.IsNullOrWhiteSpace() || newpass.Length < 6)
            {
                return Json(new { code = 1, msg = "密码有误" });
            }

            var curuser = _dataContext.Users.Find(LoginHelper.UserId);

            if (curuser != null)
            {
                curuser.PassWord = MD5Help.MD5Encoding(newpass, ConfigurationManager.AppSettings["sKey"].ToString());
                _dataContext.SaveChanges();
                return Json(new { code = 0 });
            }

            return Json(new { code = 1, msg = "更改密码失败" });
        }

    }
}

