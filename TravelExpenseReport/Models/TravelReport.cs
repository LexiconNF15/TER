using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TravelExpenseReport.Models
{
    public class TravelReport
    {
        public int TravelReportId { get; set; }
        public string TravelReportName { get; set; }
        public string Destination { get; set; }
        public string Purpose { get; set; }
        public DateTime DepartureDateTime { get; set; }
        public DateTime ReturnDateTime { get; set; }
        public int DepartureHoursExtra { get; set; }
        public int ReturnHoursExtra { get; set; }
        public int FullDay { get; set; }
        public int HalfDay { get; set; }
        public int Night { get; set; }
        public int BreakfastReduction { get; set; }
        public int LunchReduction { get; set; }
        public int DinnerReduction { get; set; }
        public string Status { get; set; }
        public string Commment { get; set; }

        public virtual ICollection<Expense> Expenses { get; set; }

        public string ApplicationUserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }

    }
}