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
    public class LanguagesController : Controller
    {
        private FIT5032_Models db = new FIT5032_Models();

        // GET: Languages
        public ActionResult Index()
        {
            return View(db.Languages.ToList());
        }

        // GET: Languages/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Language language = db.Languages.Find(id);
            if (language == null)
            {
                return HttpNotFound();
            }
            return View(language);
        }

        // GET: Languages/Create
        [Authorize]
        [ValidateInput(true)]
        public ActionResult Create()
        {
            if (User.IsInRole("Administrator"))
            {
                return View();
            }
            return HttpNotFound();
        }

        // POST: Languages/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(true)]
        [Authorize]
        public ActionResult Create([Bind(Include = "Id,Name,Description")] Language language)
        {
            if (User.IsInRole("Administrator"))
            {
                if (ModelState.IsValid)
                {
                    db.Languages.Add(language);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            return View(language);
        }

        // GET: Languages/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Language language = new Language();
            if (User.IsInRole("Administrator"))
            {
                language = db.Languages.Find(id);
            }
            if (language == null)
            {
                return HttpNotFound();
            }
            return View(language);
        }

        // POST: Languages/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit([Bind(Include = "Id,Name,Description")] Language language)
        {
            if (User.IsInRole("Administrator")){
                if (ModelState.IsValid)
                {
                    db.Entry(language).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            return View(language);
        }

        // GET: Languages/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Language language = new Language();
            if (User.IsInRole("Administrator"))
            {
                language = db.Languages.Find(id);
            }
            if (language == null)
            {
                return HttpNotFound();
            }
            return View(language);
        }

        // POST: Languages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult DeleteConfirmed(int id)
        {
            if (User.IsInRole("Administrator"))
            {
                Language language = db.Languages.Find(id);
                db.Languages.Remove(language);
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
