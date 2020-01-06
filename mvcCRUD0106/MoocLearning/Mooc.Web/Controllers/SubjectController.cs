using Mooc.DataAccess.Context;
using Mooc.DataAccess.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using Mooc.DataAccess.Entities;
using Mooc.Models.ViewModels;

//为什么ViewModel.SubjectViewModel引不进来？并且不能用SubjectViewModel生成Create.html?

namespace Mooc.Web.Controllers
{
    public class SubjectController : Controller
    {
        private ISubjectCrud _subjectCrud;

        //Inject CRUD
        public SubjectController(ISubjectCrud subjectCrud)
        {
            _subjectCrud = subjectCrud;
        }
        // GET: Subject

        public async Task<ActionResult> Index()
        {
           var subjects = await _subjectCrud.ListAsync();
            return View(subjects);
        }

        //subject/create
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Create(Subject sub)
        {
            
            await _subjectCrud.AddAsync(new Subject
            {
                SubName = sub.SubName,
                SubDetail = sub.SubDetail,
                SubCreateTime = DateTime.Now
            });
            

            return RedirectToAction("Index");
        }


        //subject/delete/1
        public ActionResult Delete(int subid)
        {


            var Subject = _subjectCrud.DeleteAsync(subid);

            return RedirectToAction("Index");

        }



    }
}