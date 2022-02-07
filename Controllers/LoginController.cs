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
    public class LoginController : Controller
    {
        public ActionResult Index()
        {
            Session["NWH"] = ListTSO.GetNWH(DateTime.Now);
            Session["BannerMessage"] = ListTSO.GetBannerMessage();
            return View();
        }

        [HttpPost]
        public JsonResult UserLogin(string jsUserId, string jsPassword)
        {
            try
            {
                string constr = ConfigurationManager.ConnectionStrings["Nerolacconstr"].ConnectionString;
                using (MySqlConnection con = new MySqlConnection(constr))
                {
                    List<Login> userlist = new List<Login>();
                    DataSet ds = new DataSet();
                    MySqlCommand com = new MySqlCommand("SP_Validateuserdlr", con);
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@iUsername", jsUserId);
                    com.Parameters.AddWithValue("@iPassword", jsPassword);
                    con.Open();
                    //com.ExecuteNonQuery();
                    MySqlDataAdapter ad = new MySqlDataAdapter(com);
                    ad.Fill(ds);
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0 && ds.Tables[0].Rows[0]["sCode"].ToString() == "1")
                    {
                        Login usercrdential = new Login();
                        String UserID = Convert.ToString(ds.Tables[0].Rows[0]["UserID"]);
                        String UserPIN = Convert.ToString(ds.Tables[0].Rows[0]["UserPIN"]);
                        usercrdential.UserId = Convert.ToString(ds.Tables[0].Rows[0]["UserID"]);
                        usercrdential.Password = Convert.ToString(ds.Tables[0].Rows[0]["UserPIN"]);
                        usercrdential.UserCategory = ds.Tables[0].Rows[0]["UserCategory"].ToString();
                        usercrdential.UserLevel = ds.Tables[0].Rows[0]["UserLevel"].ToString();
                        if ( UserID.ToLower() == UserPIN.ToLower())
                        {
                            usercrdential.IsNewUser = "true";
                        }
                        MySqlDataAdapter da = new MySqlDataAdapter("select C_DATE from loginlog where userid='"+ UserID + "' order by C_DATE DESC LIMIT 2,1", con);
                        DataSet dss = new DataSet();
                        da.Fill(dss);
                        if (dss.Tables[0].Rows.Count > 0)
                        {
                            Session["LastLogin"] = Convert.ToDateTime(dss.Tables[0].Rows[0]["C_DATE"]).ToString("ddd, dd MMM yyy hh:mm:ss tt");
                        }
                        else
                        {
                            Session["LastLogin"] = "";
                        }
                        MySqlDataAdapter dahelp = new MySqlDataAdapter("select count(rsn) from cwhelp where date_format(c_date,'%Y-%m-%d') >= date_format(curdate()-7,'%Y-%m-%d') and date_format(c_date,'%Y-%m-%d') <= date_format(curdate(),'%Y-%m-%d')", con);
                        DataSet dsshelp = new DataSet();
                        dahelp.Fill(dsshelp);
                        if (Convert.ToInt32(dsshelp.Tables[0].Rows[0][0]) > 0)
                        {
                            Session["help"] = 1;
                        }
                        else
                        {
                            Session["help"] = 0;
                        }
                        string Region = "";
                        MySqlDataAdapter da1 = new MySqlDataAdapter("select region from knusermaster where empid='" + UserID + "'", con);
                        DataSet ds1 = new DataSet();
                        da1.Fill(ds1);
                        if (ds1.Tables[0].Rows[0][0] != null)
                            Region = ds1.Tables[0].Rows[0][0].ToString();
                        MySqlDataAdapter dafm = new MySqlDataAdapter("select MessageTitle from flashmessage where (current_timestamp() between messagestartdate and " +
            "messageenddate) and (case when MessageFor = 'Region Dealers' then Region = '"+Region+"' when MessageFor = 'Specific Dealer' then  Dealer = '"+UserID+"' else MessageFor = 'All Dealers' end)", con);
                        DataSet dsfm = new DataSet();
                        dafm.Fill(dsfm);

                        string fmval = "0";
                        if (dsfm.Tables[0].Rows.Count > 0)
                            fmval = "1";
                        //(select count(rsn) from cwhelp where date_format(c_date,'%Y-%m-%d') >= date_format(curdate()-7,'%Y-%m-%d') and date_format(c_date,'%Y-%m-%d') <= date_format(curdate(),'%Y-%m-%d'))
                        Session["UserID"] = ds.Tables[0].Rows[0]["UserID"].ToString();
                        Session["UserPin"] = ds.Tables[0].Rows[0]["UserPIN"].ToString();
                        Session["UserLevel"] = ds.Tables[0].Rows[0]["UserLevel"].ToString();
                        Session["UserName"] = ds.Tables[0].Rows[0]["UserName"].ToString();
                        Session["UserCategory"] = ds.Tables[0].Rows[0]["UserCategory"].ToString();
                        Session["Depot"] = ds.Tables[1].Rows[0]["DepotCode"].ToString();
                        
                        Session["FlashMessages"] = fmval;
                        Session["code1opt1"] = "";
                        userlist.Add(usercrdential);
                    }
                    return Json(userlist, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(ex.Message.ToString(), JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult Logout()
        {
            string Userid = Session["UserID"].ToString();
            string constr = ConfigurationManager.ConnectionStrings["Nerolacconstr"].ConnectionString;
            MySqlConnection MyConn2 = new MySqlConnection(constr);
            MySqlCommand com = new MySqlCommand("Sp_InsertLogoutLog", MyConn2);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@UserID", Userid);
            MyConn2.Open();
            com.ExecuteNonQuery();
            MyConn2.Close();
            Session.Abandon();
            return RedirectToAction("Index", "Login");
        }

        public ActionResult Forgotpwd()
        {
            return View();
        }

        public ActionResult Resetpwd()
        {
            return View();
        }

        [HttpPost]
        public JsonResult ResetPassword(string jsUserId)
        {
            try
            {
                string constr = ConfigurationManager.ConnectionStrings["Nerolacconstr"].ConnectionString;
                using (MySqlConnection con = new MySqlConnection(constr))
                {
                    List<Login> userlist = new List<Login>();
                    DataSet ds = new DataSet();
                    MySqlCommand com = new MySqlCommand("SP_ResetPwd", con);
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@iUsername", jsUserId);
                    con.Open();
                    //com.ExecuteNonQuery();
                    MySqlDataAdapter ad = new MySqlDataAdapter(com);
                    ad.Fill(ds);
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0 && ds.Tables[0].Rows[0]["sCode"].ToString() == "1")
                    {
                        Login usercrdential = new Login();
                        String UserID = Convert.ToString(ds.Tables[0].Rows[0]["UserID"]);
                        String UserPIN = Convert.ToString(ds.Tables[0].Rows[0]["UserPIN"]);
                        usercrdential.UserId = Convert.ToString(ds.Tables[0].Rows[0]["UserID"]);
                        usercrdential.Password = Convert.ToString(ds.Tables[0].Rows[0]["UserPIN"]);
                        usercrdential.UserCategory = ds.Tables[0].Rows[0]["UserCategory"].ToString();
                        usercrdential.UserLevel = ds.Tables[0].Rows[0]["UserLevel"].ToString();
                        if (UserID.ToLower() == UserPIN.ToLower())
                        {
                            usercrdential.IsNewUser = "true";
                        }                    
                        userlist.Add(usercrdential);
                    }
                    return Json(userlist, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(ex.Message.ToString(), JsonRequestBehavior.AllowGet);
            }

        }
    }
}