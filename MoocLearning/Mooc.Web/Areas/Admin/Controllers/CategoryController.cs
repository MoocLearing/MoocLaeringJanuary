using Mooc.Data.Context;
using Mooc.Data.Entities;
using Mooc.Data.ViewModels;
using Mooc.Web.Areas.Admin.Attribute;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace Mooc.Web.Areas.Admin.Controllers
{
    [CheckAdminLogin]
    public class CategoryController : Controller
    {
        private DataContext _dataContext;

        //Inject EF operation class
        public CategoryController( DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        // GET: Admin/Category
        public ActionResult Index()
        {
            var subjectCategories = _dataContext.SubjectCategories.Include(c => c.CategoryType).ToList();
            ViewBag.username = Session["username"];
            return View(subjectCategories);
        }

        public ActionResult Create()
        {
            var categoryTypes = _dataContext.CategoryTypes.ToList();

            var viewModel = new CategoryViewModel
            {
                SubjectCategory = new SubjectCategory(),
                CategoryTypes = categoryTypes
            };
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Save(SubjectCategory subjectCategory)
        {
            if (!ModelState.IsValid)
            {
                var viewModel = new CategoryViewModel
                {
                    SubjectCategory = subjectCategory,
                    CategoryTypes = _dataContext.CategoryTypes.ToList()
                };
                return View("Create",viewModel);
            }

            if (subjectCategory.ID == 0)
            {
                _dataContext.SubjectCategories.Add(subjectCategory);
            }
            else
            {
                var subjectCategoryInDb = _dataContext.SubjectCategories.Single(c => c.ID == subjectCategory.ID);
                subjectCategoryInDb.CategoryName = subjectCategory.CategoryName;
                subjectCategoryInDb.CategoryTypeId = subjectCategory.CategoryTypeId;
                subjectCategoryInDb.Remark = subjectCategory.Remark;
            }

            _dataContext.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult Edit(int id)
        {
            var subjectCategory = _dataContext.SubjectCategories.SingleOrDefault(c => c.ID == id);
            if (subjectCategory == null)
            {
                return HttpNotFound();
            }

            var viewModel = new CategoryViewModel
            {
                SubjectCategory = subjectCategory,
                CategoryTypes = _dataContext.CategoryTypes.ToList()
            };

            return View("Create", viewModel);
        }

        public ActionResult Delete(int id)
        {
            var subjectCategory = _dataContext.SubjectCategories.SingleOrDefault(c => c.ID == id);
            if (subjectCategory != null)
            {
                subjectCategory.IsDel = 2;
                _dataContext.SaveChanges();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
    }
}