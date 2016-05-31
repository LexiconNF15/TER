using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TravelExpenseReport.Models
{
    public class Patient
    {
        public int PatientId { get; set; }  //Löpnr
        public string Name { get; set; }  //ev namn om ej enbart i Usertabellen
        public int CustomerId { get; set; }  //ftg om ej enbart i usertabellen.
        public string UserId { get; set; } // user-tabellens UserId för Patienten

        [ForeignKey("UserId")]
        public virtual ApplicationUser ApplicationUser { get; set; }

        //[ForeignKey("PatientId")]
        //public virtual ICollection<PatientUser> PatientUser { get; set; }
    }
}