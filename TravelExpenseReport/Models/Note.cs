using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TravelExpenseReport.Models
{
    public class Note
    {
        public int NoteId { get; set; }
        public string NoteInfo { get; set; }
        public DateTime NoteTime { get; set; }
        public string NoteStatus { get; set; }

        public string ApplicationUserId { get; set; }

        public int? TravelReportId { get; set; }
        [ForeignKey("TravelReportId")]
        public virtual TravelReport TravelReport { get; set; }
}
}