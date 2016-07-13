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
        public float MilageAmount { get; set; }

        [DisplayName("Frukost")]
        public int? BreakfastAmount { get; set; }

        [DisplayName("Lunch el. middag")]
        public int? LunchOrDinnerAmount { get; set; }

        [DisplayName("Lunch och middag")]
        public int? LunchAndDinnerAmount { get; set; }

        [DisplayName("Frukost, lunch och middag")]
        public int? AllMealsAmount { get; set; }
    }
}