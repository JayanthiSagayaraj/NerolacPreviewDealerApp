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
    public class ChangePasswordController : Controller
    {
        // GET: ChangePassword
        public ActionResult ChangePassword()
        {
            return View();
        }

      

        [HttpPost]
        public JsonResult UpdatePassword(string jsPassword)
        {
            try
            {
                List<ChangePassword> userlist = new List<ChangePassword>();
                string constr = ConfigurationManager.ConnectionStrings["Nerolacconstr"].ConnectionString;
                string userid = Convert.ToString(Session["UserID"]);
                using (MySqlConnection con = new MySqlConnection(constr))
                {
                    con.Open();
                    MySqlDataAdapter da = new MySqlDataAdapter("select * from tblsecpreviewusers where UserID='" + userid + "'", con);
                    DataSet ds = new DataSet();
                    da.Fill(ds);
                    MySqlCommand cmd = new MySqlCommand("update tblsecpreviewusers set userpin='" + jsPassword + "' where UserID='" + userid + "'" ,con);
                    int querystatus = cmd.ExecuteNonQuery();
                    if (querystatus > 0)
                    {
                        return Json(userlist, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(userlist, JsonRequestBehavior.AllowGet);
                    }
                    
                }
            }
            catch (Exception ex)
            {
                return Json(ex.Message.ToString(), JsonRequestBehavior.AllowGet);
            }
        }
    }
}