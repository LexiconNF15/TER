using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TravelExpenseReport.ViewModels
{
    public class SelectionAndTRViewModel
    {
  
        public SelectTravelUserViewModel SelectionList
        {
            get; set; 

        }
        public IEnumerable<TravelExpenseReport.Models.TravelReport> SelectedUserTravelReports
        { get; set; }


    }
}