using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TravelExpenseReport.Models
{
    public class LegalAmount
    {
        public int LegalAmountId { get; set; }
        public DateTime ValidDate { get; set; }
        public int FullDayAmount { get; set; }
        public int HalfDayAmount { get; set; }
        public int NightAmount { get; set; }
        public float MilageAmount { get; set; }
    }
}