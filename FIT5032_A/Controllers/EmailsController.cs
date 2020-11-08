using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FIT5032_A.Models;
using FIT5032_A.Utils;

namespace FIT5032_A.Controllers
{
    public class EmailsController : Controller
    {
        private FIT5032_Models db = new FIT5032_Models();

        // GET: Emails
        public ActionResult Index()
        {
            return View(db.Emails.ToList());
        }

        // GET: Emails/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Email email = db.Emails.Find(id);
            if (email == null)
            {
                return HttpNotFound();
            }
            return View(email);
        }

        // GET: Emails/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Emails/Create+		email	{FIT5032_A.Models.Email}	FIT5032_A.Models.Email

        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(true)]
        public ActionResult Create([Bind(Include = "Id,From,To,Subject,Content,Attachment")] Email email, HttpPostedFileBase postedFile)
        {
            ModelState.Clear();
            var myUniqueFileName = string.Format(@"{0}", Guid.NewGuid());
            email.Attachment = myUniqueFileName;
            email.From = User.Identity.Name;
            TryValidateModel(email);
            if (ModelState.IsValid)
            {
                String toEmail = email.To;
                String subject = email.Subject;
                String contents = email.Content;
                String fileName = email.Attachment;
                string serverPath = Server.MapPath("~/Uploads/");
                string fileExtension = Path.GetExtension(postedFile.FileName);
                string filePath = email.Attachment + fileExtension;
                email.Attachment = filePath;
                postedFile.SaveAs(serverPath + email.Attachment);
                string file = serverPath + filePath;
                db.Emails.Add(email);
                db.SaveChanges();

                EmailSender es = new EmailSender();
                es.Send(toEmail, subject, contents, file, fileExtension);

                return RedirectToAction("Index");
            }
            return View(email);
        }

        // GET: Emails/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Email email = db.Emails.Find(id);
            if (email == null)
            {
                return HttpNotFound();
            }
            return View(email);
        }

        // POST: Emails/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,From,To,Subject,Content,Attachment")] Email email)
        {
            if (ModelState.IsValid)
            {
                db.Entry(email).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(email);
        }

        // GET: Emails/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Email email = db.Emails.Find(id);
            if (email == null)
            {
                return HttpNotFound();
            }
            return View(email);
        }

        // POST: Emails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Email email = db.Emails.Find(id);
            db.Emails.Remove(email);
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
