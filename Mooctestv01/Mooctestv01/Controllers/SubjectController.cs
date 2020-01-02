using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Mooctestv01.Models;
using Mooctestv01.Repository;
using Mooctestv01.ViewModels;

namespace Mooctestv01.Controllers
{
    public class SubjectController : Controller
    {
        ISubjectRepository _subjectRepository = new SubjectRepository();
        // GET: Subject
        public async Task<ActionResult> Index()
        {
            var subjects = await _subjectRepository.ListAsync();
            return View(subjects);
        }


        public ActionResult Create()
        {
            return View();
        }

        public async Task<ActionResult> Add(SubjectModel model)
        {
            if (!ModelState.IsValid)
            {
                return Content("BadRequest !!");
            }

            await _subjectRepository.AddAsync(new Subject()
            {
                SubName = model.SubName,
                Classify = model.Classify,
                SubDetail = model.SubDetail,
                AddTime = DateTime.Now
            });
            return RedirectToAction("Index");
        }
    }
}