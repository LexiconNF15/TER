using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System;

namespace TravelExpenseReport.Models
{
    public class Expense : IValidatableObject
    {
        public int ExpenseId { get; set; }

        [DisplayName("Kostnad")]
        public int ExpenseTypeId { get; set; }

        [DisplayName("Beskrivning")]
        public string ExpenseDescription { get; set; }

        [DisplayName("Datum")]
        [Required(ErrorMessage = "Välj datum")]
        [DataType(DataType.Date)]
        public DateTime ExpenseDate { get; set; }

        [RegularExpression("[0-9]+,?[0-9]?[0-9]?", ErrorMessage = "Ange belopp som siffror med decimalkomma, ex. 124,50")]
        [DisplayName("Belopp")]
        public string ExpenseAmountInfo { get; set; }

        [DisplayName("Belopp")]
        public decimal? ExpenseAmount { get; set; }

        [DisplayName("Kilometer")]
        [Range(0, 2000, ErrorMessage = "Giltigt värde (0 - 2000)")]
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
                    yield return new ValidationResult("Datum ligger utanför perioden för reseräkningen!");
                }
                if ((ExpenseTypeId == 4) && (ExpenseMilage == 0))
                {
                    ExpenseAmountInfo = "0";
                    yield return new ValidationResult("Ange antal kilometer!");
                }
   
            }

            }
    }
}