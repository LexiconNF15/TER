using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TravelExpenseReport.Models
{
    public class Expense
    {
        public int ExpenseId { get; set; }

        [DisplayName("Utgiftstyp")]
        public int ExpenseTypeId { get; set; }

        [DisplayName("Information")]
        public string ExpenseInformation { get; set; }

        [DisplayName("Datum")]
        public DateTime ExpenseDate { get; set; }

        [DisplayName("Kostnad")]
        public float ExpenseAmount { get; set; }

        [DisplayName("Kilometer")]
        public int? ExpenseMilage { get; set; }

        public int TravelReportId { get; set; }
        [ForeignKey("TravelReportId")]
        public virtual TravelReport TravelReport { get; set; }

        [ForeignKey("ExpenseTypeId")]
        public virtual ExpenseType ExpenseType { get; set; }


    }
}