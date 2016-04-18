using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TravelExpenseReport.Models
{
    public class TravelReport
    {
        public int TravelReportId { get; set; }

        public string ApplicationUserId { get; set; }

        [DisplayName("Reseräkningsnummer")]
        public string TravelReportName { get; set; }

        [DisplayName("Resmål")]
        public string Destination { get; set; }

        [DisplayName("Syfte")]
        public string Purpose { get; set; }

       
        [DisplayName("Avresa datum")]
        [DataType(DataType.Date)]
        public DateTime DepartureDate { get; set; }

        [DisplayName("Avresa tid")]
        [DataType(DataType.Time)]
        public DateTime DepartureTime { get; set; }

        [DisplayName("Hemkomst datum")]
        [DataType(DataType.Date)]
        public DateTime ReturnDate { get; set; }

        [DisplayName("Hemkomst tid")]
        [DataType(DataType.Time)]
        public DateTime ReturnTime { get; set; }

        [DisplayName("Avresa extra timmar")]
        public int? DepartureHoursExtra { get; set; }

        [DisplayName("Hemresa extra timmar")]
        public int? ReturnHoursExtra { get; set; }

        [DisplayName("Heldag")]
        public int? FullDay { get; set; }

        [DisplayName("Halvdag")]
        public int? HalfDay { get; set; }

        [DisplayName("Natt")]
        public int? Night { get; set; }


        [DisplayName("Avdrag frukost")]
        public int? BreakfastReduction { get; set; }

        [DisplayName("Avdrag lunch")]
        public int? LunchReduction { get; set; }

        [DisplayName("Avdrag middag")]
        public int? DinnerReduction { get; set; }

        [DisplayName("Status")]
        public string Status { get; set; }

        [DisplayName("Kommentar")]
        public string Commment { get; set; }

        public virtual ICollection<Expense> Expenses { get; set; }

        [ForeignKey("ApplicationUserId")]
        public virtual ApplicationUser ApplicationUser { get; set; }

    }
}