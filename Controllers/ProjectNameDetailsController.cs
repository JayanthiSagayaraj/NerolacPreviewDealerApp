using MySql.Data.MySqlClient;
using NerolacPreviewApp.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Drawing;
using System.Xml;
namespace NerolacPreviewApp.Controllers
{
    public class ProjectNameDetailsController : Controller
    {
        // GET: ProjectNameDetails
        public ActionResult Index(int? ProjectID)
        {
            string constr = ConfigurationManager.ConnectionStrings["Nerolacconstr"].ConnectionString;
            MySqlConnection con = new MySqlConnection(constr);
            DataSet ds = new DataSet();
            MySqlCommand com = new MySqlCommand("Sp_ProjectNameDetails", con);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@Id", ProjectID);
            con.Open();
            //com.ExecuteNonQuery();
            MySqlDataAdapter ad = new MySqlDataAdapter(com);
            ad.Fill(ds);
            var ProjectDetail = ds.Tables[0].AsEnumerable();

            ProjectNameDetails model = new ProjectNameDetails()
            {
                ProjectID = Convert.ToInt32(ds.Tables[0].Rows[0]["ProjectID"]),
                UserID = Convert.ToString(ds.Tables[0].Rows[0]["UserID"]),
                ProjectName = Convert.ToString(ds.Tables[0].Rows[0]["ProjectName"]),
                WStatus = Convert.ToString(ds.Tables[0].Rows[0]["WStatus"]),
                Submittedimage1 = Convert.ToString(ds.Tables[0].Rows[0]["ImageFileName1"]),
                Submittedimage2 = Convert.ToString(ds.Tables[0].Rows[0]["ImageFileName2"]),
                Submittedimage3 = Convert.ToString(ds.Tables[0].Rows[0]["ImageFileName3"]),
                SIFilename = Convert.ToString(ds.Tables[0].Rows[0]["SIFilename"]),
                ws = Convert.ToString(ds.Tables[0].Rows[0]["ws"]),
                priimage= Convert.ToString(ds.Tables[0].Rows[0]["ImageFileName"]),
                VizOption= Convert.ToString(ds.Tables[0].Rows[0]["VizOptionDesc"])

            };
            con.Close();
            return View(model);
        }
       
       
    }
}