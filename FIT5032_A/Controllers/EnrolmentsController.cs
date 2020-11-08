using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FIT5032_A.Models;
using Microsoft.AspNet.Identity;

namespace FIT5032_A.Controllers
{
    public class EnrolmentsController : Controller
    {
        private FIT5032_Models db = new FIT5032_Models();

        // GET: Enrolments
        [Authorize]
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            List<Enrolment> enrolments = new List<Enrolment>();
            if (User.IsInRole("Administrator"))
            {
                enrolments = db.Enrolments.ToList(); //.Include(e => e.Student).Include(e => e.CourseId).ToList();
            }
            else
            {
                Student student = db.Students.Where(s => s.UserId == userId).First();
                enrolments = db.Enrolments.Where(e => e.StudentId == student.Id).ToList();
            }
            return View(enrolments);
        }

        // GET: Enrolments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Enrolment enrolment = db.Enrolments.Find(id);
            if (enrolment == null)
            {
                return HttpNotFound();
            }
            return View(enrolment);
        }

        // GET: Enrolments/Create
        [Authorize]
        public ActionResult Create()
        {
            if (User.Identity.IsAuthenticated)
            {
                ViewBag.CourseId = new SelectList(db.Courses, "Id", "Name");
                ViewBag.StudentId = new SelectList(db.Students, "Id", "FirstName");
            }
            return View();
        }

        // POST: Enrolments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create([Bind(Include = "Id, StudentId, CourseId")] Enrolment enrolment)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (ModelState.IsValid)
                {
                    var userId = User.Identity.GetUserId();
                    enrolment.Date = DateTime.UtcNow;

                    if (User.IsInRole("Administrator"))
                    {

                    }
                    else
                    {
                        Student student = db.Students.Where(s => s.UserId == userId).First();
                        enrolment.StudentId = student.Id;
                    }
                    List<Cours> courses = new List<Cours>();
                    var studentenrolments = db.Enrolments.Where(e => e.StudentId == enrolment.StudentId).ToList();
                    foreach (Enrolment en in studentenrolments)
                    {
                        Cours course = db.Courses.Where(c => c.Id == en.CourseId).First();
                        courses.Add(course);
                    }
                    List<int> courseids = new List<int>();
                    foreach (Cours co in courses)
                    {
                        courseids.Add(co.Id);
                    }
                    if (courseids.Contains(enrolment.CourseId))
                    {
                        ViewBag.Result = "Already enrolled in this course";
                        return RedirectToAction("Index");
                    }
                    db.Enrolments.Add(enrolment);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                ViewBag.CourseId = new SelectList(db.Courses, "Id", "Name", enrolment.CourseId);
                ViewBag.StudentId = new SelectList(db.Students, "Id", "FirstName", enrolment.StudentId);
            }
            return View(enrolment);
        }

        // GET: Enrolments/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Enrolment enrolment = new Enrolment();
            if (User.IsInRole("Administrator"))
            {
                enrolment = db.Enrolments.Find(id);
            }
            if (enrolment == null)
            {
                return HttpNotFound();
            }
            ViewBag.CourseId = new SelectList(db.Courses, "Id", "Name", enrolment.CourseId);
            ViewBag.StudentId = new SelectList(db.Students, "Id", "FirstName", enrolment.StudentId);
            return View(enrolment);
        }

        // POST: Enrolments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit([Bind(Include = "Id,Date,StudentId,CourseId")] Enrolment enrolment)
        {
            if (User.IsInRole("Administrator"))
            {
                if (ModelState.IsValid)
                {
                    db.Entry(enrolment).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                ViewBag.CourseId = new SelectList(db.Courses, "Id", "Name", enrolment.CourseId);
                ViewBag.StudentId = new SelectList(db.Students, "Id", "FirstName", enrolment.StudentId);
            }
            return View(enrolment);
        }

        // GET: Enrolments/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
        {
                if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Enrolment enrolment = new Enrolment();
            if (User.Identity.IsAuthenticated)
            {
                enrolment = db.Enrolments.Find(id);
            }
            if (enrolment == null)
            {
                return HttpNotFound();
            }
            return View(enrolment);
        }

        // POST: Enrolments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult DeleteConfirmed(int id)
        {
            if (User.Identity.IsAuthenticated)
            {
                Enrolment enrolment = db.Enrolments.Find(id);
                db.Enrolments.Remove(enrolment);
                db.SaveChanges();
            }
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
