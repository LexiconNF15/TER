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

        public int? PatientId { get; set; }

        [DisplayName("Reseräkning")]
        public string TravelReportName { get; set; }

        [DisplayName("Resmål")]
        [Required(ErrorMessage = "Skriv in resmål.")]
        public string Destination { get; set; }

        [DisplayName("Syfte")]
        [Required(ErrorMessage = "Skriv in syfte.")]
        public string Purpose { get; set; }

        [DisplayName("Avresa")]
        [Required(ErrorMessage = "Skriv in datum.")]
        [DataType(DataType.Date)]
        public DateTime DepartureDate { get; set; }

        [Required(ErrorMessage = "Skriv in tid.")]
        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:hh\\:mm}", ApplyFormatInEditMode = true)]
        public TimeSpan DepartureTime { get; set; }


        [DisplayName("Hemkomst")]
        [Required(ErrorMessage = "Skriv in datum.")]
        [DataType(DataType.Date)]
        public DateTime ReturnDate { get; set; }

        //[DisplayName("Hemkomst tid")]
        [DataType(DataType.Time)]
        [Required(ErrorMessage = "Skriv in tid.")]
        [DisplayFormat(DataFormatString = "{0:hh\\:mm}", ApplyFormatInEditMode = true)]
        public TimeSpan ReturnTime { get; set; }

        [DisplayName("Avresa extra timmar")]
        [Required(ErrorMessage = "Skriv in värde.")]
        [Range(0, 72, ErrorMessage = "Giltigt värde (0 - 72)")]
        public int DepartureHoursExtra { get; set; }

        [DisplayName("Hemresa extra timmar")]
        [Required(ErrorMessage = "Skriv in värde.")]
        [Range(0, 72, ErrorMessage = "Giltigt värde (0 - 72)")]
        public int ReturnHoursExtra { get; set; }

        [DisplayName("Heldag")]
        [Required(ErrorMessage = "Skriv in antal.")]
        [Range(0, int.MaxValue, ErrorMessage = "Måste vara minst 0")]
        public int? FullDay { get; set; }

        [DisplayName("Halvdag")]
        [Required(ErrorMessage = "Skriv in antal.")]
        [Range(0, int.MaxValue, ErrorMessage = "Måste vara minst 0")]
        public int? HalfDay { get; set; }

        [DisplayName("Natt")]
        [Required(ErrorMessage = "Skriv in antal.")]
        [Range(0, int.MaxValue, ErrorMessage = "Måste vara minst 0")]
        public int? Night { get; set; }

        [DisplayName("Frukost")]
        [Required(ErrorMessage = "Skriv in antal.")]
        [Range(0, int.MaxValue, ErrorMessage = "Måste vara minst 0")]
        public int? BreakfastDeduction { get; set; }

        [DisplayName("Lunch el. middag")]
        [Required(ErrorMessage = "Skriv in antal.")]
        [Range(0, int.MaxValue, ErrorMessage = "Måste vara minst 0")]
        public int? LunchOrDinnerDeduction { get; set; }

        [DisplayName("Lunch och middag")]
        [Required(ErrorMessage = "Skriv in antal.")]
        [Range(0, int.MaxValue, ErrorMessage = "Måste vara minst 0")]
        public int? LunchAndDinnerDeduction { get; set; }

        [DisplayName("Frukost, lunch och middag")]
        [Required(ErrorMessage = "Skriv in antal.")]
        [Range(0, int.MaxValue, ErrorMessage = "Måste vara minst 0")]
        public int? AllMealsDeduction { get; set; }

        [DisplayName("Status")]
        public int StatusTypeId { get; set; }

        [DisplayName("Kommentar")]
        public string Comment { get; set; }

        public virtual ICollection<Expense> Expenses { get; set; }

        public virtual ICollection<Note> Notes { get; set; }

        [ForeignKey("ApplicationUserId")]
        public virtual ApplicationUser ApplicationUser { get; set; }

        [ForeignKey("PatientId")]
        public virtual Patient Patient { get; set; }


        //public virtual ICollection<PatientUser> PatientUsers { get; set; }

        [ForeignKey("StatusTypeId")]
        public virtual StatusType StatusType { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (ReturnDate < DepartureDate)
            {
                yield return new ValidationResult("Hemkomstdatum måste vara avresedatum eller senare!");
            }
            else if ((ReturnDate == DepartureDate) && (ReturnTime < DepartureTime))
                {
                    yield return new ValidationResult("Hemkomsttid måste senare än avresetid!");
                }
        }


        // Set default values
        public TravelReport()
        {
            BreakfastDeduction = 0;
            LunchOrDinnerDeduction = 0;
            LunchAndDinnerDeduction = 0;
            AllMealsDeduction = 0;
            DepartureHoursExtra = 0;
            ReturnHoursExtra = 0;
            HalfDay = 0;
            FullDay = 0;
            Night = 0;
            //StatusTypeId = 4;
        }
    }
}