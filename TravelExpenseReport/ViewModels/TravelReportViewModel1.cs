using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TravelExpenseReport.ViewModels
{
    public class TravelReportViewModel1
    {
  
        public TravelReportViewModel UserList
        {
            get; set; 

        }
        public IEnumerable<TravelExpenseReport.Models.TravelReport> SelectedTRUser
        { get; set; }


    }
}