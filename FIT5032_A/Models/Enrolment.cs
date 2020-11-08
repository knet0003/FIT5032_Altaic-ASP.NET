namespace FIT5032_A.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Enrolment
    {
        public int Id { get; set; }

        [Display(Name = "Enrolment date")]
        public DateTime Date { get; set; }

        public int StudentId { get; set; }
        public int CourseId { get; set; }

        public virtual Cours Cours { get; set; }

        public virtual Student Student { get; set; }
    }
}
