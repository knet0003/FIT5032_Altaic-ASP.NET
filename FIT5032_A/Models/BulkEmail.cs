namespace FIT5032_A.Models
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;

    public class BulkEmail
    {
        public string ToEmails { get; set; }

        [Required(ErrorMessage = "Please enter a subject.")]
        public string Subject { get; set; }

        [AllowHtml]
        [Required(ErrorMessage = "Please enter the contents")]
        public string Contents { get; set; }

        public string Attachment { get; set; }
    }
}