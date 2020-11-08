using FIT5032_A.Models;
using FIT5032_A.Utils;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FIT5032_A.Controllers
{
    [RequireHttps]
    public class HomeController : Controller
    {
        private Users db = new Users();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult BulkEmailList()
        {
            return View(db.AspNetUsers.ToList());
        }

        // [HttpPost]
        public ActionResult BulkEmailList2(string id)
        {
            ViewBag.Result = id;

            return View("~/Views/Home/BulkEmail.cshtml");
        }

        public ActionResult BulkEmail()
        {
            return View();
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult BulkEmail(BulkEmail model, HttpPostedFileBase postedFile)
        {
            ModelState.Clear();
            var myUniqueFileName = string.Format(@"{0}", Guid.NewGuid());
            model.Attachment = myUniqueFileName;
            if (ModelState.IsValid)
            {
                try
                {
                    string[] emailList = model.ToEmails.Split(',');

                    var toEmails = new List<EmailAddress>();

                    for (int i = 0; i < emailList.Length; i++)
                    {
                        if (emailList[i].Trim().Length != 0)
                        {
                            toEmails.Add(new EmailAddress(emailList[i], emailList[i]));
                        }

                    }
                    string serverPath = Server.MapPath("~/Uploads/");
                    string fileExtension = Path.GetExtension(postedFile.FileName);
                    string filePath = model.Attachment + fileExtension;
                    model.Attachment = filePath;
                    postedFile.SaveAs(serverPath + model.Attachment);
                    string file = serverPath + filePath;
                    String subject = model.Subject;
                    String contents = model.Contents;
                    EmailSender es = new EmailSender();
                    es.Send_To_Many(toEmails, subject, contents, file);
                    ViewBag.Result = "Email sent to everyone";

                    ModelState.Clear();

                    return View(new BulkEmail());
                }
                catch
                {
                    ViewBag.Result = "Error Sending Email";
                    return View();
                }
            }

            return View();
        }

        [Authorize]
        public ActionResult Chat()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return HttpNotFound();
            }
            return View();
            
        }
    }
}