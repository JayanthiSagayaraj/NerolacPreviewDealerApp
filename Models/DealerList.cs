using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace NerolacPreviewApp.Models
{
    public class DealerList
    {
        [DisplayName("RSN")]
        public string RSN { get; set; }

        [DisplayName("Dealer_SAP_Code")]
        public string Dealer_SAP_Code { get; set; }

        [DisplayName("Dealer_Name")]
        public string Dealer_Name { get; set; }

        [DisplayName("Address_Line1")]
        public string Address_Line1 { get; set; }

        [DisplayName("Address_Line2")]
        public string Address_Line2 { get; set; }

        [DisplayName("Street")]
        public string Street { get; set; }

        [DisplayName("District")]
        public string District { get; set; }

        [DisplayName("PostalCode")]
        public string PostalCode { get; set; }

        [DisplayName("City")]
        public string City { get; set; }

        [DisplayName("State")]
        public string State { get; set; }

        [DisplayName("StateCode")]
        public string StateCode { get; set; }

        [DisplayName("LAT")]
        public string LAT { get; set; }

        [DisplayName("LONG")]
        public string LONG { get; set; }

        [DisplayName("TelephoneNo")]
        public string TelephoneNo { get; set; }

        [DisplayName("SMS_Mobile")]
        public string SMS_Mobile { get; set; }

        [DisplayName("Language")]
        public string Language { get; set; }

        [DisplayName("VR_User")]
        public string VR_User { get; set; }

        [DisplayName("StatusYN")]
        public string StatusYN { get; set; }

        [DisplayName("Remarks")]
        public string Remarks { get; set; }

        [DisplayName("C_ID")]
        public string C_ID { get; set; }

        [DisplayName("C_DATE")]
        public string C_DATE { get; set; }

        [DisplayName("M_ID")]
        public string M_ID { get; set; }

        [DisplayName("M_DATE")]
        public string M_DATE { get; set; }

    }
}