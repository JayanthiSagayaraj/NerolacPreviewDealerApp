using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NerolacPreviewApp.Models
{
    public class BizStatus
    {
        [Key]
        [DisplayName("Project Id")]
        public int ProjectId { get; set; }
        [DisplayName("Customer Name")]
        public string CustomerName { get; set; }
        [DisplayName("Business Status Date")]
        public DateTime StatusDate { get; set; }
        [DisplayName("Business Status")]
        public string BStatus { get; set; }
        [DisplayName("Status Reason")]
        public string StatusReason { get; set; }

    }
}