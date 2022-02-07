using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NerolacPreviewApp.Models
{
    public class Help
    {
        [Key]
        [DisplayName("Ref#")]
        public decimal ChapterNumber { get; set; }

        [DisplayName("Title")]
        public string MainTitle  { get; set; }
        public string SubTitle { get; set; }

        [DisplayName("InBrief")]
        public string Shortdescription { get; set; } 
        public string DetailsSet1 { get; set; }

        public string DetailsSet2 { get; set; }
        public string Attachment { get; set; }
        public string Tags { get; set; }
        public string c_date { get; set; }

        public string n { get; set; }
        public string s { get; set; }

    }
}