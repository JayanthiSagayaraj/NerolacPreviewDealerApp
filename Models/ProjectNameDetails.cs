using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;


namespace NerolacPreviewApp.Models
{
    public class ProjectNameDetails
    {
        public Int32 ProjectID { get; set; }
        public string UserID { get; set; }
        public string ProjectName { get; set; }

        public string WStatus { get; set; }

        [Display(Name = "Submitted Image 1")] 
        public string Submittedimage1 { get; set; }
        public string Submittedimage2 { get; set; }
        public string Submittedimage3 { get; set; }
        public string SIFilename { get; set; }
        public string priimage { get; set; }
        public string ws { get; set; }
        public string VizOption { get; set; }
    }
}