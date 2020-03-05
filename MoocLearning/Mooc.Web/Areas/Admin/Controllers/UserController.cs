using Mooc.Data.Context;
using Mooc.Data.Entities;
using Mooc.Data.Enums;
using Mooc.Data.Mongo;
using Mooc.Data.Service;
using Mooc.Data.ViewModels;
using Mooc.Utils;
using Mooc.Web.Areas.Admin.Attribute;
using Mooc.Web.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mooc.Web.Areas.Admin.Controllers
{
   //[CheckAdminLogin]

    public class UserController : Controller
    {

        private TeacherOption _teacheroption = new TeacherOption();
        private CategoryOption _categoryoption = new CategoryOption();



        private IUserService _userService;
        private DataContext _dataContext;

        //Inject EF operation class
        public UserController(IUserService userService, DataContext dataContext)
        {
            _userService = userService;
            _dataContext = dataContext;
        }


        // GET: Admin/User
        //public ActionResult Index()
        //{

        //    List<User> userList = _dataContext.Users.ToList();
        //    //List<UserViewModel> userViewModels = AutoMapper.Mapper.Map<List<User>,List<UserViewModel>>(users);
        //    List<UserViewModel> list = new List<UserViewModel>();
        //    if (userList != null && userList.Count > 0)
        //    {
        //        list = AutoMapper.Mapper.Map<List<User>, List<UserViewModel>>(userList);
        //    }

        //    return View(list);
        //}

        public ActionResult Index()
        {
            return View("List");
        }


        public ActionResult AjaxList()
        {
            return View();
        }


        [HttpPost]
        public JsonResult GetUserList()
        {
            List<User> userList = _dataContext.Users.ToList();

            List<UserViewModel> list = new List<UserViewModel>();
            if (userList != null && userList.Count > 0)
            {
                list = AutoMapper.Mapper.Map<List<User>, List<UserViewModel>>(userList);
            }
            return Json(new { list = list });
        }

        public ActionResult AjaxPartialList()
        {
            return View();
        }

        [HttpPost]
        public ActionResult _PartUserList()
        {
            List<User> userList = _dataContext.Users.ToList();

            List<UserViewModel> list = new List<UserViewModel>();
            if (userList != null && userList.Count > 0)
            {
                list = AutoMapper.Mapper.Map<List<User>, List<UserViewModel>>(userList);
            }
            return PartialView(list);
        }


        public ActionResult Create()
        {
            ViewBag.RoleList = SelectOptions.GetRoleSelectOptions();
            ViewBag.TeacherList = _teacheroption.GetTeacherSelectOptions();
            var viewmodel = new UserViewModel() { ID = 0 };
            return View(viewmodel);
        }


        //~~~~~~~~~~~~~~~~~~~
        //加入base64图片储存的create
        public ActionResult AjCreate()
        {
            ViewBag.RoleList = SelectOptions.GetRoleSelectOptions();
            ViewBag.GenderList = SelectOptions.GetGenderSelectOptions();
            ViewBag.TeacherList = _teacheroption.GetTeacherSelectOptions();
            ViewBag.CategoryList = _categoryoption.GetCategorySelectOptions();

            return View();
        }

        [HttpPost]
        public JsonResult SaveBase64(string base64)
        {
            if (string.IsNullOrEmpty(base64))
                return Json(new { code = 1, msg = "图片不存在" });

            try
            {

                string fileName = $"{Guid.NewGuid().ToString("N")}.jpg";


                string data = base64.Substring(base64.IndexOf(",") + 1);
                byte[] arr = Convert.FromBase64String(data);

                string uploadId = MongoDBHelper.Upload(fileName, arr);
                if (string.IsNullOrEmpty(uploadId))
                    return Json(new { code = 1, msg = "图片不存在" });
                return Json(new { code = 0, msg = "上传成功", fileName = fileName, objectId = uploadId });
            }
            catch (Exception e)
            {

                return Json(new { code = 1, msg = e.Message });
            }
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
        public JsonResult AjSave(User user)
        {
            if (user != null)
            {
                //不用这方法处理MD5加密
                //String EncryptPass(String pass)
                //{
                //    System.Security.Cryptography.MD5CryptoServiceProvider md5CSP = new System.Security.Cryptography.MD5CryptoServiceProvider();
                //    byte[] passEncrypt = System.Text.Encoding.Unicode.GetBytes(pass);
                //    byte[] resultEncrypt = md5CSP.ComputeHash(passEncrypt);
                //    string passResult = System.Text.Encoding.Unicode.GetString(resultEncrypt);
                //    return passResult;
                //}

                //DES处理加密
                user.PassWord = MD5Help.MD5Encrypt(user.PassWord, ConfigurationManager.AppSettings["sKey"].ToString());
               

                _dataContext.Users.Add(user);
                _dataContext.SaveChanges();
                return Json(new { code = 0 });
            }
            return Json(new { code = 1, msg = "出错" });

        }


        public ActionResult AjEdit(long? id)
        {
            if (id != null && id != 0)
            {
                ViewBag.RoleList = SelectOptions.GetRoleSelectOptions();
                ViewBag.GenderList = SelectOptions.GetGenderSelectOptions();
                ViewBag.TeacherList = _teacheroption.GetTeacherSelectOptions();

                User user = _dataContext.Users.Find(id);
                
                user.PassWord = MD5Help.MD5Decrypt(user.PassWord, ConfigurationManager.AppSettings["sKey"].ToString());
                var viewmodel = AutoMapper.Mapper.Map<UserViewModel>(user);
                
                return View(viewmodel);

            }

            return HttpNotFound("用户不存在");
        }


        [HttpPost]
        public JsonResult AjSaveEdit(User user)
        {
            var exist = _dataContext.Users.Find(user.ID);
            if (user != null && exist != null)
            {
                    String EncryptPass(String pass)
                    {
                        System.Security.Cryptography.MD5CryptoServiceProvider md5CSP = new System.Security.Cryptography.MD5CryptoServiceProvider();
                        byte[] passEncrypt = System.Text.Encoding.Unicode.GetBytes(pass);
                        byte[] resultEncrypt = md5CSP.ComputeHash(passEncrypt);
                        string passResult = System.Text.Encoding.Unicode.GetString(resultEncrypt);
                        return passResult;
                    }

                    //处理加密
                    exist.PassWord = EncryptPass(user.PassWord);
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
        public JsonResult DeleteUser(long? DeleteID)
        {
            if (DeleteID != null)
            {

                var user = _dataContext.Users.Find(DeleteID);
                _dataContext.Users.Remove(user);

                _dataContext.SaveChanges();

                return Json(new { code = 0 });
            }
            return Json(new { code = 1, msg = "该学员不存在" });

        }

        //~~~~~~~~~~~~~~~~~~~~~~

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Save(UserViewModel model)
        //{
        //    ViewBag.RoleList = SelectOptions.GetRoleSelectOptions();
        //    ViewBag.TeacherList = _teacheroption.GetTeacherSelectOptions();
        //    if (!ModelState.IsValid)
        //    {
        //        User user = new User();

        //        if (model.ID > 0)
        //        {
        //            user = _dataContext.Users.Find(model.ID);
        //            if (user == null)
        //            {
        //                ModelState.AddModelError("", "当前用户不存在");
        //                //return RedirectToAction("Create", model);
        //                return View("Create", model);
        //            }

        //            //保存上传图片！！
        //            var file = model.Img;
        //            string fileExtension = Path.GetExtension(file.FileName);
        //            string[] filetype = { ".jpg", ".gif", ".bmp", ".png" }; //文件允许格式 ".jpg", ".gif", ".bmp",".png"
        //            bool checkType = Array.IndexOf(filetype, fileExtension) == -1;
        //            if (checkType)
        //            {
        //                ModelState.AddModelError("", "图片文件格式错误");
        //                return View("Create", model);
        //            }

        //            if (file.ContentLength >= 10 * 1024 * 1024)//1000MB
        //            {

        //                ModelState.AddModelError("", "图片文件大小不能超过10MB");
        //                return View("Create", model);
        //            }

        //            string fileName = $"{Guid.NewGuid().ToString("N")}{fileExtension}";
        //            try
        //            {
        //                string savaFile = System.Web.HttpContext.Current.Server.MapPath("~/Areas/Admin/Images/");
        //                if (!Directory.Exists(savaFile))
        //                {
        //                    Directory.CreateDirectory(savaFile);
        //                }
        //                var filePath = Path.Combine(savaFile, fileName);
        //                file.SaveAs(filePath);

        //                user = AutoMapper.Mapper.Map<User>(model);
        //                user.UserState = (int)StatusEnum.Normal;
        //                user.RoleType = model.RoleName.ToInt(3);
        //                user.ImgGuid = fileName;

        //            }
        //            catch (Exception)
        //            {

        //                return View("Create", model);
        //            }
        //            user = AutoMapper.Mapper.Map<User>(model);
        //            user.UserState = (int)StatusEnum.Normal;
        //            user.RoleType = model.RoleName.ToInt(3);
        //        }
        //        else
        //        {
        //            user = _dataContext.Users.FirstOrDefault(c => c.UserName == model.UserName);
        //            if (user != null)
        //            {
        //                ModelState.AddModelError("", "当前用户已存在");
        //                // return RedirectToAction("Create", model);
        //                return View("Create", model);
        //            }
        //            //保存上传图片！！
        //            var file = model.Img;
        //            string fileExtension = Path.GetExtension(file.FileName);
        //            string[] filetype = { ".jpg", ".gif", ".bmp", ".png" }; //文件允许格式 ".jpg", ".gif", ".bmp",".png"
        //            bool checkType = Array.IndexOf(filetype, fileExtension) == -1;
        //            if (checkType)
        //            {
        //                ModelState.AddModelError("", "图片文件格式错误");
        //                return View("Create", model);
        //            }

        //            if (file.ContentLength >= 10 * 1024 * 1024)//1000MB
        //            {

        //                ModelState.AddModelError("", "图片文件大小不能超过10MB");
        //                return View("Create", model);
        //            }

        //            string fileName = $"{Guid.NewGuid().ToString("N")}{fileExtension}";
        //            try
        //            {
        //                string savaFile = System.Web.HttpContext.Current.Server.MapPath("~/Areas/Admin/Images/");
        //                if (!Directory.Exists(savaFile))
        //                {
        //                    Directory.CreateDirectory(savaFile);
        //                }
        //                var filePath = Path.Combine(savaFile, fileName);
        //                file.SaveAs(filePath);

        //                user = AutoMapper.Mapper.Map<User>(model);
        //                user.ImgGuid = fileName;
        //                user.UserState = (int)StatusEnum.Normal;
        //                user.RoleType = model.RoleName.ToInt(model.RoleType);
        //                _dataContext.Users.Add(user);

        //            }
        //            catch (Exception)
        //            {

        //                return View("Create", model);
        //            }

        //        }

        //        _dataContext.SaveChanges();
        //        return RedirectToAction("List");

        //    }

        //return RedirectToAction("Create", model);
        //return View("Create", model);
        //ArrayList nameList = new ArrayList();
        //List<User> users = _dataContext.Users.ToList();
        //foreach (var user in users)
        //{
        //    nameList.Add(user.UserName);
        //}

        //if (!ModelState.IsValid)
        //{
        //    return View("Create", adminUserViewModel);
        //}

        //if (nameList.Contains(adminUserViewModel.UserName))
        //{
        //    var userInDb = _dataContext.Users.Single(c => c.UserName == adminUserViewModel.UserName);

        //    //User user = AutoMapper.Mapper.Map<User>(adminUserViewModel);

        //    userInDb.UserName = adminUserViewModel.UserName;
        //    userInDb.PassWord = adminUserViewModel.PassWord;
        //    userInDb.Email = adminUserViewModel.Email;
        //    userInDb.TeacherId = adminUserViewModel.TeacherId;
        //}
        //else
        //{
        //    User user = AutoMapper.Mapper.Map<User>(adminUserViewModel);
        //    _dataContext.Users.Add(user);
        //}

        //_dataContext.SaveChanges();
        // return RedirectToAction("Index");
        //}

        public ActionResult Edit(int id)
        {
            ViewBag.RoleList = SelectOptions.GetRoleSelectOptions();
            ViewBag.TeacherList = _teacheroption.GetTeacherSelectOptions();
            /// var user = _dataContext.Users.SingleOrDefault(c => c.ID == id);
            var user = _dataContext.Users.Find(id);
            if (user == null)
                return HttpNotFound();

            var adminUserViewModel = AutoMapper.Mapper.Map<UserViewModel>(user);

            return View("Create", adminUserViewModel);

        }

        public ActionResult Delete(int id)
        {
            var user = _dataContext.Users.SingleOrDefault(c => c.ID == id);
            if (user != null)
            {
                //使用假删除，把更改userstate，前端做便利user显示时剔除userstate=2的对象
                //userdel.UserState = 2;
                //_dataContext.SaveChanges();
                //return RedirectToAction("Index");
                _dataContext.Users.Remove(user);
                _dataContext.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        //[HttpPost]
        //public ActionResult Edit(UserViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        if (model.ID == 0)
        //        {
        //            _dataContext.Users.Add(model);
        //        }
        //        else
        //        {
        //            var updateUser = _dataContext.Users.Find(model.ID);

        //            updateUser.UserName = model.UserName;
        //            updateUser.PassWord = model.PassWord;
        //            updateUser.Email = model.Email;
        //            updateUser.StudentNo = model.StudentNo;
        //            updateUser.TeacherId = model.TeacherId;
        //            updateUser.RoleType = model.RoleType;

        //        }

        //        _dataContext.SaveChanges();
        //        return Redirect("~/Admin/User/Index");

        //    }
        //    else
        //    {
        //        ModelState.AddModelError("", "请正确填写");
        //    }
        //    return View(model);
        //}

        public ActionResult AjaxEdit(int id)
        {
            ViewBag.RoleList = SelectOptions.GetRoleSelectOptions();
            ViewBag.TeacherList = _teacheroption.GetTeacherSelectOptions();

            var user = _dataContext.Users.Find(id);
            if (user == null)
                return HttpNotFound();

            var adminUserViewModel = AutoMapper.Mapper.Map<UserViewModel>(user);

            return View("Create", adminUserViewModel);

        }

        public ActionResult List()
        {
            return View();
        }


        [HttpPost]
        public JsonResult PageList(int pageIndex, int pageSize)
        {
            PageResult<UserViewModel> result = new PageResult<UserViewModel>() { data = new List<UserViewModel>(), PageIndex = pageIndex, PageSize = pageSize };
            int current = (pageIndex - 1) * pageSize;

            //这里必须用左链接，要不然只能查出来有teacherID的user，别的“admin”“student”类型的用户就查不到
            var list = (from a in _dataContext.Users
                        join c in _dataContext.Teachers on a.TeacherId equals c.ID into ps
                        from c in ps.DefaultIfEmpty()
                        select new UserViewModel
                        {
                            ID = a.ID,
                            UserName = a.UserName,
                            PassWord = a.PassWord,
                            Email = a.Email,
                            StudentNo = a.StudentNo,
                            TeacherName = c.TeacherName==null?"not teacher":c.TeacherName,
                            Gender = a.Gender,

                            UserState = a.UserState,
                            RoleType = a.RoleType,
                            ImgGuid = a.ImgGuid


                        }); ;



            var listview = list.OrderByDescending(p => p.ID).Skip(current).Take(pageSize).ToList();
            result.data = listview;
            result.Count = list.Count();

            return Json(result);
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
                    string[] filetype = { ".jpg", ".jpeg", ".gif", ".png"}; //文件允许格式jpg、jpeg、gif、png
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
    }
}