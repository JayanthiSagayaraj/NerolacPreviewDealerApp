using MySql.Data.MySqlClient;
using NerolacPreviewApp.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NerolacPreviewApp.Controllers
{
    public class TSODashboardController : Controller
    {
        // GET: TSODashboard
        public ActionResult Index(string sortOrder, string q, int page = 1, int pageSize = 10)
        {
            ViewBag.searchQuery = String.IsNullOrEmpty(q) ? "" : q;

            page = page > 0 ? page : 1;
            pageSize = pageSize > 0 ? pageSize : 5;

            ViewBag.ProjectIDSortParam = sortOrder == "ProjectID" ? "ProjectID_desc" : "ProjectID";
            ViewBag.WstatusSortParam = sortOrder == "Wstatus" ? "Wstatus_desc" : "Wstatus";
            ViewBag.CaseIDSortParam = sortOrder == "CaseID" ? "CaseID_desc" : "CaseID";

            ViewBag.CurrentSort = sortOrder;

            List<TSODashboard> tsodashboardmodel = new List<TSODashboard>();
            string constr = ConfigurationManager.ConnectionStrings["Nerolacconstr"].ConnectionString;
            string userid = Convert.ToString(Session["UserID"]);
            ViewBag.AllowNewProject = ListTSO.GetAllowNewProject(userid).AllowNewProject.ToUpper().Trim();
            Session["DisallowMessage"] = ListTSO.GetAllowNewProject(userid).DisallowMessage;
            ViewBag.PRI = ListTSO.GetPRI(userid);
            ViewBag.SI = ListTSO.GetSI(userid);
            Session["NWH"] = ListTSO.GetNWH(DateTime.Now);
            using (MySqlConnection con = new MySqlConnection(constr))
            {
                DataSet ds = new DataSet();
                MySqlCommand com = new MySqlCommand("Sp_PopulateTSODashboard_new", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@UserId", userid);
                con.Open();

                string curdate = string.Empty;
                MySqlDataAdapter da = new MySqlDataAdapter("select curdate()", con);
                DataSet dss = new DataSet();
                da.Fill(dss);
                if (dss.Tables[0].Rows.Count > 0)
                {
                    DateTime curdate2 = Convert.ToDateTime(dss.Tables[0].Rows[0][0]);

                    curdate = curdate2.ToString("dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                }
                List<TSODashboard> lstTSODashboard = new List<TSODashboard>();
                var ProjectList = new List<TSODashboard>();

                using (MySqlDataReader sdr = com.ExecuteReader())
                {
                    while (sdr.Read())
                    {
                        string dates = string.Empty;
                        if ((sdr["ProjectExpiryDate"] != null || sdr["ProjectExpiryDate"] != DBNull.Value) && sdr["ProjectExpiryDate"].ToString() != "NA")
                        {
                            //DateTime dates2 = Convert.ToDateTime(sdr["ProjectExpiryDate"]).tos;
                            dates = (sdr["ProjectExpiryDate"]).ToString();

                        }

                        else
                        {
                            dates = "NA";
                        }
                        Session["ProjectExpiryDate"] = dates;
                        Session["CURDATE"] = curdate;
                        string s = "0";

                        ProjectList.Add(new TSODashboard
                        {
                            ProjectID = sdr["ProjectID"].ToString(),
                            ProjectName = sdr["ProjectName"].ToString(),
                            UserID = sdr["UserID"].ToString(),
                            CustomerName = sdr["CustomerName"].ToString(),
                            SiteAddress = sdr["SiteAddress"].ToString(),
                            Wstatus = sdr["WorkStatusDesc"].ToString(),
                            WstatusDate = Convert.ToDateTime(sdr["WstatusDate"]),
                            Workstatus = Convert.ToInt32(sdr["Wstatus"]),

                            ProjectExpiryDate = ((dates != "NA") ? (dates) : "NA"),

                            VizOption = sdr["VizOptionDesc"].ToString(),
                            Bstatus = sdr["BStatusDesc"].ToString(),
                            curdate = (sdr["curdate"]).ToString()
                        });
                    }
                }
                var list = ProjectList.ToList();

                if (!String.IsNullOrEmpty(q))
                {
                    q = q.ToLower();
                    list = list.Where(s => s.ProjectID.ToLower().Contains(q) || s.ProjectName.ToLower().Contains(q) || s.UserID.ToLower().Contains(q) || s.CustomerName.ToLower().Contains(q) || s.SiteAddress.ToLower().Contains(q) || s.Wstatus.ToLower().Contains(q) || s.VizOption.ToLower().Contains(q) || s.Bstatus.ToLower().Contains(q)).ToList();
                }


                switch (sortOrder)
                {
                    case "ProjectID_desc":
                        list = list.OrderByDescending(x => x.ProjectID).ToList();
                        break;
                    case "Wstatus_desc":
                        list = list.OrderByDescending(x => x.Wstatus).ToList();
                        break;
                    case "ProjectID":
                        list = list.OrderBy(x => x.ProjectID).ToList();
                        break;
                    case "Wstatus":
                        list = list.OrderBy(x => x.Wstatus).ToList();
                        break;
                    default:
                        break;
                }
                con.Close();
                return View(list.ToPagedList(page, pageSize));
            }
        }
        //for AllLevel projects
        public ActionResult AllLevelsDashboard(string sortOrder, string q, int page = 1, int pageSize = 10)
        {
            ViewBag.searchQuery = String.IsNullOrEmpty(q) ? "" : q;

            page = page > 0 ? page : 1;
            pageSize = pageSize > 0 ? pageSize : 5;

            ViewBag.ProjectIDSortParam = sortOrder == "ProjectID" ? "ProjectID_desc" : "ProjectID";
            ViewBag.WstatusSortParam = sortOrder == "Wstatus" ? "Wstatus_desc" : "Wstatus";
            ViewBag.CaseIDSortParam = sortOrder == "CaseID" ? "CaseID_desc" : "CaseID";

            ViewBag.CurrentSort = sortOrder;

            List<TSODashboard> tsodashboardmodel = new List<TSODashboard>();
            string constr = ConfigurationManager.ConnectionStrings["Nerolacconstr"].ConnectionString;
            string userid = Convert.ToString(Session["UserID"]);
            ViewBag.AllowNewProject = ListTSO.GetAllowNewProject(userid).AllowNewProject.ToUpper().Trim();
            Session["DisallowMessage"] = ListTSO.GetAllowNewProject(userid).DisallowMessage;
            ViewBag.PRI = ListTSO.GetPRI(userid);
            ViewBag.SI = ListTSO.GetSI(userid);
            Session["NWH"] = ListTSO.GetNWH(DateTime.Now);
            using (MySqlConnection con = new MySqlConnection(constr))
            {
                DataSet ds = new DataSet();
                MySqlCommand com = new MySqlCommand("Sp_AllLevelPopulateTSODashboard_new", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@UserId", userid);
                con.Open();

                string curdate = string.Empty;
                MySqlDataAdapter da = new MySqlDataAdapter("select curdate()", con);
                DataSet dss = new DataSet();
                da.Fill(dss);
                if (dss.Tables[0].Rows.Count > 0)
                {
                    DateTime curdate2 = Convert.ToDateTime(dss.Tables[0].Rows[0][0]);

                    curdate = curdate2.ToString("dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                }
                List<TSODashboard> lstTSODashboard = new List<TSODashboard>();
                var ProjectList = new List<TSODashboard>();

                using (MySqlDataReader sdr = com.ExecuteReader())
                {
                    while (sdr.Read())
                    {
                        string dates = string.Empty;
                        if ((sdr["ProjectExpiryDate"] != null || sdr["ProjectExpiryDate"] != DBNull.Value) && sdr["ProjectExpiryDate"].ToString() != "NA")
                        {
                            //DateTime dates2 = Convert.ToDateTime(sdr["ProjectExpiryDate"]).tos;
                            dates = (sdr["ProjectExpiryDate"]).ToString();

                        }

                        else
                        {
                            dates = "NA";
                        }
                        Session["ProjectExpiryDate"] = dates;
                        Session["CURDATE"] = curdate;
                        string s = "0";

                        ProjectList.Add(new TSODashboard
                        {
                            ProjectID = sdr["ProjectID"].ToString(),
                            ProjectName = sdr["ProjectName"].ToString(),
                            UserID = sdr["UserID"].ToString(),
                            CustomerName = sdr["CustomerName"].ToString(),
                            SiteAddress = sdr["SiteAddress"].ToString(),
                            Wstatus = sdr["WorkStatusDesc"].ToString(),
                            WstatusDate = Convert.ToDateTime(sdr["WstatusDate"]),
                            Workstatus = Convert.ToInt32(sdr["Wstatus"]),

                            ProjectExpiryDate = ((dates != "NA") ? (dates) : "NA"),

                            VizOption = sdr["VizOptionDesc"].ToString(),
                            Bstatus = sdr["BStatusDesc"].ToString(),
                            curdate = (sdr["curdate"]).ToString(),
                            CPBody1 = (sdr["CPBody1"]).ToString(),
                            CPBody2 = (sdr["CPBody2"]).ToString(),
                            CPBody3 = (sdr["CPBody3"]).ToString(),
                            CPBorder1= (sdr["CPBorder1"]).ToString(),
                            CPBorder2 = (sdr["CPBorder2"]).ToString(),
                            CPBorder3 = (sdr["CPBorder3"]).ToString(),
                            CPHighlight1 = (sdr["CPHighlight1"]).ToString(),
                            CPHighlight2 = (sdr["CPHighlight2"]).ToString(),
                            CPHighlight3 = (sdr["CPHighlight3"]).ToString(),
                            CPSplRequest= (sdr["CPSplRequest"]).ToString(),
                            InSyComments= (sdr["InSyComments"]).ToString()


                        });
                    }
                }
                var list = ProjectList.ToList();

                if (!String.IsNullOrEmpty(q))
                {
                    q = q.ToLower();
                    list = list.Where(s => s.ProjectID.ToLower().Contains(q) || s.ProjectName.ToLower().Contains(q) || s.UserID.ToLower().Contains(q) || s.CustomerName.ToLower().Contains(q) || s.SiteAddress.ToLower().Contains(q) || s.Wstatus.ToLower().Contains(q) || s.VizOption.ToLower().Contains(q) || s.Bstatus.ToLower().Contains(q)).ToList();
                }


                switch (sortOrder)
                {
                    case "ProjectID_desc":
                        list = list.OrderByDescending(x => x.ProjectID).ToList();
                        break;
                    case "Wstatus_desc":
                        list = list.OrderByDescending(x => x.Wstatus).ToList();
                        break;
                    case "ProjectID":
                        list = list.OrderBy(x => x.ProjectID).ToList();
                        break;
                    case "Wstatus":
                        list = list.OrderBy(x => x.Wstatus).ToList();
                        break;
                    default:
                        break;
                }
                con.Close();
                return View(list.ToPagedList(page, pageSize));
            }
        }
        public ActionResult InsertLog(int Id)
        {
            string userid = Convert.ToString(Session["UserID"]);
           
            string constr = ConfigurationManager.ConnectionStrings["Nerolacconstr"].ConnectionString;
            MySqlConnection conn = new MySqlConnection(constr);
            conn.Open();
            MySqlDataAdapter da = new MySqlDataAdapter("select caseid from dpcaseidmaster where OpportunityNo='" + Id+"'",conn);
            DataSet dss = new DataSet();
            da.Fill(dss);
            string caseid = "";
            if (dss.Tables[0].Rows.Count > 0)
            {
                caseid = Convert.ToString(dss.Tables[0].Rows[0][0]);
            }
            conn.Close();
            using (MySqlConnection con = new MySqlConnection(constr))
            {
                DataSet ds = new DataSet();
                MySqlCommand com = new MySqlCommand("Sp_InsertWstatusLog", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@ProjectID", Id);
                com.Parameters.AddWithValue("@TSOID", userid);
                com.Parameters.AddWithValue("@CaseID", caseid);

                con.Open();
                com.ExecuteNonQuery();
                MySqlDataAdapter ad = new MySqlDataAdapter(com);
                ad.Fill(ds);
                // String UrlToRedirect = String.Format(String.Concat("https://colourmyspace.co.in", "/MyVizKN/CROSOLanding.aspx?caseId={0}&previewCenter={1}&source=0"), "P521F12S3833", "PVC05");
                String PreviewCentreCode = String.Empty;
                String CaseIDfromKN = String.Empty;
                //String UrlToRedirect = "https://colourmyspace.co.in/MyViz/Login/Index";
                String UrlToRedirect = "https://bincrm.com/MyViz/Login/Index";
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    PreviewCentreCode = !String.IsNullOrEmpty(ds.Tables[0].Rows[0]["PreviewCentreCode"].ToString()) ? (Convert.ToString(ds.Tables[0].Rows[0]["PreviewCentreCode"])) : "NA";
                    CaseIDfromKN = !String.IsNullOrEmpty(ds.Tables[0].Rows[0]["CaseIDfromKN"].ToString()) ? (Convert.ToString(ds.Tables[0].Rows[0]["CaseIDfromKN"])) : "NA";
                     UrlToRedirect = String.Format(String.Concat("https://colourmyspace.co.in", "/MyVizKN/CROSOLanding.aspx?caseId={0}&previewCenter={1}&source=0"), CaseIDfromKN, PreviewCentreCode);
                    //UrlToRedirect = String.Format(String.Concat("https://coloursgalore.com", "/CW/CROSOLanding.aspx?caseId={0}&previewCenter={1}&source=0"), CaseIDfromKN, PreviewCentreCode);
                }
                //else
                //{
                //    UrlToRedirect = String.Format(String.Concat("https://colourmyspace.co.in", "/MyVizKN/CROSOLanding.aspx?caseId={0}&previewCenter={1}&source=0"), "P521F12S3833", "PVC05");

                //}
                return Redirect(UrlToRedirect);
            }
        }
        // GET: TSODashboard/Create
        public ActionResult Create()
        {
            return View();
        }

        public ActionResult myModal(int Id)
        {
            ViewBag.type = 1;
            string constr = ConfigurationManager.ConnectionStrings["Nerolacconstr"].ConnectionString;
            using (MySqlConnection con = new MySqlConnection(constr))
            {
                DataSet ds = new DataSet();
                MySqlCommand com = new MySqlCommand("SP_WorkstatusLog", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@ProjectID", Id);
                con.Open();
                //com.ExecuteNonQuery();
                MySqlDataAdapter ad = new MySqlDataAdapter(com);
                ad.Fill(ds);
                var ProjectList = ds.Tables[0].AsEnumerable().Select(dataRow => new WStatusLog
                {
                    ProjectName = dataRow.Field<string>("ProjectName"),
                    CustomerName = dataRow.Field<string>("CustomerName"),
                    WorkStatus = dataRow.Field<string>("WorkStatusDesc"),
                    M_Date = dataRow.Field<DateTime>("M_Date"),
                    AssignedTo = dataRow.Field<string>("AssignedTo")
                });
                var list = ProjectList.ToList();
                con.Close();
                return View(list.ToPagedList(1, 5));
            }


        }

        public ActionResult PreviewAlbum(int Id)
        {
            ViewBag.ProjectID = Id;
            string constr = ConfigurationManager.ConnectionStrings["Nerolacconstr"].ConnectionString;
            MySqlConnection con = new MySqlConnection(constr);
            DataSet ds = new DataSet();
            string query = "select ft.ProjectName,vz.vizoptionDesc from fromtso ft left join lkupvizoption vz on ft.vizoption=vz.vizoption where ft.ProjectID='" + Id + "'";
            con.Open();
            MySqlCommand cmd = new MySqlCommand(query, con);
            cmd.CommandType = CommandType.Text;
            MySqlDataAdapter sda = new MySqlDataAdapter(cmd);
            sda.Fill(ds);
            ViewBag.ProjectName = Convert.ToString(ds.Tables[0].Rows[0]["ProjectName"]);
            ViewBag.Vizoption = Convert.ToString(ds.Tables[0].Rows[0]["vizoptionDesc"]);
            con.Close();
            return View();
        }

        public ActionResult DownloadOption1(int Id)
        {
            string constr = ConfigurationManager.ConnectionStrings["Nerolacconstr"].ConnectionString;
            using (MySqlConnection con = new MySqlConnection(constr))
            {
                DataSet ds = new DataSet();
                MySqlCommand com = new MySqlCommand("Sp_GetCaseID", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@ProjectId", Id);
                con.Open();
                MySqlDataAdapter ad = new MySqlDataAdapter(com);
                ad.Fill(ds);
                String CaseIDfromKN = String.Empty;
                String UrlToRedirect = "https://colourmyspace.co.in/MyViz/PDF/";
                //String UrlToRedirect = "https://coloursgalore.com/CW/PDF/";
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    CaseIDfromKN = !String.IsNullOrEmpty(ds.Tables[0].Rows[0]["CaseIDfromKN"].ToString()) ? (Convert.ToString(ds.Tables[0].Rows[0]["CaseIDfromKN"])) : "NA";
                    UrlToRedirect = String.Format(String.Concat("https://colourmyspace.co.in/MyViz/PDF/", "{0}/{1}_{2}.pdf"), Id, 1, CaseIDfromKN);
                    //UrlToRedirect = String.Format(String.Concat("https://coloursgalore.com/CW/PDF/", "{0}/{1}_{2}.pdf"), Id, 1, CaseIDfromKN);
                }
                return Redirect(UrlToRedirect);
            }
        }

        public ActionResult DownloadOption2(int Id)
        {
            string constr = ConfigurationManager.ConnectionStrings["Nerolacconstr"].ConnectionString;
            using (MySqlConnection con = new MySqlConnection(constr))
            {
                DataSet ds = new DataSet();
                MySqlCommand com = new MySqlCommand("Sp_GetCaseID", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@ProjectId", Id);
                con.Open();
                MySqlDataAdapter ad = new MySqlDataAdapter(com);
                ad.Fill(ds);
                String CaseIDfromKN = String.Empty;
                String UrlToRedirect = "https://colourmyspace.co.in/MyViz/PDF/";
                //String UrlToRedirect = "https://coloursgalore.com/CW/PDF/";
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    CaseIDfromKN = !String.IsNullOrEmpty(ds.Tables[0].Rows[0]["CaseIDfromKN"].ToString()) ? (Convert.ToString(ds.Tables[0].Rows[0]["CaseIDfromKN"])) : "NA";
                    UrlToRedirect = String.Format(String.Concat("https://colourmyspace.co.in/MyViz/PDF/", "{0}/{1}_{2}.pdf"), Id, 2, CaseIDfromKN);
                    //UrlToRedirect = String.Format(String.Concat("https://coloursgalore.com/CW/PDF/", "{0}/{1}_{2}.pdf"), Id, 2, CaseIDfromKN);
                }
                return Redirect(UrlToRedirect);
            }
        }

        public ActionResult DownloadOption3(int Id)
        {
            string constr = ConfigurationManager.ConnectionStrings["Nerolacconstr"].ConnectionString;
            using (MySqlConnection con = new MySqlConnection(constr))
            {
                DataSet ds = new DataSet();
                MySqlCommand com = new MySqlCommand("Sp_GetCaseID", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@ProjectId", Id);
                con.Open();
                MySqlDataAdapter ad = new MySqlDataAdapter(com);
                ad.Fill(ds);
                String CaseIDfromKN = String.Empty;
                String UrlToRedirect = "https://colourmyspace.co.in/MyViz/PDF/";
                //String UrlToRedirect = "https://coloursgalore.com/CW/PDF/";
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    CaseIDfromKN = !String.IsNullOrEmpty(ds.Tables[0].Rows[0]["CaseIDfromKN"].ToString()) ? (Convert.ToString(ds.Tables[0].Rows[0]["CaseIDfromKN"])) : "NA";
                    UrlToRedirect = String.Format(String.Concat("https://colourmyspace.co.in/MyViz/PDF/", "{0}/{1}_{2}.pdf"), Id, 3, CaseIDfromKN);
                    //UrlToRedirect = String.Format(String.Concat("https://coloursgalore.com/CW/PDF/", "{0}/{1}_{2}.pdf"), Id, 3, CaseIDfromKN);
                }
                return Redirect(UrlToRedirect);
            }
        }


        public ActionResult LaunchPreview(int Id)
        {
            string userid = Convert.ToString(Session["UserID"]);
            string constr = ConfigurationManager.ConnectionStrings["Nerolacconstr"].ConnectionString;
            using (MySqlConnection con = new MySqlConnection(constr))
            {
                DataSet ds = new DataSet();
                MySqlCommand com = new MySqlCommand("Sp_InsertWstatusLog", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@ProjectID", Id);
                com.Parameters.AddWithValue("@TSOID", userid);
                con.Open();
                //com.ExecuteNonQuery();
                MySqlDataAdapter ad = new MySqlDataAdapter(com);
                ad.Fill(ds);
                // String UrlToRedirect = String.Format(String.Concat("https://colourmyspace.co.in", "/MyVizKN/CROSOLanding.aspx?caseId={0}&previewCenter={1}&source=0"), "P521F12S3833", "PVC05");
                String PreviewCentreCode = String.Empty;
                String CaseIDfromKN = String.Empty;
                String UrlToRedirect = "https://colourmyspace.co.in/MyViz/Login/Index";
                // String UrlToRedirect = "https://coloursgalore.com/CW/Login/Index";
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    PreviewCentreCode = !String.IsNullOrEmpty(ds.Tables[0].Rows[0]["PreviewCentreCode"].ToString()) ? (Convert.ToString(ds.Tables[0].Rows[0]["PreviewCentreCode"])) : "NA";
                    CaseIDfromKN = !String.IsNullOrEmpty(ds.Tables[0].Rows[0]["CaseIDfromKN"].ToString()) ? (Convert.ToString(ds.Tables[0].Rows[0]["CaseIDfromKN"])) : "NA";
                    UrlToRedirect = String.Format(String.Concat("https://colourmyspace.co.in", "/MyVizKN/CROSOLanding.aspx?caseId={0}&previewCenter={1}&source=0"), CaseIDfromKN, PreviewCentreCode);
                    //UrlToRedirect = String.Format(String.Concat("https://coloursgalore.com", "/CW/CROSOLanding.aspx?caseId={0}&previewCenter={1}&source=0"), CaseIDfromKN, PreviewCentreCode);
                }
                //else
                //{
                //UrlToRedirect = String.Format(String.Concat("https://colourmyspace.co.in", "/MyVizKN/CROSOLanding.aspx?caseId={0}&previewCenter={1}&source=0"), "P521F12S3833", "PVC05");

                //}
                return Redirect(UrlToRedirect);
            }
        }
        // POST: TSODashboard/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: TSODashboard/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: TSODashboard/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: TSODashboard/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: TSODashboard/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
