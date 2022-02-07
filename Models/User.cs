using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace NerolacPreviewApp.Models
{
    public class User
    {
        [DisplayName("ID")]
        public string UserId { get; set; }

        [DisplayName("Name")]
        public string UserName { get; set; }

        [DisplayName("Department")]
        public string Department { get; set; }

        [DisplayName("Designation")]
        public string Designation { get; set; }

        [DisplayName("Location")]
        public string Location { get; set; }

        [DisplayName("State")]
        public string State { get; set; }

        [DisplayName("City")]
        public string City { get; set; }

        [DisplayName("Email ID")]
        public string MailID { get; set; }

        [DisplayName("Phone")]
        public string UserContactNo { get; set; }

        [DisplayName("Depot Code")]
        public string Depot { get; set; }

        [DisplayName("Depot Name")]
        public string DepotName { get; set; }

        [DisplayName("Depot Address")]
        public string DepotAd1 { get; set; }

        [DisplayName("Address")]
        public string DepotAd2 { get; set; }

        [DisplayName("Address")]
        public string DepotAd3 { get; set; }

        [DisplayName("Address")]
        public string DepotAd4 { get; set; }

        [DisplayName("PinCode")]
        public string DepotPinCode { get; set; }

    }
}