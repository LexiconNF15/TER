using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TravelExpenseReport.Models
{
    public class StaffRole
    {
        public int StaffRoleId { get; set; }
        public string Name { get; set; }

        [ForeignKey("StaffRoleId")]
        public virtual ICollection<PatientUser> PatientUsers { get; set; }
    }
}