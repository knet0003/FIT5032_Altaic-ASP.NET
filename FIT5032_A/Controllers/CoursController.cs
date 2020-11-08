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
    public class CoursController : Controller
    {
        private FIT5032_Models db = new FIT5032_Models();

        // GET: Cours
        public ActionResult Index()
        {
            List<Cours> courses = new List<Cours>();
            if (User.IsInRole("Student"))
            {
                var userId = User.Identity.GetUserId();
                Student student = db.Students.Where(s => s.UserId == userId).First();
                var studentenrolments = db.Enrolments.Where(e => e.StudentId == student.Id).ToList();
                foreach (Enrolment enrolment in studentenrolments)
                {
                    Cours course = db.Courses.Where(c => c.Id == enrolment.CourseId).First();
                    courses.Add(course);
                }
            }
            else
            {
                courses = db.Courses.Include(c => c.Language).Include(c => c.School).ToList();
            }
            return View(courses);
        }

        // GET: Cours/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cours cours = db.Courses.Find(id);
            if (cours == null)
            {
                return HttpNotFound();
            }
            return View(cours);
        }

        // GET: Cours/Create
        [Authorize]
        public ActionResult Create()
        {
            if (User.IsInRole("Administrator"))
            {
                ViewBag.LanguageId = new SelectList(db.Languages, "Id", "Name");
                ViewBag.SchoolId = new SelectList(db.Schools, "Id", "Name");
            }
            return View();
        }

        // POST: Cours/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create([Bind(Include = "Id,Name,Description,Start,End,LanguageId,SchoolId")] Cours cours)
        {
            if (ModelState.IsValid)
            {
                if (User.IsInRole("Administrator"))
                {
                    db.Courses.Add(cours);
                    db.SaveChanges();
                   
                    return RedirectToAction("Index");
                }
            }

            ViewBag.LanguageId = new SelectList(db.Languages, "Id", "Name", cours.LanguageId);
            ViewBag.SchoolId = new SelectList(db.Schools, "Id", "Name", cours.SchoolId);
            return View(cours);
        }

        // GET: Cours/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cours cours = new Cours();

            if (User.IsInRole("Administrator"))
            {
                cours = db.Courses.Find(id);
                if (cours == null)
                {
                    return HttpNotFound();
                }
                ViewBag.LanguageId = new SelectList(db.Languages, "Id", "Name", cours.LanguageId);
                ViewBag.SchoolId = new SelectList(db.Schools, "Id", "Name", cours.SchoolId);
                    }
            return View(cours);
        }

        // POST: Cours/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit([Bind(Include = "Id,Name,Description,Start,End,LanguageId,SchoolId")] Cours cours)
        {
            if (User.IsInRole("Administrator"))
            {
                if (ModelState.IsValid)
                {
                    db.Entry(cours).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                ViewBag.LanguageId = new SelectList(db.Languages, "Id", "Name", cours.LanguageId);
                ViewBag.SchoolId = new SelectList(db.Schools, "Id", "Name", cours.SchoolId);
            }
            return View(cours);
        }

        // GET: Cours/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cours cours = new Cours();
            if (User.IsInRole("Administrator"))
            {
                cours = db.Courses.Find(id);
                if (cours == null)
                {
                    return HttpNotFound();
                }
            }
            return View(cours);
        }

        // POST: Cours/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult DeleteConfirmed(int id)
        {
            if (User.IsInRole("Administrator"))
            {
                Cours cours = db.Courses.Find(id);
                db.Courses.Remove(cours);
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
