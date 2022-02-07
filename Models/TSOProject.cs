using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NerolacPreviewApp.Models
{
    public class TSOProject
    {
        [Key]
        public int ProjectID { get; set; }

        public string MinSizeKBytes { get; set; }
        public string MaxSizeKBytes { get; set; }
        public string UserID { get; set; }
        [DisplayName("Customer Name")]
        [Required(ErrorMessage = "Enter correct information to avoid delays")]
        public string CustomerName { get; set; }
        [DisplayName("Project Name")]
        [Required(ErrorMessage = "Project Name Required")]
        public string ProjectName { get; set; }

        [DisplayName("Project Value")]
        [RegularExpression("([0-9][0-9]*)", ErrorMessage = "Optional project value in Rs")]
        [Range(0, 10000000, ErrorMessage = "Project Value must be between 0 to 10000000")]
        [Required(ErrorMessage = "Project Value must be between 0 to 10000000")]


        public string ProjectValue { get; set; } = "0";

        [DisplayName("Customer Contact No")]
        //[StringLength(10, MinimumLength = 10, ErrorMessage = "Customer Contact Number must be 10 digits number")]
        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "Invalid Mobile Number.")]
        public string CustomerContactNo { get; set; }
        [DisplayName("Customer EMail ID")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string CustomerEMailId { get; set; }

        [DisplayName("Site Address")]
        [Required(ErrorMessage = "Site Address Required")]
        public string SiteAddress { get; set; }
        public string Location { get; set; }

        [Required(ErrorMessage = "Please select HCA or SCA")]
        [DisplayName("Viz Option")]
        public string VizOption { get; set; }
        public string Remarks { get; set; }

        [DisplayName("Priority?")]
        public string Priority { get; set; }
        [DisplayName("Dealer Name")]
        public string DealerName { get; set; }
        [DisplayName("Dealer SAP Code")]
        [Required(ErrorMessage = "Dealer SAP Code required.")]
        public string DealerSAPCode { get; set; }

        [Required(ErrorMessage = "Please select Type")]
        [DisplayName("Type")]
        public string IorEorMS { get; set; }
        [DisplayName("Fresh Painting?")]
        public string FreshPainting { get; set; }
        [DisplayName("Image version 1 :")]
        public string ImageFileName1 { get; set; }

        public string ImageSelected1 { get; set; }
        [DisplayName("Image version 2 :")]
        public string ImageFileName2 { get; set; }
        [DisplayName("Image version 3 :")]
        public string ImageFileName3 { get; set; }
        [NotMapped]

        [Required(ErrorMessage = "Please select file.")]
        [DisplayName("Upload File1")]
        [AllowedExtensions(new string[] { ".jpg", ".jpeg" })]
        //[RegularExpression(@"([a-zA-Z0-9\s_\\.\-:])+(.jpeg|.jpg)$", ErrorMessage = "Only jpg/jpeg Image files allowed.")]
        public HttpPostedFileBase ImageFile1 { get; set; }
        [NotMapped]
       
        [DisplayName("Upload File2")]
        [AllowedExtensions(new string[] { ".jpg", ".jpeg" })]
        //[RegularExpression(@"([a-zA-Z0-9\s_\\.\-:])+(.jpeg|.jpg)$", ErrorMessage = "Only  jpg/jpeg Image files allowed.")]
        public HttpPostedFileBase ImageFile2 { get; set; }
        [NotMapped]
         
        [DisplayName("Upload File3")]
        [AllowedExtensions(new string[] { ".jpg", ".jpeg" })]
        //[RegularExpression(@"([a-zA-Z0-9\s_\\.\-:])+(.jpeg|.jpg)$", ErrorMessage = "Only jpg/jpeg Image files allowed.")]
        public HttpPostedFileBase ImageFile3 { get; set; }
        [DisplayName("Option 1 Body")]
        public string CPBody1 { get; set; }
        [DisplayName("Option 1 Border")]
        public string CPBorder1 { get; set; }
        [DisplayName("Option 1 Others")]
        public string CPHighlight1 { get; set; }
        [DisplayName("Option 2 Body")]
        public string CPBody2 { get; set; }
        [DisplayName("Option 2 Border")]
        public string CPBorder2 { get; set; }
        [DisplayName("Option 2 Others")]
        public string CPHighlight2 { get; set; }
        [DisplayName("Option 3 Body")]
        public string CPBody3 { get; set; }
      
        [DisplayName("Option 3 Border")]
        public string CPBorder3 { get; set; }
        [DisplayName("Option 3 Others")]
        public string CPHighlight3 { get; set; }
        [DisplayName("Any special request")]
        public string CPSplRequest { get; set; }
        
        public string Wstatus { get; set; }
        public DateTime WStatusDate { get; set; }
        public string SIOption { get; set; }
        public string SIFileName { get; set; }
        public string CaseID { get; set; }

        [DisplayName("Depot Code")]
        public string Depot { get; set; }
        public string C_ID { get; set; }
        public DateTime C_Date { get; set; }
        public string M_ID { get; set; }
        public DateTime M_Date { get; set; }
        public SelectList BuildingTypeList { get; set; }
        public string ImageType { get; set; }

        [DisplayName("Contact Name")]
        public string DepotContactName { get; set; }
        public string DepotRemarks { get; set; }

        [DisplayName("Depot Name")]
        public string DepotName { get; set; }

        [DisplayName("Door No/Street")]
        public string DepotAd1 { get; set; }

        [DisplayName("Addr.Line 2")]
        public string DepotAd2 { get; set; }

        [DisplayName("Town/City")]
        public string DepotAd3 { get; set; }

        [DisplayName("State")]
        public string DepotAd4 { get; set; }

        [DisplayName("Pincode")]
        [Required(ErrorMessage = "Please enter Pincode")]
        public string DepotPinCode { get; set; }

        [DisplayName("Delivery EMail ID")]
        public string DepotEmailId { get; set; }

        [DisplayName("Contact No")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "Depot Contact Number must be 10 digits number")]
        public string DepotContactNo { get; set; }

        public List<PreviousColourOptions> Comments { get; set; }

    }
    public class AllowedExtensionsAttribute : ValidationAttribute
    {
        private readonly string[] _extensions;

        public AllowedExtensionsAttribute(string[] extensions)
        {
            _extensions = extensions;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var file = value as HttpPostedFileBase;
            if (file != null)
            {
                var extension = Path.GetExtension(file.FileName);
                if (!_extensions.Contains(extension.ToLower()))
                {
                    return new ValidationResult(GetErrorMessage());
                }
                
            }
            return ValidationResult.Success;
        }

        public string GetErrorMessage()
        {
            return $"Only jpg/jpeg Image files allowed.";
        }

        public class PreviousColourOptions1
        {

            public string RSN { get; set; }
            public string Projectid { get; set; }
            public string Optionno { get; set; }
            public string code1 { get; set; }
            public string Name1 { get; set; }
            public string Colour1 { get; set; }

            public string code2 { get; set; }
            public string Name2 { get; set; }
            public string Colour2 { get; set; }
            public string code3 { get; set; }
            public string Name3 { get; set; }
            public string Colou3 { get; set; }
            public string hcode1 { get; set; }
            public string hcode2 { get; set; }
            public string hcode3 { get; set; }

        }

    }
}