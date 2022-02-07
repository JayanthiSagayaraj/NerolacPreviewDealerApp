using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using context = System.Web.HttpContext;

namespace NerolacPreviewApp.Models
{
    public class ListTSO
    {
        public static List<SelectListItem> PopulateVizOptions()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            string constr = ConfigurationManager.ConnectionStrings["Nerolacconstr"].ConnectionString;
            using (MySqlConnection con = new MySqlConnection(constr))
            {
                string query = " SELECT Vizoption,VizOptionDesc FROM LkUpVizOption";
                using (MySqlCommand cmd = new MySqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    using (MySqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            items.Add(new SelectListItem
                            {
                                Text = sdr["VizOptionDesc"].ToString(),
                                Value = sdr["Vizoption"].ToString()
                            });
                        }
                    }
                    con.Close();
                }
            }

            return items;
        }

        public static List<SelectListItem> PopulateIoreorms(int prodid)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            string constr = ConfigurationManager.ConnectionStrings["Nerolacconstr"].ConnectionString;
            using (MySqlConnection con = new MySqlConnection(constr))
            {
                string query = "Select  ioreorms.ImageType,ioreorms.ImageTypeDesc  from fromtso as tso join lkupioreorms as ioreorms on tso.IorEorMS=ioreorms.ImageType where tso.ProjectId=" + prodid;
                using (MySqlCommand cmd = new MySqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    using (MySqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            items.Add(new SelectListItem
                            {
                                Text = sdr["ImageTypeDesc"].ToString(),
                                Value = sdr["ImageType"].ToString()
                            });
                        }
                    }
                    con.Close();
                }
            }

            return items;
        }
        public static List<SelectListItem> PopulateIoreorms()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            string constr = ConfigurationManager.ConnectionStrings["Nerolacconstr"].ConnectionString;
            using (MySqlConnection con = new MySqlConnection(constr))
            {
                string query = "Select   ImageType, ImageTypeDesc  from lkupioreorms";
                using (MySqlCommand cmd = new MySqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    using (MySqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            items.Add(new SelectListItem
                            {
                                Text = sdr["ImageTypeDesc"].ToString(),
                                Value = sdr["ImageType"].ToString()
                            });
                        }
                    }
                    con.Close();
                }
            }

            return items;
        }
        public static TSOProject GetProjectByID(int proId)
        {
            List<TSOProject> items = new List<TSOProject>();
            TSOProject tSO = new TSOProject();
            string constr = ConfigurationManager.ConnectionStrings["Nerolacconstr"].ConnectionString;
            MySqlConnection con = new MySqlConnection(constr);

            DataSet ds = new DataSet();
            string query = "Select *  from fromtso as tso join lkupioreorms as ioreorms on tso.IorEorMS=ioreorms.ImageType  where  tso.ProjectId=" + proId;

            con.Open();
            MySqlCommand cmd = new MySqlCommand(query, con);
            cmd.CommandType = CommandType.Text;
            MySqlDataAdapter sda = new MySqlDataAdapter(cmd);
            sda.Fill(ds);
            tSO.UserID = ds.Tables[0].Rows[0]["UserID"].ToString();
            tSO.ProjectID = Convert.ToInt32(ds.Tables[0].Rows[0]["ProjectId"]);
            tSO.CustomerName = ds.Tables[0].Rows[0]["CustomerName"].ToString();
            tSO.ProjectName = ds.Tables[0].Rows[0]["ProjectName"].ToString();
            tSO.ProjectValue = (ds.Tables[0].Rows[0]["ProjectValue"]).ToString();
            tSO.CustomerContactNo = ds.Tables[0].Rows[0]["CustomerContactNo"].ToString();
            tSO.CustomerEMailId = ds.Tables[0].Rows[0]["CustomerEMailId"].ToString();
            tSO.SiteAddress = ds.Tables[0].Rows[0]["SiteAddress"].ToString();
            tSO.Location = ds.Tables[0].Rows[0]["Location"].ToString();
            tSO.VizOption = ds.Tables[0].Rows[0]["VizOption"].ToString();
            tSO.Priority = ds.Tables[0].Rows[0]["Priority"].ToString();
            tSO.DealerName = ds.Tables[0].Rows[0]["DealerName"].ToString();
            tSO.DealerSAPCode = ds.Tables[0].Rows[0]["DealerSAPCode"].ToString();
            tSO.IorEorMS = ds.Tables[0].Rows[0]["ImageType"].ToString();
            tSO.FreshPainting = ds.Tables[0].Rows[0]["FreshPainting"].ToString();
            tSO.ImageFileName1 = ds.Tables[0].Rows[0]["ImageFileName1"].ToString();
            tSO.ImageFileName2 = ds.Tables[0].Rows[0]["ImageFileName2"].ToString();
            tSO.ImageFileName3 = ds.Tables[0].Rows[0]["ImageFileName3"].ToString();

            tSO.CPBody1 = ds.Tables[0].Rows[0]["CPBody1"].ToString();
            tSO.CPBody2 = ds.Tables[0].Rows[0]["CPBody2"].ToString();
            tSO.CPBody3 = ds.Tables[0].Rows[0]["CPBody3"].ToString();
            tSO.CPBorder1 = ds.Tables[0].Rows[0]["CPBorder1"].ToString();
            tSO.CPBorder2 = ds.Tables[0].Rows[0]["CPBorder2"].ToString();
            tSO.CPBorder3 = ds.Tables[0].Rows[0]["CPBorder3"].ToString();
            tSO.CPHighlight1 = ds.Tables[0].Rows[0]["CPHighlight1"].ToString();
            tSO.CPHighlight2 = ds.Tables[0].Rows[0]["CPHighlight2"].ToString();
            tSO.CPHighlight3 = ds.Tables[0].Rows[0]["CPHighlight3"].ToString();
            tSO.CPSplRequest = ds.Tables[0].Rows[0]["CPSplRequest"].ToString();
            tSO.Wstatus = ds.Tables[0].Rows[0]["WStatus"].ToString();
            string date = ds.Tables[0].Rows[0]["WStatusDate"].ToString();
            tSO.WStatusDate = Convert.ToDateTime(date);
            con.Close();
            return tSO;



        }


        public static int GetId()
        {
            List<TSOProject> items = new List<TSOProject>();
            TSOProject tSO = new TSOProject();
            string constr = ConfigurationManager.ConnectionStrings["Nerolacconstr"].ConnectionString;
            MySqlConnection con = new MySqlConnection(constr);

            DataSet ds = new DataSet();
            string query = "Select ProjectId from fromtso order by ProjectId desc limit 1";

            con.Open();
            MySqlCommand cmd = new MySqlCommand(query, con);
            cmd.CommandType = CommandType.Text;
            MySqlDataAdapter sda = new MySqlDataAdapter(cmd);
            sda.Fill(ds);

            tSO.ProjectID = Convert.ToInt32(ds.Tables[0].Rows[0]["ProjectId"]);

            con.Close();
            return tSO.ProjectID;

        }

        public static string GetType(int proid)
        {
            List<TSOProject> items = new List<TSOProject>();
            TSOProject tSO = new TSOProject();
            string constr = ConfigurationManager.ConnectionStrings["Nerolacconstr"].ConnectionString;
            MySqlConnection con = new MySqlConnection(constr);

            DataSet ds = new DataSet();
            string query = "Select ioreorms.ImageType,ioreorms.ImageTypeDesc  from fromtso as tso join lkupioreorms as ioreorms on tso.IorEorMS=ioreorms.ImageType where tso.projectid=" + proid;

            con.Open();
            MySqlCommand cmd = new MySqlCommand(query, con);
            cmd.CommandType = CommandType.Text;
            MySqlDataAdapter sda = new MySqlDataAdapter(cmd);
            sda.Fill(ds);

            tSO.IorEorMS = ds.Tables[0].Rows[0]["ImageType"].ToString();

            con.Close();
            return tSO.IorEorMS;

        }

        public static TSOProject GetDepotDetails(String UserDepot)
        {
            List<TSOProject> items = new List<TSOProject>();
            TSOProject tSO = new TSOProject();
            string constr = ConfigurationManager.ConnectionStrings["Nerolacconstr"].ConnectionString;
            MySqlConnection con = new MySqlConnection(constr);
            DataSet ds = new DataSet();
            string query = "select * from lkupdepot where depot='" + UserDepot + "'";


            con.Open();
            MySqlCommand cmd = new MySqlCommand(query, con);
            cmd.CommandType = CommandType.Text;
            MySqlDataAdapter sda = new MySqlDataAdapter(cmd);
            sda.Fill(ds);

            tSO.Depot = ds.Tables[0].Rows[0]["Depot"].ToString();
            tSO.DepotContactName = ds.Tables[0].Rows[0]["DepotContactName"].ToString();
            tSO.DepotName = ds.Tables[0].Rows[0]["DepotName"].ToString();
            tSO.DepotAd1 = ds.Tables[0].Rows[0]["DepotAd1"].ToString();
            tSO.DepotAd2 = ds.Tables[0].Rows[0]["DepotAd2"].ToString();
            tSO.DepotAd3 = ds.Tables[0].Rows[0]["DepotAd3"].ToString();
            tSO.DepotAd4 = ds.Tables[0].Rows[0]["DepotAd4"].ToString();
            tSO.DepotPinCode = ds.Tables[0].Rows[0]["DepotPinCode"].ToString();
            tSO.DepotEmailId = ds.Tables[0].Rows[0]["DepotEmailId"].ToString();
            tSO.DepotContactNo = ds.Tables[0].Rows[0]["DepotContactNo"].ToString();
            tSO.DepotRemarks = ds.Tables[0].Rows[0]["Remarks"].ToString();


            con.Close();
            return tSO;

        }
        public List<PreviousColourOptions> GetPreviousColourOptions()
        {
            List<PreviousColourOptions> lstColourPalette = new List<PreviousColourOptions>();
            var PreviousColourOptions = new List<PreviousColourOptions>();
            string constr = ConfigurationManager.ConnectionStrings["Nerolacconstr"].ConnectionString;

            using (MySqlConnection con = new MySqlConnection(constr))
            {
                DataSet ds = new DataSet();
                MySqlCommand com = new MySqlCommand("select mc.RSN,mc.ProjectID,mc.OptionNo,mc.ShadeCode1,mc.Shadecode2,mc.shadecode3,(select shadename from ancptable where shadecode=mc.shadecode1) as shadename1," +
                    "(select shadename from ancptable where shadecode=mc.shadecode2) as shadename2," +
                    "(select shadename from ancptable where shadecode=mc.shadecode3) as shadename3," +
                    "(select concat('#',red_h,green_h,blue_h) from ancptable where shadecode=mc.shadecode1) as hcode1," +
                    "(select concat('#',red_h,green_h,blue_h) from ancptable where shadecode=mc.shadecode2) as hcode2," +
                    "(select concat('#',red_h,green_h,blue_h) from ancptable where shadecode=mc.shadecode3) as hcode3 from myvizcolours mc ", con);

                com.CommandTimeout = 1000;

                con.Open();



                using (MySqlDataReader sdr = com.ExecuteReader())
                {
                    while (sdr.Read())
                    {

                        PreviousColourOptions.Add(new PreviousColourOptions
                        {
                            RSN = String.IsNullOrEmpty(sdr["RSN"].ToString()) ? "-" : sdr["RSN"].ToString(),
                            Projectid = String.IsNullOrEmpty(sdr["ProjectID"].ToString()) ? "-" : sdr["ProjectID"].ToString(),
                            Optionno = String.IsNullOrEmpty(sdr["optionno"].ToString()) ? "-" : sdr["optionno"].ToString(),
                            code1 = String.IsNullOrEmpty(sdr["Shadecode1"].ToString()) ? "-" : sdr["Shadecode1"].ToString(),
                            Name1 = String.IsNullOrEmpty(sdr["shadename1"].ToString()) ? "-" : sdr["shadename1"].ToString(),
                            hcode1 = String.IsNullOrEmpty(sdr["hcode1"].ToString()) ? "-" : sdr["hcode1"].ToString(),
                            code2 = String.IsNullOrEmpty(sdr["Shadecode2"].ToString()) ? "-" : sdr["Shadecode2"].ToString(),
                            Name2 = String.IsNullOrEmpty(sdr["shadename2"].ToString()) ? "-" : sdr["shadename2"].ToString(),
                            hcode2 = String.IsNullOrEmpty(sdr["hcode2"].ToString()) ? "-" : sdr["hcode2"].ToString(),
                            code3 = String.IsNullOrEmpty(sdr["Shadecode3"].ToString()) ? "-" : sdr["Shadecode3"].ToString(),
                            Name3 = String.IsNullOrEmpty(sdr["shadename3"].ToString()) ? "-" : sdr["shadename3"].ToString(),
                            hcode3 = String.IsNullOrEmpty(sdr["hcode3"].ToString()) ? "-" : sdr["hcode3"].ToString()


                        });
                    }
                }
                var list = PreviousColourOptions.ToList();


                con.Close();
                return list;
            }
        }


        public static BizStatus GetBizStatusByID(int proId)
        {
            BizStatus biz = new BizStatus();
            string constr = ConfigurationManager.ConnectionStrings["Nerolacconstr"].ConnectionString;
            MySqlConnection con = new MySqlConnection(constr);
            DataSet ds = new DataSet();
            con.Open();
            MySqlCommand cmd = new MySqlCommand("Sp_BizStatus", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ProjectId", proId);
            MySqlDataAdapter sda = new MySqlDataAdapter(cmd);
            sda.Fill(ds);
            biz.ProjectId = Convert.ToInt32(ds.Tables[0].Rows[0]["ProjectID"]);
            biz.CustomerName = ds.Tables[0].Rows[0]["CustomerName"].ToString();
            //DateTime dates = Convert.ToDateTime(ds.Tables[0].Rows[0]["BstatusDate"]);
            //string date = dates.ToString("dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
            biz.StatusDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["BstatusDate"]);
            biz.BStatus = ds.Tables[0].Rows[0]["BStatusDesc"].ToString();
            biz.StatusReason = ds.Tables[0].Rows[0]["BStatusReason"].ToString();
            con.Close();
            return biz;
        }

        public static List<BizStatus> GetBizList()
        {
            List<BizStatus> items = new List<BizStatus>();

            string constr = ConfigurationManager.ConnectionStrings["Nerolacconstr"].ConnectionString;

            using (MySqlConnection con = new MySqlConnection(constr))
            {
                string query = "Select FT.ProjectID, FT.CustomerName, DPM.BstatusDate, lkupb.BStatusDesc,DPM.BStatusReason from FromTSO FT Join dpcaseidmaster DPM on FT.ProjectID = DPM.OpportunityNo join lkupworkstatus lkupws on FT.Wstatus = lkupws.WorkStatus join lkupvizoption lkupviz on FT.VizOption = lkupviz.Vizoption join lkupbstatus lkupb on DPM.Bstatus = lkupb.Bstatus";
                using (MySqlCommand cmd = new MySqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    using (MySqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            //DateTime dates = Convert.ToDateTime(sdr["BstatusDate"]);
                            //string date = dates.ToString("dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);

                            items.Add(new BizStatus
                            {
                                ProjectId = Convert.ToInt32(sdr["ProjectID"]),
                                CustomerName = sdr["CustomerName"].ToString(),
                                BStatus = sdr["BStatusDesc"].ToString(),
                                StatusDate = Convert.ToDateTime(sdr["BstatusDate"]),
                                StatusReason = sdr["BStatusReason"].ToString()
                            });
                        }
                    }
                    con.Close();
                }
            }

            return items;
        }

        public static List<projectsupdate> projectsupdate()
        {
            List<projectsupdate> items = new List<projectsupdate>();

            string constr = ConfigurationManager.ConnectionStrings["Nerolacconstr"].ConnectionString;

            using (MySqlConnection con = new MySqlConnection(constr))
            {
                string query = "Select ProjectID from fromtso";
                using (MySqlCommand cmd = new MySqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    using (MySqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {


                            items.Add(new projectsupdate
                            {
                                ProjectID = (sdr["ProjectID"]).ToString()

                            });
                        }
                    }
                    con.Close();
                }
            }

            return items;
        }

        public static List<SelectListItem> PopulateBizStatus(int proid)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            string constr = ConfigurationManager.ConnectionStrings["Nerolacconstr"].ConnectionString;
            using (MySqlConnection con = new MySqlConnection(constr))
            {
                string query = "Select  lkupb.Bstatus, lkupb.BStatusDesc from FromTSO FT join lkupworkstatus lkupws on FT.Wstatus = lkupws.WorkStatus join lkupvizoption lkupviz on FT.VizOption = lkupviz.Vizoption join lkupbstatus lkupb on FT.Bstatus = lkupb.Bstatus where FT.ProjectId=" + proid;
                using (MySqlCommand cmd = new MySqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    using (MySqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            items.Add(new SelectListItem
                            {
                                Text = sdr["BStatusDesc"].ToString(),
                                Value = sdr["Bstatus"].ToString()
                            });
                        }
                    }
                    con.Close();
                }
            }

            return items;
        }

        public static List<SelectListItem> PopulateFeedBackCategory()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            string constr = ConfigurationManager.ConnectionStrings["Nerolacconstr"].ConnectionString;
            using (MySqlConnection con = new MySqlConnection(constr))
            {
                string query = "Select FeedbackCategory,FBCatTitle from lkupfbcat order by FBCatTitle ";
                using (MySqlCommand cmd = new MySqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    using (MySqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            items.Add(new SelectListItem
                            {
                                Text = sdr["FBCatTitle"].ToString(),
                                Value = sdr["FeedbackCategory"].ToString()
                            });
                        }
                    }
                    con.Close();
                }
            }

            return items;
        }


        public static string GetBiz(int proid)
        {
            List<TSOProject> items = new List<TSOProject>();
            TSOProject tSO = new TSOProject();
            string constr = ConfigurationManager.ConnectionStrings["Nerolacconstr"].ConnectionString;
            MySqlConnection con = new MySqlConnection(constr);

            DataSet ds = new DataSet();
            string query = "Select FT.Bstatus from FromTSO FT join lkupworkstatus lkupws on FT.Wstatus = lkupws.WorkStatus join lkupvizoption lkupviz on FT.VizOption = lkupviz.Vizoption join lkupbstatus lkupb on FT.Bstatus = lkupb.Bstatus where FT.ProjectId=" + proid;

            con.Open();
            MySqlCommand cmd = new MySqlCommand(query, con);
            cmd.CommandType = CommandType.Text;
            MySqlDataAdapter sda = new MySqlDataAdapter(cmd);
            sda.Fill(ds);

            tSO.IorEorMS = ds.Tables[0].Rows[0]["Bstatus"].ToString();

            con.Close();
            return tSO.IorEorMS;

        }
        public static List<Stats> GetStatsList()
        {
            List<Stats> items = new List<Stats>();

            string constr = ConfigurationManager.ConnectionStrings["Nerolacconstr"].ConnectionString;

            using (MySqlConnection con = new MySqlConnection(constr))
            {
                string query = "Sp_StatsCount";
                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@iMode", 1);
                    con.Open();
                    using (MySqlDataAdapter sdr = new MySqlDataAdapter(cmd))
                    {
                        DataSet ds = new DataSet();
                        MySqlDataAdapter sda = new MySqlDataAdapter(cmd);
                        sdr.Fill(ds);

                        items.Add(new Stats
                        {
                            ProjectsToday = Convert.ToInt32(ds.Tables[0].Rows[0]["ProjectsToday"]),
                            TSOInvolvedToday = Convert.ToInt32(ds.Tables[1].Rows[0]["TSOInvolvedToday"]),
                            SCACountToday = Convert.ToInt32(ds.Tables[2].Rows[0]["SCACountToday"]),
                            HCACountToday = Convert.ToInt32(ds.Tables[3].Rows[0]["HCACountToday"]),
                            ProjectsYesterday = Convert.ToInt32(ds.Tables[4].Rows[0]["ProjectsYesterday"]),
                            TSOInvolvedYesterday = Convert.ToInt32(ds.Tables[5].Rows[0]["TSOInvolvedYesterday"]),
                            SCACountYesterday = Convert.ToInt32(ds.Tables[6].Rows[0]["SCACountYesterday"]),
                            HCACountYesterday = Convert.ToInt32(ds.Tables[7].Rows[0]["HCACountYesterday"]),
                            Projects = Convert.ToInt32(ds.Tables[8].Rows[0]["Projects"]),
                            TSOInvolved = Convert.ToInt32(ds.Tables[9].Rows[0]["TSOInvolved"]),
                            SCACount = Convert.ToInt32(ds.Tables[10].Rows[0]["SCACount"]),
                            HCACount = Convert.ToInt32(ds.Tables[11].Rows[0]["HCACount"])
                        });

                    }
                    con.Close();
                }
            }

            return items;
        }

        public static User GetUserByID(string userid)
        {
            User user = new User();
            string constr = ConfigurationManager.ConnectionStrings["Nerolacconstr"].ConnectionString;
            MySqlConnection con = new MySqlConnection(constr);
            DataSet ds = new DataSet();
            con.Open();
            MySqlCommand cmd = new MySqlCommand("Sp_myProfile", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UserId", userid);
            MySqlDataAdapter sda = new MySqlDataAdapter(cmd);
            sda.Fill(ds);
            user.UserId = ds.Tables[0].Rows[0]["EmpID"].ToString();
            user.UserName = ds.Tables[0].Rows[0]["Name"].ToString();
            user.Department = ds.Tables[0].Rows[0]["Department"].ToString();
            user.Designation = ds.Tables[0].Rows[0]["Designation"].ToString();
            user.Location = ds.Tables[0].Rows[0]["Location"].ToString();
            user.State = ds.Tables[0].Rows[0]["State"].ToString();
            user.City = ds.Tables[0].Rows[0]["City"].ToString();
            user.MailID = ds.Tables[0].Rows[0]["MailID"].ToString();
            user.UserContactNo = ds.Tables[0].Rows[0]["UserContactNo"].ToString();
            user.Depot = ds.Tables[0].Rows[0]["Depot"].ToString();
            user.DepotName = ds.Tables[0].Rows[0]["DepotName"].ToString();
            user.DepotAd1 = ds.Tables[0].Rows[0]["DepotAd1"].ToString();
            user.DepotAd2 = ds.Tables[0].Rows[0]["DepotAd2"].ToString();
            user.DepotAd3 = ds.Tables[0].Rows[0]["DepotAd3"].ToString();
            user.DepotAd4 = ds.Tables[0].Rows[0]["DepotAd4"].ToString();
            user.DepotPinCode = ds.Tables[0].Rows[0]["DepotPinCode"].ToString();
            con.Close();
            return user;
        }
        //public static FlashMessages GetFlashMsgList(string userid)
        //{

        //    FlashMessages FlashMessages = new FlashMessages();
        //    string constr = ConfigurationManager.ConnectionStrings["Nerolacconstr"].ConnectionString;
        //    MySqlConnection con = new MySqlConnection(constr);
        //    DataSet ds = new DataSet();
        //    con.Open();
        //    string Region = "";
        //    MySqlCommand cmd = new MySqlCommand("Sp_FlashMessageList", con);
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.Parameters.AddWithValue("@UserId", userid);
        //    cmd.Parameters.AddWithValue("@RegionName", Region);
        //    MySqlDataAdapter sda = new MySqlDataAdapter(cmd);
        //    sda.Fill(ds);
        //    if (ds.Tables[0].Rows.Count > 0)
        //    {
        //        FlashMessages.Title1 = ds.Tables[0].Rows[0]["MessageTitle"].ToString();
        //        FlashMessages.Text1 = ds.Tables[0].Rows[0]["MessageText"].ToString();
        //    }
        //    con.Close();
        //    return FlashMessages;


        //}

        public static List<Feedback> GetFeedbackList(String UserID,String UserLevel)
        {
            List<Feedback> items = new List<Feedback>();

            string constr = ConfigurationManager.ConnectionStrings["Nerolacconstr"].ConnectionString;

            using (MySqlConnection con = new MySqlConnection(constr))
            {
                string query = "";

                if (UserLevel.ToString() == "1")
                    query = "select  fb.UserID, fb.FeedbackRef,ft.projectname,fb.FeedbackDate,fb.FeedbackCategory,fb.TSOInput,fb.ReplyGiven,fb.RepliedBy,fb.RepliedDate from feedback as fb join fromtso as ft on fb.projectid=ft.projectid where fb.UserID= '" + UserID + "' order by fb.FeedbackDate desc";
                else
                    query = "select  fb.UserID, fb.FeedbackRef,ft.projectname,fb.FeedbackDate,fb.FeedbackCategory,fb.TSOInput,fb.ReplyGiven,fb.RepliedBy,fb.RepliedDate from feedback as fb join fromtso as ft on fb.projectid=ft.projectid order by fb.FeedbackDate desc";

                using (MySqlCommand cmd = new MySqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    using (MySqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            items.Add(new Feedback
                            {
                                Date = Convert.ToDateTime(sdr["FeedbackDate"]),
                                TSOId = sdr["UserID"].ToString(),
                                ProjectName = sdr["projectname"].ToString(),
                                TSOInput = sdr["TSOInput"].ToString(),
                                ReplyBy = sdr["RepliedBy"].ToString(),
                                ReplyGiven = sdr["ReplyGiven"].ToString(),
                                RDate = sdr["RepliedDate"] == DBNull.Value ? null : sdr["RepliedDate"].ToString()

                            }); ;
                    }
                }
                con.Close();
            }
        }

            return items;
        }

    public static List<Help> GetHelpList(string userid)
    {
        List<Help> items = new List<Help>();
            
           
          string constr = ConfigurationManager.ConnectionStrings["Nerolacconstr"].ConnectionString;

           
            using (MySqlConnection con = new MySqlConnection(constr))
            {
                con.Open();
                MySqlCommand com = new MySqlCommand("SP_InsertHelpLog", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@iUsername", userid);
                com.ExecuteNonQuery();
                string query = "select *,DATE_FORMAT(c_date,'%Y-%m-%d') as c_date ,DATE_FORMAT(curdate(),'%Y-%m-%d')  as cdate from cwhelp order by C_Date desc";
            using (MySqlCommand cmd = new MySqlCommand(query))
            {
                cmd.Connection = con;
               
                using (MySqlDataReader sdr = cmd.ExecuteReader())
                {
                    while (sdr.Read())
                    {

                        items.Add(new Help
                        {
                            ChapterNumber = Convert.ToDecimal(sdr["ChapterNumber"]),
                            MainTitle = sdr["MainTitle"].ToString(),
                            SubTitle = sdr["SubTitle"].ToString(),
                            Shortdescription = sdr["Shortdescription"].ToString(),
                            DetailsSet1 = sdr["DetailsSet1"].ToString(),
                            DetailsSet2 = sdr["DetailsSet2"].ToString(),
                            Attachment = sdr["Attachment"].ToString(),
                            Tags = sdr["Tags"].ToString(),
                            c_date= Convert.ToDateTime(sdr["c_date"]).ToString("yyyy-MM-dd"),
                            n= Convert.ToDateTime(sdr["cdate"]).ToString("yyyy-MM-dd"),
                            //s = Convert.ToDateTime(sdr["sdate"])

                        });
                    }
                }
                con.Close();
            }
        }

        return items;
    }
    public static List<SelectListItem> PopulateProjectName(String userid, String userlevel)
    {
        List<SelectListItem> items = new List<SelectListItem>();
        string constr = ConfigurationManager.ConnectionStrings["Nerolacconstr"].ConnectionString;
        using (MySqlConnection con = new MySqlConnection(constr))
        {
                string query = "";
                if (userlevel.ToString() == "1")
                    query = "select distinct ProjectID,projectname from fromtso where UserID = '" + userid + "' order by ProjectID desc";
                else
                    query = "select distinct ProjectID,projectname from fromtso order by ProjectID desc";

                using (MySqlCommand cmd = new MySqlCommand(query))
            {
                cmd.Connection = con;
                con.Open();
                using (MySqlDataReader sdr = cmd.ExecuteReader())
                {
                    while (sdr.Read())
                    {
                        items.Add(new SelectListItem
                        {
                            Text = String.Concat(sdr["ProjectID"].ToString(),"-" ,sdr["projectname"].ToString()),
                            Value = sdr["ProjectID"].ToString()
                        });
                    }
                }
                con.Close();
            }
        }

        return items;
    }
    public static string GetRemarks(string Vizoption)
    {
        List<TSOProject> items = new List<TSOProject>();
        TSOProject tSO = new TSOProject();
        string constr = ConfigurationManager.ConnectionStrings["Nerolacconstr"].ConnectionString;
        MySqlConnection con = new MySqlConnection(constr);

            DataSet ds = new DataSet();
            string query = "Select distinct Remarks from lkupvizoption where Vizoption='" + Vizoption + "'";

        con.Open();
        MySqlCommand cmd = new MySqlCommand(query, con);
        cmd.CommandType = CommandType.Text;
        MySqlDataAdapter sda = new MySqlDataAdapter(cmd);
        sda.Fill(ds);
        tSO.Remarks = ds.Tables[0].Rows[0]["Remarks"].ToString();
        con.Close();
        return tSO.Remarks;

        } 
        public static int GetPRI(string userid)
        {
            List<TSODashboard> items = new List<TSODashboard>();
            TSODashboard tSO = new TSODashboard();
            string constr = ConfigurationManager.ConnectionStrings["Nerolacconstr"].ConnectionString;
            MySqlConnection con = new MySqlConnection(constr);

            DataSet ds = new DataSet();
            string query = "select COUNT(Projectid) as PRICOUNT from fromtso where wstatus in ('60','70','80') and UserID='" + userid + "'";

            con.Open();
            MySqlCommand cmd = new MySqlCommand(query, con);
            cmd.CommandType = CommandType.Text;
            MySqlDataAdapter sda = new MySqlDataAdapter(cmd);
            sda.Fill(ds);

            tSO.PRI = Convert.ToInt32(ds.Tables[0].Rows[0]["PRICOUNT"]);

            con.Close();
            return tSO.PRI;

        }

        public static int GetSI(string userid)
        {
            List<TSODashboard> items = new List<TSODashboard>();
            TSODashboard tSO = new TSODashboard();
            string constr = ConfigurationManager.ConnectionStrings["Nerolacconstr"].ConnectionString;
            MySqlConnection con = new MySqlConnection(constr);

            DataSet ds = new DataSet();
            string query = "select COUNT(Projectid) as SICOUNT from fromtso where wstatus !=99 and UserID='" + userid + "'";
            

            con.Open();
            MySqlCommand cmd = new MySqlCommand(query, con);
            cmd.CommandType = CommandType.Text;
            MySqlDataAdapter sda = new MySqlDataAdapter(cmd);
            sda.Fill(ds);

            tSO.SI = Convert.ToInt32(ds.Tables[0].Rows[0]["SICOUNT"]);

            con.Close();
            return tSO.SI;

        }

        public static TSODashboard GetAllowNewProject(string userid)
        {
            List<TSODashboard> items = new List<TSODashboard>();
            TSODashboard tSO = new TSODashboard();
            string constr = ConfigurationManager.ConnectionStrings["Nerolacconstr"].ConnectionString;
            MySqlConnection con = new MySqlConnection(constr);

            DataSet ds = new DataSet();
            string query = "select AllowNewProject, DisallowMessage from settings";

            con.Open();
            MySqlCommand cmd = new MySqlCommand(query, con);
            cmd.CommandType = CommandType.Text;
            MySqlDataAdapter sda = new MySqlDataAdapter(cmd);
            sda.Fill(ds);

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0 )
            {
                tSO.AllowNewProject = Convert.ToString(ds.Tables[0].Rows[0]["AllowNewProject"]);
                tSO.DisallowMessage = Convert.ToString(ds.Tables[0].Rows[0]["DisallowMessage"]);
            }
            else
            {
                tSO.AllowNewProject = "NO";
            }
            con.Close();
            return tSO;

        }
        public static string GetNWH(DateTime date)
        {
            List<TSODashboard> items = new List<TSODashboard>();
            TSODashboard tSO = new TSODashboard();
            string constr = ConfigurationManager.ConnectionStrings["Nerolacconstr"].ConnectionString;
            MySqlConnection con = new MySqlConnection(constr);
            DateTime NextDate = DateTime.Now.AddDays(1);
            DataSet ds = new DataSet();
            string query = "Select DisplayMessage from nonworkinghours where DayDate='" + date.ToString("y-M-d") + "' or DayDate='" + NextDate.ToString("y-M-d") + "' limit 1;";

            con.Open();
            MySqlCommand cmd = new MySqlCommand(query, con);
            cmd.CommandType = CommandType.Text;
            MySqlDataAdapter sda = new MySqlDataAdapter(cmd);
            sda.Fill(ds);
            if(ds!=null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                tSO.NWH = ds.Tables[0].Rows[0]["DisplayMessage"].ToString();
            }
            else
            {
                tSO.NWH = "";
            }
           
            con.Close();
            return tSO.NWH;

        }

        public static string GetBannerMessage()
        {
            List<TSODashboard> items = new List<TSODashboard>();
            TSODashboard tSO = new TSODashboard();
            string constr = ConfigurationManager.ConnectionStrings["Nerolacconstr"].ConnectionString;
            MySqlConnection con = new MySqlConnection(constr);
            DateTime NextDate = DateTime.Now.AddDays(1);
            DataSet ds = new DataSet();
            string query = "select BannerMessage from settings;";
            con.Open();
            MySqlCommand cmd = new MySqlCommand(query, con);
            cmd.CommandType = CommandType.Text;
            MySqlDataAdapter sda = new MySqlDataAdapter(cmd);
            sda.Fill(ds);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                tSO.BannerMessage = ds.Tables[0].Rows[0]["BannerMessage"].ToString();
            }
            else
            {
                tSO.BannerMessage = "";
            }

            con.Close();
            return tSO.BannerMessage;

        }

        public static bool ChkDealerExist(String DealerCode)
        {
            string constr = ConfigurationManager.ConnectionStrings["Nerolacconstr"].ConnectionString;
            MySqlConnection con = new MySqlConnection(constr);
            DataSet ds = new DataSet();
            string query = "select * from lkupdealer where Dealer_SAP_Code='"+ DealerCode + "';";
            con.Open();
            MySqlCommand cmd = new MySqlCommand(query, con);
            cmd.CommandType = CommandType.Text;
            MySqlDataAdapter sda = new MySqlDataAdapter(cmd);
            sda.Fill(ds);
            con.Close();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    

        public static class ExceptionLogging
        {

            private static String ErrorlineNo, Errormsg, extype, exurl, hostIp, ErrorLocation, HostAdd;

            public static void SendErrorToText(Exception ex)
            {
                var line = Environment.NewLine + Environment.NewLine;
                ErrorlineNo = ex.StackTrace.Substring(ex.StackTrace.Length - 7, 7);
                Errormsg = ex.GetType().Name.ToString();
                extype = ex.GetType().ToString();
                exurl = context.Current.Request.Url.ToString();
                ErrorLocation = ex.Message.ToString();
                try
                {
                    string filepath = context.Current.Server.MapPath("~/ExceptionDetailsFile/");  //Text File Path
                    if (!Directory.Exists(filepath))
                    {
                        Directory.CreateDirectory(filepath);
                    }
                    filepath = filepath + DateTime.Today.ToString("dd-MM-yy") + ".txt";   //Text File Name
                    if (!File.Exists(filepath))
                    {
                        File.Create(filepath).Dispose();
                    }
                    using (StreamWriter sw = File.AppendText(filepath))
                    {
                        string error = "Log Written Date:" + " " + DateTime.Now.ToString() + line + "Error Line No :" + " " + ErrorlineNo + line + "Error Message:" + " " + Errormsg + line + "Exception Type:" + " " + extype + line + "Error Location :" + " " + ErrorLocation + line + " Error Page Url:" + " " + exurl + line + "User Host IP:" + " " + hostIp + line;
                        sw.WriteLine("-----------Exception Details on " + " " + DateTime.Now.ToString() + "-----------------");
                        sw.WriteLine("-------------------------------------------------------------------------------------");
                        sw.WriteLine(line);
                        sw.WriteLine(error);
                        sw.WriteLine("--------------------------------*End*------------------------------------------");
                        sw.WriteLine(line);
                        sw.Flush();
                        sw.Close();

                    }

                }
                catch (Exception e)
                {
                    e.ToString();

                }
            }

        }
    }
}