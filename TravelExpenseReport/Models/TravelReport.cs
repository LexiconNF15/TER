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

        [DisplayName("Reseräkningsnummer")]
        public string TravelReportName { get; set; }

        [DisplayName("Resmål")]
        public string Destination { get; set; }

        [DisplayName("Syfte")]
        public string Purpose { get; set; }

        [Required]
        [DisplayName("Avresa datum")]
        [DataType(DataType.DateTime)]
        public DateTime DepartureDateTime { get; set; }

        [Required]
        [DisplayName("Hemkomst datum")]
        [DataType(DataType.DateTime)]
        public DateTime ReturnDateTime { get; set; }

        [DisplayName("Avresa extra timmar")]
        public int DepartureHoursExtra { get; set; }

        [DisplayName("Hemresa extra timmar")]
        public int ReturnHoursExtra { get; set; }

        [DisplayName("Heldag")]
        public int FullDay { get; set; }

        [DisplayName("Halvdag")]
        public int HalfDay { get; set; }

        [DisplayName("Natt")]
        public int Night { get; set; }


        [DisplayName("Avdrag frukost")]
        public int BreakfastReduction { get; set; }

        [DisplayName("Avdrag lunch")]
        public int LunchReduction { get; set; }

        [DisplayName("Avdrag middag")]
        public int DinnerReduction { get; set; }

        [DisplayName("Status")]
        public string Status { get; set; }

        [DisplayName("Kommentar")]
        public string Commment { get; set; }

        public virtual ICollection<Expense> Expenses { get; set; }

    

        public string AId { get; set; }
        [ForeignKey("AId")]
        public virtual ApplicationUser Users { get; set; }

    }
}