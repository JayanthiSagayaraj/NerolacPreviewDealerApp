using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NerolacPreviewApp.Models
{
    public class Stats
    {
        [Key]
        public int ProjectID { get; set; }
        public int ProjectsToday { get; set; }
        public int TSOInvolvedToday { get; set; }
        public int SCACountToday { get; set; }
        public int HCACountToday { get; set; }

        public int ProjectsYesterday { get; set; }
        public int TSOInvolvedYesterday { get; set; }
        public int SCACountYesterday { get; set; }
        public int HCACountYesterday { get; set; }

        public int Projects { get; set; }
        public int TSOInvolved { get; set; }
        public int SCACount { get; set; }
        public int HCACount { get; set; }

    }
}