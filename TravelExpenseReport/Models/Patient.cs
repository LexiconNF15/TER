using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TravelExpenseReport.Models
{
    public class Patient
    {
        public int PatientId { get; set; }
        public string Name { get; set; }
        public int CustomerId { get; set; }
    }
}