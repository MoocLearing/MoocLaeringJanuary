using Mooc.Data.Context;
using Mooc.Data.Entities;
using Mooc.Data.Enums;
using Mooc.Data.ViewModels;
using Mooc.Web.Areas.Admin.Attribute;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace Mooc.Web.Areas.Admin.Controllers
{
    //[CheckAdminLogin]
    public class CategoryController : Controller
    {
        private DataContext _dataContext;

        //Inject EF operation class
        public CategoryController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        // GET: Admin/Categlory
        public ActionResult Index()
        {
            List<CategoryViewModel> viewList = new List<CategoryViewModel>();
            var subjectFirstCategory = _dataContext.Categorys.Where(p => p.Type == (int)CategoryTypeEnum.FirstCategory).ToList();

            viewList = AutoMapper.Mapper.Map<List<CategoryViewModel>>(subjectFirstCategory);
            foreach (var item in viewList)
            {
                item.CategryList = _dataContext.Categorys.Where(p => p.ParentId == item.ID && p.Type == (int)CategoryTypeEnum.SecondCategory).ToList();
            }

            return View(viewList);
        }

        public ActionResult Create()
        {
            //var categoryTypes = _dataContext.CategoryTypes.ToList();

            //var viewModel = new CategoryViewModel
            //{
            //    SubjectCategory = new SubjectCategory(),
            //    CategoryTypes = categoryTypes
            //};
            // CategoryViewModel model = new CategoryViewModel();
            var categoryList = _dataContext.Categorys.Where(p => p.Type == (int)CategoryTypeEnum.FirstCategory).ToList();
            ViewBag.categoryList = categoryList;

            return View();
        }

        [HttpPost]
        public JsonResult CreateCategory(Category category)
        {
            try
            {
                category.ParentId = category.Type == 1 ? 0 : category.ParentId;
                _dataContext.Categorys.Add(category);
                _dataContext.SaveChanges();
                return Json(new { code = 0 });

            }
            catch (System.Exception e)
            {
                return Json(new { code = 1, msg = e.Message });
            }

        }


        public ActionResult Edit(long? id)
        {
            var model = _dataContext.Categorys.Find(id);
            if (model == null)
                return HttpNotFound();

            var categoryList = _dataContext.Categorys.Where(p => p.Type == (int)CategoryTypeEnum.FirstCategory).ToList();
            ViewBag.categoryList = categoryList;
            return View(model);
        }


        [HttpPost]
        public JsonResult EditCategory(Category category)
        {
            try
            {
                var model = _dataContext.Categorys.Find(category.ID);
                if (model == null)
                    return Json(new { code = 1, msg = "当前类别不存在" });

                model.ParentId = category.Type == 1 ? 0 : category.ParentId;
                model.CategoryName = category.CategoryName;
                model.Remark = category.Remark;
                model.Type = category.Type;
                _dataContext.SaveChanges();

                return Json(new { code = 0 });

            }
            catch (System.Exception e)
            {
                return Json(new { code = 1, msg = e.Message });
            }

        }

        [HttpPost]
        public JsonResult DeleteCategory(long id)
        {
            var model = _dataContext.Categorys.Find(id);
            if (model == null)
                return Json(new { code = 1, msg = "当前类别不存在" });
            if (model.Type == 1)
            {
                if (_dataContext.Categorys.Count(p => p.ParentId == model.ID) > 0)
                {
                    return Json(new { code = 1, msg = "当前类别下包含二级分类，暂时无法删除" });
                }
            }
            _dataContext.Categorys.Remove(model);
            _dataContext.SaveChanges();

            return Json(new { code = 0 });
        }

        //[HttpPost]
        //public ActionResult Save(SubjectCategory subjectCategory)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        var viewModel = new CategoryViewModel
        //        {
        //            SubjectCategory = subjectCategory,
        //            CategoryTypes = _dataContext.CategoryTypes.ToList()
        //        };
        //        return View("Create",viewModel);
        //    }

        //    if (subjectCategory.ID == 0)
        //    {
        //        _dataContext.SubjectCategories.Add(subjectCategory);
        //    }
        //    else
        //    {
        //        var subjectCategoryInDb = _dataContext.SubjectCategories.Single(c => c.ID == subjectCategory.ID);
        //        subjectCategoryInDb.CategoryName = subjectCategory.CategoryName;
        //        subjectCategoryInDb.CategoryTypeId = subjectCategory.CategoryTypeId;
        //        subjectCategoryInDb.Remark = subjectCategory.Remark;
        //    }

        //    _dataContext.SaveChanges();

        //    return RedirectToAction("Index");
        //}

        //public ActionResult Edit(int id)
        //{
        //    var subjectCategory = _dataContext.SubjectCategories.SingleOrDefault(c => c.ID == id);
        //    if (subjectCategory == null)
        //    {
        //        return HttpNotFound();
        //    }

        //    var viewModel = new CategoryViewModel
        //    {
        //        SubjectCategory = subjectCategory,
        //        CategoryTypes = _dataContext.CategoryTypes.ToList()
        //    };

        //    return View("Create", viewModel);
        //}

        //public ActionResult Delete(int id)
        //{
        //    var subjectCategory = _dataContext.SubjectCategories.SingleOrDefault(c => c.ID == id);
        //    if (subjectCategory != null)
        //    {
        //        subjectCategory.IsDel = 2;
        //        _dataContext.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    return RedirectToAction("Index");
        //}
    }
}