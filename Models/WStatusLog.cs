using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NerolacPreviewApp.Models
{
    public class WStatusLog
    {
        public Int32 ProjectID { get; set; }

        [Display(Name = "ProjectName")]
        public string ProjectName { get; set; }

        [Display(Name = "Customer")]
        public string CustomerName { get; set; }

        [Display(Name = "WorkStatus")]
        public string WorkStatus { get; set; }

        [Display(Name = "AssignedTo")]
        public string AssignedTo { get; set; }

        [Display(Name = "StatusDate")]
        public DateTime M_Date { get; set; }

        [Display(Name = "TSO")]
        public string UserID { get; set; }

    }
}