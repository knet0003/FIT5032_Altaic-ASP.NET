namespace FIT5032_A.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Web.Mvc;

    public partial class Email
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string From { get; set; }

        [Required]
        [StringLength(50)]
        public string To { get; set; }

        [Required]
        [StringLength(50)]
        public string Subject { get; set; }

        [AllowHtml]
        [Required]
        [StringLength(50)]
        public string Content { get; set; }

        [Required]
        [StringLength(50)]
        public string Attachment { get; set; }
    }
}
