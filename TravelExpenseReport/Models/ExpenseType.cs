using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace TravelExpenseReport.Models
{
    public class ExpenseType
    {
        public int ExpenseTypeId { get; set; }

        [DisplayName("Utgiftstyp")]
        public string ExpenseTypeName { get; set; }


    }
}