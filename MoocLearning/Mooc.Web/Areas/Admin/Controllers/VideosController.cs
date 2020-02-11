using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Mooc.Data.Context;
using Mooc.Data.Entities;

namespace Mooc.Web.Areas.Admin.Controllers
{
    public class VideosController : Controller
    {
        private DataContext db = new DataContext();

        // GET: Admin/Videos
        public ActionResult Index()
        {
            return View();
        }

        // GET: Admin/Videos/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
           // Video video = db.Videos.Find(id);
            //if (video == null)
            //{
            //    return HttpNotFound();
            //}
            return View();
        }

        // GET: Admin/Videos/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Videos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,VideoName,VideoLocation,AddTime")] Video video)
        {
            if (ModelState.IsValid)
            {
               // db.Videos.Add(video);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(video);
        }

        // GET: Admin/Videos/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
           // Video video = db.Videos.Find(id);
            //if (video == null)
            //{
            //    return HttpNotFound();
            //}
            return View();
        }

        // POST: Admin/Videos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,VideoName,VideoLocation,AddTime")] Video video)
        {
            if (ModelState.IsValid)
            {
                db.Entry(video).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(video);
        }

        // GET: Admin/Videos/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Video video = db.Videos.Find(id);
            //if (video == null)
            //{
            //    return HttpNotFound();
            //}
            return View();
        }

        // POST: Admin/Videos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
           // Video video = db.Videos.Find(id);
            //db.Videos.Remove(video);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
