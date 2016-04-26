using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TravelExpenseReport.Models
{
    public class TravelReport : IValidatableObject
    {
        public int TravelReportId { get; set; }

        public string ApplicationUserId { get; set; }

        [DisplayName("Reseräkningsnummer")]
        public string TravelReportName { get; set; }

        [DisplayName("Resmål")]
        [Required]
        public string Destination { get; set; }

        [DisplayName("Syfte")]
        [Required]
        public string Purpose { get; set; }

       
        [DisplayName("Avresa datum")]
        [DataType(DataType.Date)]
        public DateTime DepartureDate { get; set; }

        [DisplayName("Avresa tid")]
        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:hh\\:mm}", ApplyFormatInEditMode = true)]
        public TimeSpan DepartureTime { get; set; }


        [DisplayName("Hemkomst datum")]
        [DataType(DataType.Date)]
        public DateTime ReturnDate { get; set; }

        [DisplayName("Hemkomst tid")]
        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:hh\\:mm}", ApplyFormatInEditMode = true)]
        public TimeSpan ReturnTime { get; set; }

        [DisplayName("Avresa extra timmar")]
        [Range(0, 72, ErrorMessage = "Måste vara minst 0")]
        public int? DepartureHoursExtra { get; set; }

        [DisplayName("Hemresa extra timmar")]
        [Range(0, 72, ErrorMessage = "Måste vara minst 0")]
        public int? ReturnHoursExtra { get; set; }

        [DisplayName("Heldag")]
        [Range(0, int.MaxValue, ErrorMessage = "Måste vara minst 0")]
        public int? FullDay { get; set; }

        [DisplayName("Halvdag")]
        [Range(0, int.MaxValue, ErrorMessage = "Måste vara minst 0")]
        public int? HalfDay { get; set; }

        [DisplayName("Natt")]
        [Range(0, int.MaxValue, ErrorMessage = "Måste vara minst 0")]
        public int? Night { get; set; }

        [DisplayName("Avdrag frukost")]
        [Range(0, int.MaxValue, ErrorMessage = "Måste vara minst 0")]
        public int? BreakfastReduction { get; set; }

        [DisplayName("Avdrag lunch")]
        [Range(0, int.MaxValue, ErrorMessage = "Måste vara minst 0")]
        public int? LunchReduction { get; set; }

        [DisplayName("Avdrag middag")]
        [Range(0, int.MaxValue, ErrorMessage = "Måste vara minst 0")]
        public int? DinnerReduction { get; set; }

        [DisplayName("Status")]
        public int StatusTypeId { get; set; }

        [DisplayName("Kommentar")]
        public string Comment { get; set; }

        public virtual ICollection<Expense> Expenses { get; set; }

        [ForeignKey("ApplicationUserId")]
        public virtual ApplicationUser ApplicationUser { get; set; }

        [ForeignKey("StatusTypeId")]
        public virtual StatusType StatusType { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (ReturnDate < DepartureDate)
            {
                yield return new ValidationResult("Hemkomstdatum måste vara senare eller lika med avresedatum!");
            }
        }


        // Set default values
        public TravelReport()
        {
            BreakfastReduction = 0;
            LunchReduction = 0;
            DinnerReduction = 0;
            DepartureHoursExtra = 0;
            ReturnHoursExtra = 0;
            StatusTypeId = 4;
        }
    }
}