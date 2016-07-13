using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TravelExpenseReport.Models
{
    public class PatientUser
    {
        public int PatientUserId { get; set; }  // Löpnr
        public int PatientId { get; set; }      // BrukarID i Patient-tabellen (PatientId?)
        public string StaffUserId { get; set; } // user-tabellens Id för assistent/workleader/gruppadmin  etc
        public int StaffRoleId { get; set; } // StaffUserRoles-tabellens Id

        [ForeignKey("StaffUserId")]
        public virtual ApplicationUser ApplicationUser { get; set; }

        [ForeignKey("PatientId")]
        public virtual Patient Patient { get; set; }

       [ForeignKey("StaffRoleId")]
        public virtual StaffRole StaffRole { get; set; }

        }
}