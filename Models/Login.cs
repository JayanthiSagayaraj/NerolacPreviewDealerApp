using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace NerolacPreviewApp.Models
{
    public class Login
    {
        [Required(ErrorMessage = "Please enter your User ID.")]
        [Display(Name = "UserId : ")]
        public string UserId { get; set; }
        

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Please enter your Password.")]
        [Display(Name = "Password : ")]
        public string Password { get; set; }

        public string UserPresent { get; set; }

        public string UserCategory { get; set; }

        public string UserLevel { get; set; }

        public string IsNewUser { get; set; }


    }

}