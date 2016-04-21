using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TravelExpenseReport.Models
{
    public class LegalAmount
    {
        public int LegalAmountId { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime ValidDate { get; set; }
        [DisplayName("Heldagstraktamente")]
        public int? FullDayAmount { get; set; }
        [DisplayName("Halvdagstraktamente")]
        public int? HalfDayAmount { get; set; }
        [DisplayName("Natttraktamente")]
        public int? NightAmount { get; set; }
        [DisplayName("Kilometerbelopp")]
        public int? MilageAmount { get; set; }
        [DisplayName("Frukostavdrag")]
        public int? BreakfastReductionAmount { get; set; }
        [DisplayName("Lunchavdrag")]
        public int? LunchReductionAmount { get; set; }
        [DisplayName("Middagsavdrag")]
        public int? DinnerReductionAmount { get; set; }

    }
}