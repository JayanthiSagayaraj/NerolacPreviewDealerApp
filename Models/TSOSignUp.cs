using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace NerolacPreviewApp.Models
{
    public class TSOSignUp
    {
        public string UserId { get; set; }
        [DisplayName("User Name")]
        public string UserName { get; set; }
        [DisplayName("User ContactNo")]
        public string UserContactNo { get; set; }
        [DisplayName("User EmailId")]
        public string UserEmailId { get; set; }
        public string Depot { get; set; }
    }
}