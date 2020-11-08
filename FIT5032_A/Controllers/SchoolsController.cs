using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FIT5032_A.Models;

namespace FIT5032_A.Controllers
{
    public class SchoolsController : Controller
    {
        private FIT5032_Models db = new FIT5032_Models();

        // GET: Schools
        public ActionResult Index()
        {
            return View(db.Schools.ToList());
        }

        // GET: Schools/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            School school = db.Schools.Find(id);
            if (school == null)
            {
                return HttpNotFound();
            }
            return View(school);
        }

        // GET: Schools/Create
        [Authorize]
        public ActionResult Create()
        {
            if (User.IsInRole("Administrator"))
            {
                return View();
            }
            return HttpNotFound();
        }

        // POST: Schools/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create([Bind(Include = "Id,Name,Address,Phone,Latitude,Longitude")] School school)
        {
            if (User.IsInRole("Administrator"))
            {
                if (ModelState.IsValid)
                {
                    db.Schools.Add(school);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            return View(school);
        }

        // GET: Schools/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            School school = new School();
            if (User.IsInRole("Administrator"))
            {
                school = db.Schools.Find(id);
            }
            if (school == null)
            {
                return HttpNotFound();
            }
            return View(school);
        }

        // POST: Schools/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit([Bind(Include = "Id,Name,Address,Phone,Latitude,Longitude")] School school)
        {
            if (ModelState.IsValid)
            {
                if (User.IsInRole("Administrator"))
                {
                    db.Entry(school).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            return View(school);
        }

        // GET: Schools/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            School school = new School();
            if (User.IsInRole("Administrator"))
            {
                school = db.Schools.Find(id);
            }
            if (school == null)
            {
                return HttpNotFound();
            }
            return View(school);
        }

        // POST: Schools/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult DeleteConfirmed(int id)
        {
            if (User.IsInRole("Administrator"))
            {
                School school = db.Schools.Find(id);
                db.Schools.Remove(school);
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
