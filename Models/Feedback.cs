using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NerolacPreviewApp.Models
{
    public class Feedback
    {
        [Key]
        [DisplayName("Ref#")]
        public int FeedbackRef { get; set; }
        public string ProjectId { get; set; }
        public string ProjectName { get; set; }

        [DisplayName("Nerolite ID")]
        public string TSOId { get; set; }
        public DateTime Date { get; set; }
        public string Category { get; set; }

        [DisplayName("Feedback")]
        [Required(ErrorMessage = "Enter Feedback")]
        public string TSOInput { get; set; }
        [DisplayName("Reply by Support Team")]
        public string ReplyGiven { get; set; }
        public string ReplyBy { get; set; }

        [DisplayName("Replied")]
        public string RDate { get; set; } 

    }
}