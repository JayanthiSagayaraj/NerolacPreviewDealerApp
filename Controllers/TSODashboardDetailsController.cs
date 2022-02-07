using MySql.Data.MySqlClient;
using NerolacPreviewApp.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NerolacPreviewApp.Controllers
{
    public class TSODashboardDetailsController : Controller
    {
        // GET: TSODashboardDetails
        public ActionResult Index(int? ProjectID)
        {
            string constr = ConfigurationManager.ConnectionStrings["Nerolacconstr"].ConnectionString;
            MySqlConnection con = new MySqlConnection(constr);
            DataSet ds = new DataSet();
            MySqlCommand com = new MySqlCommand("Sp_PopulateTSODashboardDetails", con);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@Id", ProjectID);
            con.Open();
            //com.ExecuteNonQuery();
            MySqlDataAdapter ad = new MySqlDataAdapter(com);
            ad.Fill(ds);
            var ProjectDetail = ds.Tables[0].AsEnumerable();

            TSODashboardDetails model = new TSODashboardDetails()
            {
                ProjectID = Convert.ToInt32(ds.Tables[0].Rows[0]["ProjectID"]),
                UserID = Convert.ToString(ds.Tables[0].Rows[0]["UserID"]),
                CustomerName = Convert.ToString(ds.Tables[0].Rows[0]["CustomerName"]),
                ProjectName = Convert.ToString(ds.Tables[0].Rows[0]["ProjectName"]),
                ProjectValue = Convert.ToString(ds.Tables[0].Rows[0]["ProjectValue"]),
                CustomerContactNo = Convert.ToString(ds.Tables[0].Rows[0]["CustomerContactNo"]),
                CustomerEMailId = Convert.ToString(ds.Tables[0].Rows[0]["CustomerEMailId"]),
                SiteAddress = Convert.ToString(ds.Tables[0].Rows[0]["SiteAddress"]),
                Location = Convert.ToString(ds.Tables[0].Rows[0]["Location"]),
                VizOption = Convert.ToString(ds.Tables[0].Rows[0]["VizOption"]),
                Priority = Convert.ToString(ds.Tables[0].Rows[0]["Priority"]),
                DealerName = Convert.ToString(ds.Tables[0].Rows[0]["DealerName"]),
                DealerSAPCode = Convert.ToString(ds.Tables[0].Rows[0]["DealerSAPCode"]),
                IorEorMS = Convert.ToString(ds.Tables[0].Rows[0]["IorEorMS"]),
                FreshPainting = (Convert.ToString(ds.Tables[0].Rows[0]["FreshPainting"])) == "Y" ? "Yes"  : "No",
                ImageFileName1 = Convert.ToString(ds.Tables[0].Rows[0]["ImageFileName1"]),
                ImageFileName2 = Convert.ToString(ds.Tables[0].Rows[0]["ImageFileName2"]),
                ImageFileName3 = Convert.ToString(ds.Tables[0].Rows[0]["ImageFileName3"]),
                CPBody1 = Convert.ToString(ds.Tables[0].Rows[0]["CPBody1"]),
                CPBorder1 = Convert.ToString(ds.Tables[0].Rows[0]["CPBorder1"]),
                CPHighlight1 = Convert.ToString(ds.Tables[0].Rows[0]["CPHighlight1"]),
                CPBody2 = Convert.ToString(ds.Tables[0].Rows[0]["CPBody2"]),
                CPBorder2 = Convert.ToString(ds.Tables[0].Rows[0]["CPBorder2"]),
                CPHighlight2 = Convert.ToString(ds.Tables[0].Rows[0]["CPHighlight2"]),
                CPBody3 = Convert.ToString(ds.Tables[0].Rows[0]["CPBody3"]),
                CPBorder3 = Convert.ToString(ds.Tables[0].Rows[0]["CPBorder3"]),
                CPHighlight3 = Convert.ToString(ds.Tables[0].Rows[0]["CPHighlight3"]),
                CPSplRequest = Convert.ToString(ds.Tables[0].Rows[0]["CPSplRequest"]),
                Wstatus = Convert.ToString(ds.Tables[0].Rows[0]["Wstatus"]),
                WstatusDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["WstatusDate"]),
                SIOption = Convert.ToString(ds.Tables[0].Rows[0]["SIOption"]),
                SIFileName = Convert.ToString(ds.Tables[0].Rows[0]["SIFileName"]),
                CaseID = Convert.ToString(ds.Tables[0].Rows[0]["CaseID"]),
                Depot = Convert.ToString(ds.Tables[0].Rows[0]["Depot"]),
                C_ID = Convert.ToString(ds.Tables[0].Rows[0]["C_ID"]),
                C_Date = Convert.ToDateTime(ds.Tables[0].Rows[0]["C_Date"]),
                M_ID = Convert.ToString(ds.Tables[0].Rows[0]["M_ID"]),
                M_Date = Convert.ToDateTime(ds.Tables[0].Rows[0]["M_Date"]),
                DepotName = Convert.ToString(ds.Tables[0].Rows[0]["DepotName"]),
                DepotAd1 = Convert.ToString(ds.Tables[0].Rows[0]["DepotAd1"]),
                DepotAd2 = Convert.ToString(ds.Tables[0].Rows[0]["DepotAd2"]),
                DepotAd3 = Convert.ToString(ds.Tables[0].Rows[0]["DepotAd3"]),
                DepotAd4 = Convert.ToString(ds.Tables[0].Rows[0]["DepotAd4"]),
                DepotEmailId = Convert.ToString(ds.Tables[0].Rows[0]["DepotEmailId"]),
                DepotContactName = Convert.ToString(ds.Tables[0].Rows[0]["DepotContactName"]),
                DepotContactNo = Convert.ToString(ds.Tables[0].Rows[0]["DepotContactNo"]),
                DepotPinCode = Convert.ToString(ds.Tables[0].Rows[0]["DepotPinCode"]),
                FileSize = Convert.ToString(ds.Tables[0].Rows[0]["FileSize"]),
                Resolution = Convert.ToString(ds.Tables[0].Rows[0]["Resolution"]),
                InSyComments = Convert.ToString(ds.Tables[0].Rows[0]["InSyComments"])
            };
            con.Close();
            return View(model);
        }
    }
}