using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace TravelExpenseReport.Models
{
    public class StatusType
    {
        public int StatusTypeId { get; set; }
        [DisplayName("Status")]
        public string StatusName { get; set; }

        public virtual ICollection<TravelReport> TravelReport { get; set; }
    }
}