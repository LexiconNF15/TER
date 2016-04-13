using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TravelExpenseReport.Models
{
    public class Expense
    {
        public int ExpenseId { get; set; }
        public string ExpenseType { get; set; }
        public string ExpenseInformation { get; set; }
        public DateTime ExpenseDate { get; set; }
        public float ExpenseAmount { get; set; }
        public int? ExpenseMilage { get; set; }

        public int TravelReportId { get; set; }
        [ForeignKey("TravelReportId")]
        public virtual TravelReport TravelReport { get; set; }

    }
}