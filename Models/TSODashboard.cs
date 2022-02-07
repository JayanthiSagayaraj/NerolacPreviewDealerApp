using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace NerolacPreviewApp.Models
{
    public class TSODashboard
    {
        public string ProjectID { get; set; }

        public string ProjectName { get; set; }

        [Display(Name = "UserID")]
        public string UserID { get; set; }

        [Display(Name = "Customer")]
        public string CustomerName { get; set; }

        [Display(Name = "Site")]
        public string SiteAddress { get; set; }

        [Display(Name = "Status")]
        public string Wstatus { get; set; }

        [Display(Name = "Status Date")]
        public DateTime WstatusDate { get; set; }

        [Display(Name = "Workstatus")]
        public int Workstatus { get; set; }

        [Display(Name = "Expires on")]
        public string ProjectExpiryDate { get; set; }

        [Display(Name = "Visualization Option")]
        public string VizOption { get; set; }

        [Display(Name = "Business Status")]
        public string Bstatus { get; set; }
        public int PRI { get; set; }

        public int SI { get; set; }
        public string NWH { get; set; }

        public string AllowNewProject { get; set; }

        public string DisallowMessage { get; set; }

        public string BannerMessage { get; set; }

        public string curdate { get; set; }

        public string CustomerContactNo { get; set; }
        public string CPBody1 { get; set; }
        public string CPBorder1 { get; set; }
        public string CPHighlight1 { get; set; }
        public string CPBody2 { get; set; }
        public string CPBorder2 { get; set; }
        public string CPHighlight2 { get; set; }
        public string CPBody3 { get; set; }
        public string CPBorder3 { get; set; }
        public string CPHighlight3 { get; set; }
        public string CPSplRequest { get; set; }
        public string InSyComments { get; set; }
    }

    public class TSODashboardDetails
    {
        public Int32 ProjectID { get; set; }
        public string UserID { get; set; }
        public string CustomerName { get; set; }
        public string ProjectName { get; set; }
        public string ProjectValue { get; set; }
        public string CustomerContactNo { get; set; }
        public string CustomerEMailId { get; set; }
        public string SiteAddress { get; set; }
        public string Location { get; set; }
        public string VizOption { get; set; }
        public string Priority { get; set; }
        public string DealerName { get; set; }
        public string DealerSAPCode { get; set; }
        public string IorEorMS { get; set; }
        public string FreshPainting { get; set; }
        public string ImageFileName1 { get; set; }
        public string ImageFileName2 { get; set; }
        public string ImageFileName3 { get; set; }
        public string CPBody1 { get; set; }
        public string CPBorder1 { get; set; }
        public string CPHighlight1 { get; set; }
        public string CPBody2 { get; set; }
        public string CPBorder2 { get; set; }
        public string CPHighlight2 { get; set; }
        public string CPBody3 { get; set; }
        public string CPBorder3 { get; set; }
        public string CPHighlight3 { get; set; }
        public string CPSplRequest { get; set; }
        public string Wstatus { get; set; }
        public DateTime WstatusDate { get; set; }
        public string SIOption { get; set; }
        public string SIFileName { get; set; }
        public string CaseID { get; set; }
        public string Depot { get; set; }
        public string C_ID { get; set; }
        public DateTime C_Date { get; set; }
        public string M_ID { get; set; }
        public DateTime M_Date { get; set; }

        [DisplayName("ContactName")]
        public string DepotContactName { get; set; }

        [DisplayName("Delivery to")]
        public string DepotName { get; set; }

        [DisplayName("AddressL1")]
        public string DepotAd1 { get; set; }

        [DisplayName("AddressL2")]
        public string DepotAd2 { get; set; }

        [DisplayName("AddressL4")]
        public string DepotAd3 { get; set; }

        [DisplayName("Depot")]
        public string DepotAd4 { get; set; }

        [DisplayName("PinCode")]
        public string DepotPinCode { get; set; }

        [DisplayName("DeliveryEMailID")]
        public string DepotEmailId { get; set; }

        [DisplayName("ContactNo")]
        public string DepotContactNo { get; set; }
        public string FileSize { get; set; }
        public string Resolution { get; set; }
        public string InSyComments { get; set; }
    }

}