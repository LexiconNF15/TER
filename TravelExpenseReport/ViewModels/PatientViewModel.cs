using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TravelExpenseReport.ViewModels
{
    public class PatientViewModel
    {
        public IEnumerable<TravelExpenseReport.Models.TravelReport> SelectedTR
        { get; set; }
        public IEnumerable<TravelExpenseReport.Models.PatientUser> SelectedPUser
        { get; set; }
        public TravelExpenseReport.Models.Patient SelectedP
        { get; set; }

    }
}