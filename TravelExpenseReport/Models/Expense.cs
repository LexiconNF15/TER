using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TravelExpenseReport.Models
{
    public class Expense : IValidatableObject
    {
        public int ExpenseId { get; set; }

        [DisplayName("Utgiftstyp")]
        public int ExpenseTypeId { get; set; }

        [DisplayName("Beskrivning")]
        public string ExpenseInformation { get; set; }

        [DisplayName("Datum")]
        [DataType(DataType.Date)]
        public DateTime ExpenseDate { get; set; }

        [DisplayName("Kostnad")]
        public float? ExpenseAmount { get; set; }

        [DisplayName("Kilometer")]
        public int? ExpenseMilage { get; set; }

        public int? TravelReportId { get; set; }
        [ForeignKey("TravelReportId")]
        public virtual TravelReport TravelReport { get; set; }

        [ForeignKey("ExpenseTypeId")]
        public virtual ExpenseType ExpenseType { get; set; }


        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (TravelReportId > 0)
            {
                ApplicationDbContext db = new ApplicationDbContext();
                var ETravel = db.TravelReports.Where(t => t.TravelReportId == TravelReportId).FirstOrDefault();

                if ((ExpenseDate < ETravel.DepartureDate) || (ExpenseDate > ETravel.ReturnDate))
                {
                    yield return new ValidationResult("Utgiftsdatum ligger utanför datumperioden för reseräknngen!");
                }

            }

        }
    }
}