using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Mooc.Data.Context;
using Mooc.Data.Entities;

namespace Mooc.Web.Controllers
{
    public class CoursesApiController : ApiController
    {
        private DataContext db = new DataContext();

        // GET: api/CoursesApi
        public IQueryable<Course> GetCourses()
        {
            return db.Courses;
        }

        // GET: api/CoursesApi/5
        [ResponseType(typeof(Course))]
        public IHttpActionResult GetCourse(long id)
        {
            Course course = db.Courses.Find(id);
            if (course == null)
            {
                return NotFound();
            }

            return Ok(course);
        }

        // PUT: api/CoursesApi/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutCourse(long id, Course course)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != course.ID)
            {
                return BadRequest();
            }

            db.Entry(course).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CourseExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/CoursesApi
        [ResponseType(typeof(Course))]
        public IHttpActionResult PostCourse(Course course)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Courses.Add(course);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = course.ID }, course);
        }

        // DELETE: api/CoursesApi/5
        [ResponseType(typeof(Course))]
        public IHttpActionResult DeleteCourse(long id)
        {
            Course course = db.Courses.Find(id);
            if (course == null)
            {
                return NotFound();
            }

            db.Courses.Remove(course);
            db.SaveChanges();

            return Ok(course);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CourseExists(long id)
        {
            return db.Courses.Count(e => e.ID == id) > 0;
        }
    }
}