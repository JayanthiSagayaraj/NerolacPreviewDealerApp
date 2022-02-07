using NerolacPreviewApp.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MySql.Data.MySqlClient;
using System.Configuration;
using System.Data;
using System.Drawing;
using PagedList;
using System.Text;
using System.Net;
using RestSharp;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Text.RegularExpressions;


namespace NerolacPreviewApp.Controllers
{
    public class TSOController : Controller
    {
        // GET: TSO
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetBlogComment()
        {

            TSOPreviousclr BCVM = new TSOPreviousclr();
            BCVM.TSOProject = GetTSOProject();
            BCVM.PreviousColourOptions = GetPreviousColourOptions();
            return View(BCVM);
        }

            [HttpGet]
        public ActionResult NewRequestRegistration()
         {
            
            TSOProject BM = GetTSOProject();
            return View(BM);


        }
        public TSOProject GetTSOProject()
        {
            TSOProject tSOProject = new TSOProject();

            string minsize = "", maxsize = "";
            //ViewBag.IorEorMS = ListTSO.PopulateIoreorms();
            //ViewBag.VizOptions = ListTSO.PopulateVizOptions();
            string constr = ConfigurationManager.ConnectionStrings["Nerolacconstr"].ConnectionString;
            MySqlDataAdapter da = new MySqlDataAdapter("select MinSizeKBytes,MaxSizeKBytes from settings", constr);
            DataSet ds = new DataSet();
            da.Fill(ds);
            if (ds.Tables[0].Rows[0][0].ToString() != null)
                minsize = ds.Tables[0].Rows[0][0].ToString();
            maxsize = ds.Tables[0].Rows[0][1].ToString();
            tSOProject = ListTSO.GetDepotDetails(Session["Depot"].ToString());
            TSOProject bModel = new TSOProject()
            {
                

            MinSizeKBytes = minsize,
            MaxSizeKBytes = maxsize,
            Depot=tSOProject.Depot,
            DealerSAPCode=Session["UserID"].ToString(),
            DealerName=Session["UserName"].ToString(),
                
            DepotContactName = tSOProject.DepotContactName,
           DepotName = tSOProject.DepotName,
          DepotAd1 = tSOProject.DepotAd1,
            DepotAd2 = tSOProject.DepotAd2,
            DepotAd3 = tSOProject.DepotAd3,
            DepotAd4 = tSOProject.DepotAd4,
            DepotPinCode = tSOProject.DepotPinCode,
            DepotEmailId = tSOProject.DepotEmailId,
            DepotContactNo = tSOProject.DepotContactNo,
            DepotRemarks = tSOProject.DepotRemarks,
            Comments = GetPreviousColourOptions()
            };
           // bModel = ListTSO.GetDepotDetails(Session["Depot"].ToString());
            //int proid = ListTSO.GetId();
            //ViewBag.Proid = proid + 1;
            return bModel;
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
                    "(select concat('#',red_h,green_h,blue_h) from ancptable where shadecode=mc.shadecode3) as hcode3 from myvizcolours mc where mc.userid='"+Session["userid"]+"' order by mc.projectid DESC,mc.optionno asc limit 0,45", con);

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

            [HttpGet]
        public JsonResult GetLastProject(string UserId)
        {
            try
            {
                int ProjectID = 0; string ProjectValue = ""; string SiteAddress = ""; string CustomerName = "";
                string CustomerContactNo = ""; string DealerSAPCode = ""; string DealerName = "", IorEorMS = "", VizOption = "",
                 FreshPainting = "",projectname="";
                List<TSOProject> TSOProject = new List<TSOProject>();
                string query = string.Format("Select * from fromtso where userid='" + UserId + "' order by projectid desc limit 1");
                string constr = ConfigurationManager.ConnectionStrings["Nerolacconstr"].ConnectionString;
                MySqlConnection connection = new MySqlConnection(constr);
                {
                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {
                        connection.Open();
                        MySqlDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            projectname = (reader["ProjectName"].ToString());
                            ProjectValue = (reader["Projectvalue"].ToString());
                            SiteAddress = (reader["SiteAddress"].ToString());
                            CustomerName = (reader["CustomerName"].ToString());
                            CustomerContactNo = (reader["CustomerContactNo"].ToString());
                            DealerSAPCode = (reader["DealerSAPCode"].ToString());
                            DealerName = (reader["DealerName"].ToString());
                            IorEorMS = (reader["IorEorMS"].ToString());
                            VizOption = (reader["VizOption"].ToString());
                            if (VizOption == "OTS")
                                VizOption = "SCA";
                            FreshPainting = (reader["FreshPainting"].ToString());


                            
                        }

                    }
                    
                    Object[] salaryTypes = new Object[] { projectname, ProjectValue , SiteAddress , CustomerName , 
                        CustomerContactNo , DealerSAPCode,DealerName, IorEorMS,VizOption,FreshPainting};
                    return Json(salaryTypes, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NewRequestRegistration(TSOProject tSOProject)
        {
            Byte[] bytes = null;
            Image img1; Image img2; Image img3;
            int height1 = 0; int height2 = 0; int height3 = 0;
            int width1 = 0; int width2 = 0; int width3 = 0;
            decimal size1 = 0; decimal size2 = 0; decimal size3 = 0;
            try
            {
                if (ModelState.IsValid)
                {
                    // Check if entered dealer code is available in DB

                    //bool IsDealerCodeExist = ListTSO.ChkDealerExist(tSOProject.DealerSAPCode);
                    //if(!IsDealerCodeExist)
                    //{
                    //    ModelState.AddModelError(nameof(TSOProject.DealerSAPCode), "Entered DealerSAPCode not available in the system..!");
                    //    return View(tSOProject);
                    //}

                    if (tSOProject.ImageFile1 != null)
                    {
                        string fileName1 = Path.GetFileNameWithoutExtension(tSOProject.ImageFile1.FileName);
                        string extension = Path.GetExtension(tSOProject.ImageFile1.FileName);
                       

                        if (tSOProject.ImageFile1.FileName != null)

                        {

                            Stream fs = tSOProject.ImageFile1.InputStream;

                            BinaryReader br = new BinaryReader(fs);

                            bytes = br.ReadBytes((Int32)fs.Length);
                        }
                        string path = ConfigurationManager.AppSettings["SISubmittedPath"];
                        fileName1 = Session["UserID"] + "_" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + extension;
                        tSOProject.ImageFileName1 = fileName1;
                        
                            if (!Directory.Exists(path))
                            {
                                Directory.CreateDirectory(path);
                            }

                            tSOProject.ImageFile1.SaveAs(Path.Combine(path, fileName1));
                        img1 = System.Drawing.Image.FromStream(tSOProject.ImageFile1.InputStream);
                        height1 = img1.Height;
                        width1 = img1.Width;
                        size1 = Math.Round(((decimal)tSOProject.ImageFile1.ContentLength / (decimal)1024), 2);
                        if (tSOProject.VizOption == "OTS")
                        {
                             path = ConfigurationManager.AppSettings["SISelectedPath"];
                            fileName1 = Session["UserID"] + "_" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + extension;
                            tSOProject.ImageSelected1 = fileName1;

                            if (!Directory.Exists(path))
                            {
                                Directory.CreateDirectory(path);
                            }

                            tSOProject.ImageFile1.SaveAs(Path.Combine(path, fileName1));
                        }
                    }

                    if (tSOProject.ImageFile2 != null)
                    {
                        string fileName2 = Path.GetFileNameWithoutExtension(tSOProject.ImageFile2.FileName);
                        string extension = Path.GetExtension(tSOProject.ImageFile2.FileName);

                        string path = ConfigurationManager.AppSettings["SISubmittedPath"];
                        fileName2 = Session["UserID"] + "_" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + extension;
                        tSOProject.ImageFileName2 = fileName2;
                      
                            if (!Directory.Exists(path))
                            {
                                Directory.CreateDirectory(path);
                            }

                            tSOProject.ImageFile2.SaveAs(Path.Combine(path, fileName2));
                        img2 = System.Drawing.Image.FromStream(tSOProject.ImageFile2.InputStream);
                        height2 = img2.Height;
                        width2 = img2.Width;
                        size2 = Math.Round(((decimal)tSOProject.ImageFile2.ContentLength / (decimal)1024), 2);

                    }

                    if (tSOProject.ImageFile3 != null)
                    {
                        string fileName3 = Path.GetFileNameWithoutExtension(tSOProject.ImageFile3.FileName);
                        string extension = Path.GetExtension(tSOProject.ImageFile3.FileName);

                        string path = ConfigurationManager.AppSettings["SISubmittedPath"];
                        fileName3 = Session["UserID"] + "_" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + extension;
                        tSOProject.ImageFileName3 = fileName3;
                      
                            if (!Directory.Exists(path))
                            {
                                Directory.CreateDirectory(path);
                            }
                      

                        tSOProject.ImageFile3.SaveAs(Path.Combine(path, fileName3));

                       img3 = System.Drawing.Image.FromStream(tSOProject.ImageFile3.InputStream);
                       height3 = img3.Height;
                       width3 = img3.Width;
                       size3 = Math.Round(((decimal)tSOProject.ImageFile3.ContentLength / (decimal)1024), 2);

                    }
                    WriteToFile("Before SP Insert : " + DateTime.Today.ToString());
                    string pid = "";
                    string InSyComments = string.Empty;
                    string FileSize = string.Concat(Convert.ToString(size1), ",", Convert.ToString(size2),",", Convert.ToString(size3));
                    string Resolution = string.Concat(Convert.ToString(width1),"x", Convert.ToString(height1),",", Convert.ToString(width2), "x", Convert.ToString(height2), ",", Convert.ToString(width3), "x", Convert.ToString(height3));
                    if(size1 > 0 && size1 < 500 | size2 > 0 && size2 <500 | size3 > 0 && size3 <500)
                    {
                       InSyComments = "Some images are below minimum file size. Inform MYVIZSUPPORT immediately if you wish to replace them.";
                    }

                    string constr = ConfigurationManager.ConnectionStrings["Nerolacconstr"].ConnectionString;
                    MySqlConnection MyConn2 = new MySqlConnection(constr);
                    MySqlCommand com = new MySqlCommand();
                    if (tSOProject.VizOption == "OTS")
                    {
                        com = new MySqlCommand("proc_InsertTSODLR", MyConn2);
                    }
                    else
                    {
                        com = new MySqlCommand("proc_InsertTSO", MyConn2);
                    }
                       
                   
                    com.CommandType = CommandType.StoredProcedure; 
              
                    com.Parameters.AddWithValue("@Mode", 1);
                    com.Parameters.AddWithValue("@UserID", Convert.ToString(Session["UserID"]));
                    com.Parameters.AddWithValue("@Project_Id", tSOProject.ProjectID); 
                    com.Parameters.AddWithValue("@CustomerName", tSOProject.CustomerName);
                    com.Parameters.AddWithValue("@ProjectName", tSOProject.ProjectName);
                    com.Parameters.AddWithValue("@ProjectValue", tSOProject.ProjectValue!=string.Empty? tSOProject.ProjectValue :"0");
                    com.Parameters.AddWithValue("@CustomerContactNo", tSOProject.CustomerContactNo);
                    com.Parameters.AddWithValue("@CustomerEMailId", tSOProject.CustomerEMailId);
                    com.Parameters.AddWithValue("@SiteAddress", tSOProject.SiteAddress);
                    com.Parameters.AddWithValue("@Location", tSOProject.Location);
                    com.Parameters.AddWithValue("@VizOption", tSOProject.VizOption);
                    com.Parameters.AddWithValue("@Priority", "N");
                    com.Parameters.AddWithValue("@DealerName", tSOProject.DealerName);
                    com.Parameters.AddWithValue("@DealerSAPCode", tSOProject.DealerSAPCode);
                    com.Parameters.AddWithValue("@IorEorMS", tSOProject.IorEorMS);
                    com.Parameters.AddWithValue("@FreshPainting", tSOProject.FreshPainting);
                    com.Parameters.AddWithValue("@ImageFileName1", tSOProject.ImageFileName1);
                    com.Parameters.AddWithValue("@ImageFileName2", tSOProject.ImageFileName2);
                    com.Parameters.AddWithValue("@ImageFileName3", tSOProject.ImageFileName3);
                    com.Parameters.AddWithValue("@ImageSelected1", tSOProject.ImageSelected1);
                    com.Parameters.AddWithValue("@CPBody1", tSOProject.CPBody1);
                    com.Parameters.AddWithValue("@CPBorder1", tSOProject.CPBorder1);
                    com.Parameters.AddWithValue("@CPHighlight1", tSOProject.CPHighlight1);
                    com.Parameters.AddWithValue("@CPBody2", tSOProject.CPBody2);
                    com.Parameters.AddWithValue("@CPBorder2", tSOProject.CPBorder2);
                    com.Parameters.AddWithValue("@CPHighlight2", tSOProject.CPHighlight2);
                    com.Parameters.AddWithValue("@CPBody3", tSOProject.CPBody3);
                    com.Parameters.AddWithValue("@CPBorder3", tSOProject.CPBorder3);
                    com.Parameters.AddWithValue("@CPHighlight3", tSOProject.CPHighlight3);
                    com.Parameters.AddWithValue("@CPSplRequest", tSOProject.CPSplRequest);
                    com.Parameters.AddWithValue("@Depot", Convert.ToString(Session["Depot"]));
                    com.Parameters.AddWithValue("@DepotName", tSOProject.DepotName);
                    com.Parameters.AddWithValue("@DepotAd1", tSOProject.DepotAd1);
                    com.Parameters.AddWithValue("@DepotAd2", tSOProject.DepotAd2);
                    com.Parameters.AddWithValue("@DepotAd3", tSOProject.DepotAd3);
                    com.Parameters.AddWithValue("@DepotAd4", tSOProject.DepotAd4);
                    com.Parameters.AddWithValue("@DepotPinCode", tSOProject.DepotPinCode);
                    com.Parameters.AddWithValue("@DepotEmailId", tSOProject.DepotEmailId);
                    com.Parameters.AddWithValue("@DepotContactName", tSOProject.DepotContactName);
                    com.Parameters.AddWithValue("@DepotContactNo", tSOProject.DepotContactNo);
                    com.Parameters.AddWithValue("@FileSize", FileSize);
                    com.Parameters.AddWithValue("@Resolution", Resolution);
                    com.Parameters.AddWithValue("@InSyComments", InSyComments);
                    MyConn2.Open();
                    com.ExecuteNonQuery();

                    WriteToFile("END SP Insert : ");


                    MyConn2.Close();


                    if (tSOProject.VizOption == "OTS")
                    {
                        MySqlDataAdapter da = new MySqlDataAdapter("select projectid from fromtso where userid='"+Session["UserID"]+"' and vizoption='OTS' order by projectid desc limit 1",MyConn2);
                        DataSet ds = new DataSet();
                        da.Fill(ds);
                        if (ds.Tables[0].Rows.Count > 0 && ds.Tables[0].Rows[0][0] != null)
                        {
                            WriteToFile("ProjectID: "+ds.Tables[0].Rows[0][0].ToString()+ "Before API Call: ");
                            upload(ds.Tables[0].Rows[0][0].ToString(), bytes);
                        }
                    }
                    WriteToFile("END-- API Call: ");

                    TempData["PrjSuccessmessage"] = "New Project Registered Successfully..!";

                    return RedirectToAction("Index", "TSODashboard");
                   

                }
                else
                {
                   
                    return View(tSOProject);
                }

            }


            catch (Exception ex)
            {
                ListTSO.ExceptionLogging.SendErrorToText(ex);
                return View(tSOProject);
            }
           
        }
private void WriteToFile(string text)
{
    string path =Server.MapPath("~/LogFile/LogFile.txt");
    using (StreamWriter writer = new StreamWriter(path, true))
    {
        writer.WriteLine(string.Format(text+ DateTime.Now));
        writer.Close();
    }
}
public string GetAuthToken()
        {
            try
            {
               //var client = new RestClient("https://coloursgalore.com/CW.API/api/Token/AuthenticateUser");
                var client = new RestClient("https://colourmyspace.co.in/MyVizKN.API/api/Token/AuthenticateUser");
                var userid = Session["UserID"];
                var userpin = Session["UserPin"];
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("Content-Type", "application/json");
                var body = @"{
            " + "\n" +
                @"    ""UserID"": """ + userid + @""",
            " + "\n" +
                @"    ""UserPin"": """ + userpin + @"""
            " + "\n" +
                @"}";
                request.AddParameter("application/json", body, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                var responseJson = response.Content;

                String myJson = response.Content;
                int firststrindex= myJson.IndexOf("token");
                int laststrindex= myJson.IndexOf("auth");
                string stringBetweenTwoStrings = myJson.Substring(firststrindex+10, laststrindex);
                stringBetweenTwoStrings = stringBetweenTwoStrings.Remove(stringBetweenTwoStrings.IndexOf("auth"));
                stringBetweenTwoStrings=stringBetweenTwoStrings.Substring(0,stringBetweenTwoStrings.Length - 5);


                //if (list.data[0].access_token != null)
                //{
                //    access = tokenresponse.data[0].access_token; //This correctly gets the Access Token. You should return this to a class variable so that all the  other functions can access it easily and you're not constantly passing along the variable through them.

                //}
                return (stringBetweenTwoStrings);
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }
        
	





        public string upload(string pid, Byte[] img)
        {
            
            WriteToFile("Before authtoken API Call: " );
           
            string authtoken = GetAuthToken();

            WriteToFile("After authtoken API Call: " );

            //var client = new RestClient("https://coloursgalore.com/CW.API/api/Dpcaseidmasters/TSOCases");
            var client = new RestClient(" https://colourmyspace.co.in/MyVizKN.API/api/Dpcaseidmasters/TSOCases");

            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", authtoken);
            request.AddHeader("projectId", pid);
            request.AddHeader("caseId", "D"+pid);
            request.AddHeader("PreviewCentreCode", Convert.ToString(Session["Depot"]));
            request.AddHeader("PreviewCentreSignInId", Convert.ToString(Session["Depot"]));
            request.AddHeader("PreviewCentreId", Convert.ToString(Session["Depot"]));
            request.AddHeader("Imagetype", "P");
            request.AddHeader("Content-Type", "image/jpeg");
            request.AddParameter("image/jpeg", img, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            var s = response.Content;
            WriteToFile("End of Upload API: " );
            return s;
            
            
        }
        public ActionResult ProjectInfo(int proId)
        {
            ViewBag.VizOptions = ListTSO.PopulateVizOptions();
            
         
            List<SelectListItem> listItems = ListTSO.PopulateIoreorms(proId);
            var list = ListTSO.GetType(proId);
             
            ViewBag.IorEorMS = listItems;

            Session["Id"] = proId;
            var ProInfo = ListTSO.GetProjectByID(proId);
            if (list == "I")
            {
                
                listItems.Add(new SelectListItem() { Text = "Exterior", Value = "E" });
                listItems.Add(new SelectListItem() { Text = "MultiStorey", Value = "MS" });
                ProInfo.IorEorMS = "Interior";
                ViewBag.IorEorMS = listItems;
            }
            if (list == "E")
            {
                listItems.Add(new SelectListItem() { Text = "Interior", Value = "I", Selected = false });
             
                listItems.Add(new SelectListItem() { Text = "MultiStorey", Value = "MS", Selected = false });
                ProInfo.IorEorMS = "Exterior";
                ViewBag.IorEorMS = listItems;
            }

            if (list == "MS")
            {
                listItems.Add(new SelectListItem() { Text = "Interior", Value = "I", Selected = false });
                listItems.Add(new SelectListItem() { Text = "Exterior", Value = "E", Selected = false });
                
                ProInfo.IorEorMS = "MultiStorey";
                ViewBag.IorEorMS = listItems;
            }



            return View(ProInfo);
        }
        [HttpPost]
        public ActionResult ProjectInfo(TSOProject tSOProject)
        { 
            try
            {
                if (tSOProject.ProjectName != null && tSOProject.CustomerName != null)
                {
                    if (tSOProject.ImageFile1 != null)
                    {
                        string fileName1 = Path.GetFileNameWithoutExtension(tSOProject.ImageFile1.FileName);
                        string extension = Path.GetExtension(tSOProject.ImageFile1.FileName);

                        string path = ConfigurationManager.AppSettings["SISubmittedPath"];
                        fileName1 = fileName1 + extension;
                      
                            tSOProject.ImageFileName1 = fileName1;
                        
                            if (!Directory.Exists(path))
                            {
                                Directory.CreateDirectory(path);
                            }

                            tSOProject.ImageFile1.SaveAs(Path.Combine(path, fileName1));
 
                    
                    }

                    if (tSOProject.ImageFile2 != null)
                    {
                        string fileName2 = Path.GetFileNameWithoutExtension(tSOProject.ImageFile2.FileName);
                        string extension = Path.GetExtension(tSOProject.ImageFile2.FileName);

                        string path = ConfigurationManager.AppSettings["SISubmittedPath"];
                        fileName2 = fileName2 + extension;
                        
                            tSOProject.ImageFileName2 = fileName2;
                       
                            if (!Directory.Exists(path))
                            {
                                Directory.CreateDirectory(path);
                            }

                            tSOProject.ImageFile2.SaveAs(Path.Combine(path, fileName2)); 
                    } 

                    if (tSOProject.ImageFile3 != null)
                    {
                        string fileName3 = Path.GetFileNameWithoutExtension(tSOProject.ImageFile3.FileName);
                        string extension = Path.GetExtension(tSOProject.ImageFile3.FileName);

                        string path = ConfigurationManager.AppSettings["SISubmittedPath"];
                        fileName3 = fileName3 + extension; 
                         
                            tSOProject.ImageFileName3 = fileName3;
                        
                            if (!Directory.Exists(path))
                            {
                                Directory.CreateDirectory(path);
                            }


                            tSOProject.ImageFile3.SaveAs(Path.Combine(path, fileName3)); 
                       
                    } 
                    string constr = ConfigurationManager.ConnectionStrings["Nerolacconstr"].ConnectionString;
                    MySqlConnection MyConn2 = new MySqlConnection(constr);
                    MySqlCommand com = new MySqlCommand("proc_InsertTSO", MyConn2);
                    com.CommandType = CommandType.StoredProcedure;

                    com.Parameters.AddWithValue("@Mode", 2);
                    com.Parameters.AddWithValue("@UserID", Convert.ToString(Session["UserID"]));
                    com.Parameters.AddWithValue("@Project_Id", Convert.ToInt32(Session["Id"]));
                    com.Parameters.AddWithValue("@CustomerName", tSOProject.CustomerName);
                    com.Parameters.AddWithValue("@ProjectName", tSOProject.ProjectName);
                    com.Parameters.AddWithValue("@ProjectValue", tSOProject.ProjectValue);
                    com.Parameters.AddWithValue("@CustomerContactNo", tSOProject.CustomerContactNo);
                    com.Parameters.AddWithValue("@CustomerEMailId", tSOProject.CustomerEMailId);
                    com.Parameters.AddWithValue("@SiteAddress", tSOProject.SiteAddress);
                    com.Parameters.AddWithValue("@Location", tSOProject.Location);
                    com.Parameters.AddWithValue("@VizOption", tSOProject.VizOption);
                    com.Parameters.AddWithValue("@Priority", tSOProject.Priority);
                    com.Parameters.AddWithValue("@DealerName", tSOProject.DealerName);
                    com.Parameters.AddWithValue("@DealerSAPCode", tSOProject.DealerSAPCode);
                    com.Parameters.AddWithValue("@IorEorMS", tSOProject.IorEorMS);
                    com.Parameters.AddWithValue("@FreshPainting", tSOProject.FreshPainting);
                    com.Parameters.AddWithValue("@ImageFileName1", tSOProject.ImageFileName1);
                    com.Parameters.AddWithValue("@ImageFileName2", tSOProject.ImageFileName2);
                    com.Parameters.AddWithValue("@ImageFileName3", tSOProject.ImageFileName3);
                    com.Parameters.AddWithValue("@CPBody1", tSOProject.CPBody1);
                    com.Parameters.AddWithValue("@CPBorder1", tSOProject.CPBorder1);
                    com.Parameters.AddWithValue("@CPHighlight1", tSOProject.CPHighlight1);
                    com.Parameters.AddWithValue("@CPBody2", tSOProject.CPBody2);
                    com.Parameters.AddWithValue("@CPBorder2", tSOProject.CPBorder2);
                    com.Parameters.AddWithValue("@CPHighlight2", tSOProject.CPHighlight2);
                    com.Parameters.AddWithValue("@CPBody3", tSOProject.CPBody3);
                    com.Parameters.AddWithValue("@CPBorder3", tSOProject.CPBorder3);
                    com.Parameters.AddWithValue("@CPHighlight3", tSOProject.CPHighlight3);
                    com.Parameters.AddWithValue("@CPSplRequest", tSOProject.CPSplRequest);
                    MyConn2.Open();
                    com.ExecuteNonQuery();
                    MyConn2.Close(); 
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return RedirectToAction("ProjectInfo");
                }

            }


            catch (Exception ex)
            {
                throw ex;
            }

        }

        [HttpPost]
        public string updateprostatus(string PID)
        {
            if (!String.IsNullOrEmpty(PID))
            {
                string constr = ConfigurationManager.ConnectionStrings["Nerolacconstr"].ConnectionString;
                MySqlConnection MyConn2 = new MySqlConnection(constr);
                MyConn2.Open();

                MySqlCommand com = new MySqlCommand("update fromtso set wstatus='30',M_Date=current_timestamp() where projectID='"+PID+"'", MyConn2);
                com.ExecuteNonQuery();
                com = new MySqlCommand("update fromtso set wstatus='40',M_Date=current_timestamp() where projectID='" + PID + "'", MyConn2);
                com.ExecuteNonQuery();
                
                MyConn2.Close();


                return "Thank you " +PID + "- Project Updated.";
                //return Json(new { success = true, responseText = "Thank you " + PID + "- Project Updated." }, JsonRequestBehavior.AllowGet);
            }
          
            else
                return "Please enter valid projectid.";
                //return Json(new { success = false, responseText = "Please enter valid projectid." }, JsonRequestBehavior.AllowGet);

        }
        public ActionResult BizList()
        {
            var bizStatus = ListTSO.GetBizList();
            ViewBag.biz = bizStatus;
            return View(); 
        }
        public ActionResult Projectsupdate()
        {
            //var Projectsupdate = ListTSO.projectsupdate();
            //ViewBag.proj = Projectsupdate;
            return View();
        }

        [HttpGet]
        public JsonResult GetprojectID(string ProjectID)
        {
            try
            {
                string ProjectID1 = ""; 
                List<projectsupdate > TSOProject = new List<projectsupdate>();
                string query = string.Format("Select * from fromtso where projectid='" + ProjectID + "' and wstatus < 40");
                string constr = ConfigurationManager.ConnectionStrings["Nerolacconstr"].ConnectionString;
                MySqlConnection connection = new MySqlConnection(constr);
                {
                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {
                        connection.Open();
                        MySqlDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            
                            ProjectID1 = (reader["ProjectID"].ToString());



                        }

                    }
                    Object[] salaryTypes = new Object[] { ProjectID1 };
                    return Json(salaryTypes, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public ActionResult BizStatus(int proId = 11)
        {
            ViewBag.Proid = proId;
            ViewBag.BStatus = ListTSO.PopulateBizStatus(proId);
            var bizStatus = ListTSO.GetBizStatusByID(proId); 
            Session["Id"] = proId;
            List<SelectListItem> listItems = new List<SelectListItem>();
            var list = ListTSO.GetBiz(proId); 
            ViewBag.IorEorMS = listItems; 
            Session["Id"] = proId; 
            if (list == "10")
            {
                listItems.Add(new SelectListItem() { Text = "In Progress", Value = "10",Selected=true });
                listItems.Add(new SelectListItem() { Text = "Won", Value = "20", Selected = false });
                listItems.Add(new SelectListItem() { Text = "Lost", Value = "90", Selected = false });
                bizStatus.BStatus = "In Progress";
                ViewBag.BStatus = listItems;
            }
            if (list == "20")
            {
                listItems.Add(new SelectListItem() { Text = "Won", Value = "20", Selected = true });
                listItems.Add(new SelectListItem() { Text = "In Progress", Value = "10", Selected = false }); 
                listItems.Add(new SelectListItem() { Text = "Lost", Value = "90", Selected = false });
                bizStatus.BStatus = "Won";
                ViewBag.BStatus = listItems;
            }
            if (list == "90")
            {
                listItems.Add(new SelectListItem() { Text = "Lost", Value = "90", Selected = true });
                listItems.Add(new SelectListItem() { Text = "In Progress", Value = "10", Selected = false });
                listItems.Add(new SelectListItem() { Text = "Won", Value = "20", Selected = false });
                bizStatus.BStatus = "Lost";
                ViewBag.BStatus = listItems;
            }
            return View(bizStatus);
        }
        [HttpPost]
        public ActionResult BizStatus(BizStatus biz)
        {
            if (biz != null)
            {
                string constr = ConfigurationManager.ConnectionStrings["Nerolacconstr"].ConnectionString;
                MySqlConnection MyConn2 = new MySqlConnection(constr);
                MySqlCommand com = new MySqlCommand("Sp_InsertBizStatus", MyConn2);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@Mode", 1);
                com.Parameters.AddWithValue("@Project_Id",(Session["Id"])); 
                com.Parameters.AddWithValue("@Bstatus", biz.BStatus); 
                com.Parameters.AddWithValue("@StatusDate", System.DateTime.Now);
                com.Parameters.AddWithValue("@StatusReason", biz.StatusReason);  
                MyConn2.Open();
                com.ExecuteNonQuery();
                MyConn2.Close();
                
                    return RedirectToAction("Index", "TSODashboard");
                
            }
            else
            {
                return View(biz);
            }
        }


        


        public ActionResult statsCount()
        {
            List<Stats> stats = ListTSO.GetStatsList();
            return View(stats);

        }


        public ActionResult statsList()
        {
            return View();

        }

        public ActionResult myProfileView(string userid)
        {
            ViewBag.userid = userid;
            return View();

        }

        [HttpGet]
        public ActionResult myProfile()
        {
            String userid = Session["UserID"].ToString();
            var userDetail = ListTSO.GetUserByID(userid);
            return View(userDetail);

        }

        [HttpGet]
        public ActionResult FlashMessages()
        {

            FlashMessages BM = GetFlashmsgs();
            Session["FlashMessages"] = "0";
            return View(BM);
        }
        public FlashMessages GetFlashmsgs()
        {
           
            

            FlashMessages model = new FlashMessages()
            {

                FlashMessagesLists = GetFlashMessagesLists()


            };






            return model;
        }
        public List<FlashMessagesList> GetFlashMessagesLists()
        {
            string Region = "";
            String userid = Session["UserID"].ToString();
           
            List<FlashMessagesList> FlashMessagesList = new List<FlashMessagesList>();
            var FlashMessagesLists = new List<FlashMessagesList>();
            string constr = ConfigurationManager.ConnectionStrings["Nerolacconstr"].ConnectionString;

            
            using (MySqlConnection con = new MySqlConnection(constr))
            {
                MySqlDataAdapter da1 = new MySqlDataAdapter("select region from knusermaster where empid='" + userid + "'", con);
                DataSet ds1 = new DataSet();
                da1.Fill(ds1);
                if (ds1.Tables[0].Rows[0][0] != null)
                    Region = ds1.Tables[0].Rows[0][0].ToString(); DataSet ds = new DataSet();
                MySqlCommand cmd = new MySqlCommand("Sp_FlashMessageList", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserId", userid);
                cmd.Parameters.AddWithValue("@RegionName", Region);
                cmd.CommandTimeout = 1000;

                con.Open();



                using (MySqlDataReader sdr = cmd.ExecuteReader())
                {
                    while (sdr.Read())
                    {

                        FlashMessagesLists.Add(new FlashMessagesList
                        {
                            
                            Title1 = String.IsNullOrEmpty(sdr["MessageTitle"].ToString()) ? "-" : sdr["MessageTitle"].ToString(),
                            Text1 = String.IsNullOrEmpty(sdr["MessageText"].ToString()) ? "-" : sdr["MessageText"].ToString(),
                            msgtype = String.IsNullOrEmpty(sdr["MessageType"].ToString()) ? "-" : sdr["MessageType"].ToString()


                        });
                    }
                }
                var list = FlashMessagesLists.ToList();


                con.Close();
                return list;
            }
        }



        [HttpGet]
        public ActionResult feedback()
        {
            String userid = Session["UserID"].ToString();
            string userlevel = Session["UserLevel"].ToString();
            ViewBag.pn = ListTSO.PopulateProjectName(userid,userlevel);
            ViewBag.Category = ListTSO.PopulateFeedBackCategory();
            return View();

        }

        public ActionResult DealersList(string sortOrder, string q, int page = 1, int pageSize = 10)
        {
            ViewBag.searchQuery = String.IsNullOrEmpty(q) ? "" : q;

            page = page > 0 ? page : 1;
            pageSize = pageSize > 0 ? pageSize : 5;

            string constr = ConfigurationManager.ConnectionStrings["Nerolacconstr"].ConnectionString;
          
            using (MySqlConnection con = new MySqlConnection(constr))
            {
                DataSet ds = new DataSet();
                MySqlCommand com = new MySqlCommand("select * from lkupdealer", con);
                com.CommandTimeout = 1000;

                con.Open();

                List<DealerList> lstdealers = new List<DealerList>();
                var DealersList = new List<DealerList>();

                using (MySqlDataReader sdr = com.ExecuteReader())
                {
                    while (sdr.Read())
                    {
                        
                        DealersList.Add(new DealerList
                        {
                            Dealer_SAP_Code= sdr["Dealer_SAP_Code"].ToString(),
                            Dealer_Name = sdr["Dealer_Name"].ToString(),
                            City = sdr["City"].ToString(),
                            Address_Line2 = sdr["District"].ToString(),
                            PostalCode = sdr["PostalCode"].ToString(),
                            TelephoneNo = sdr["TelephoneNo"].ToString()
                         });
                    }
                }
                var list = DealersList.ToList();

                if (!String.IsNullOrEmpty(q))
                {
                    q = q.ToLower();
                    list = list.Where(s => s.Dealer_SAP_Code.ToLower().Contains(q) || s.Dealer_Name.ToLower().Contains(q) || s.City.ToLower().Contains(q) ||s.PostalCode.ToLower().Contains(q) || s.TelephoneNo.ToLower().Contains(q)).ToList();
                }

                con.Close();
                return View(list.ToPagedList(page, pageSize));
            }
        }

        public ActionResult ColourChoices(int projectid)
        {
            Session["ProjectID"] = projectid;
            ColorChoices BM = GetColourchoices();
            return View(BM);
        }
        public ColorChoices GetColourchoices()
        {
            string constr = ConfigurationManager.ConnectionStrings["Nerolacconstr"].ConnectionString;
            MySqlConnection con = new MySqlConnection(constr);
            DataSet ds = new DataSet();
            //MySqlCommand com = new MySqlCommand("select ft.*,kn.name,kn.UserContactNo,kn.MailID,dp.CaseID,(select concat('#',red_h,green_h,blue_h) from ancptable where shadecode=FT.cpbody1) as hr1,(select concat('#',red_h,green_h,blue_h) from ancptable where shadecode=FT.cpbody2) as hr2,(select concat('#',red_h,green_h,blue_h) from ancptable where shadecode=FT.cpbody3) as hr3,(select concat('#',red_h,green_h,blue_h) from ancptable where shadecode=FT.cpborder1) as hg1,(select concat('#',red_h,green_h,blue_h) from ancptable where shadecode=FT.cpborder2) as hg2,(select concat('#',red_h,green_h,blue_h) from ancptable where shadecode=FT.cpborder3) as hg3,(select concat('#',red_h,green_h,blue_h) from ancptable where shadecode=FT.cphighlight1) as hb1,(select concat('#',red_h,green_h,blue_h) from ancptable where shadecode=FT.cphighlight2) as hb2,(select concat('#',red_h,green_h,blue_h) from ancptable where shadecode=FT.cphighlight3) as hb3,(select shadename from ancptable where shadecode=FT.cpbody1) as snbody1,(select shadename from ancptable where shadecode=FT.cpbody2) as snbody2,(select shadename from ancptable where shadecode=FT.cpbody3) as snbody3,(select shadename from ancptable where shadecode=FT.cpborder1) as snborder1,(select shadename from ancptable where shadecode=FT.cpborder2) as snborder2,(select shadename from ancptable where shadecode=FT.cpborder3) as snborder3,(select shadename from ancptable where shadecode=FT.cphighlight1) as snhl1,(select shadename from ancptable where shadecode=FT.cphighlight2) as snhl2,(select shadename from ancptable where shadecode=FT.cphighlight3) as snhl3 from fromtso ft left join knusermaster kn on ft.userid=kn.empid left join dpcaseidmaster dp on ft.projectid=dp.OpportunityNo where projectid='" + projectid + "'", con);
            MySqlCommand com = new MySqlCommand("SP_Colourchoices_New", con);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@Mode", 1);
            com.Parameters.AddWithValue("@ProjectID", Session["ProjectID"]);
            con.Close();
            con.Open();
            com.ExecuteNonQuery();
            MySqlDataAdapter ad = new MySqlDataAdapter(com);
            ad.Fill(ds);
            var colorchoices = ds.Tables[0].AsEnumerable();

            ColorChoices model = new ColorChoices()
            {
                ProjectID = Convert.ToInt32(ds.Tables[0].Rows[0]["ProjectID"]),
                ProjectName = Convert.ToString(ds.Tables[0].Rows[0]["ProjectName"]),
                NeroliteID = Convert.ToString(ds.Tables[0].Rows[0]["UserID"]),
                NeroliteName = Convert.ToString(ds.Tables[0].Rows[0]["name"]),
                NeroliteNo = Convert.ToString(ds.Tables[0].Rows[0]["UserContactNo"]),
                NeroliteEmail = Convert.ToString(ds.Tables[0].Rows[0]["MailID"]),
                Body1 = String.IsNullOrEmpty(Convert.ToString(ds.Tables[0].Rows[0]["CPBody1"]))?"-": Convert.ToString(ds.Tables[0].Rows[0]["CPBody1"]),
                Border1 = String.IsNullOrEmpty(Convert.ToString(ds.Tables[0].Rows[0]["CPBorder1"])) ? "-" : Convert.ToString(ds.Tables[0].Rows[0]["CPBorder1"]),
                Highlight1 = String.IsNullOrEmpty(Convert.ToString(ds.Tables[0].Rows[0]["CPHighlight1"])) ? "-" : Convert.ToString(ds.Tables[0].Rows[0]["CPHighlight1"]),
                Body2 = String.IsNullOrEmpty(Convert.ToString(ds.Tables[0].Rows[0]["CPBody2"])) ? "-" : Convert.ToString(ds.Tables[0].Rows[0]["CPBody2"]),
                Border2 = String.IsNullOrEmpty(Convert.ToString(ds.Tables[0].Rows[0]["CPBorder2"])) ? "-" : Convert.ToString(ds.Tables[0].Rows[0]["CPBorder2"]),
                Highlight2 = String.IsNullOrEmpty(Convert.ToString(ds.Tables[0].Rows[0]["CPHighlight2"])) ? "-" : Convert.ToString(ds.Tables[0].Rows[0]["CPHighlight2"]),
                Body3 = String.IsNullOrEmpty(Convert.ToString(ds.Tables[0].Rows[0]["CPBody3"])) ? "-" : Convert.ToString(ds.Tables[0].Rows[0]["CPBody3"]),
                Border3 = String.IsNullOrEmpty(Convert.ToString(ds.Tables[0].Rows[0]["CPBorder3"])) ? "-" : Convert.ToString(ds.Tables[0].Rows[0]["CPBorder3"]),
                Highlight3 = String.IsNullOrEmpty(Convert.ToString(ds.Tables[0].Rows[0]["CPHighlight3"])) ? "-" : Convert.ToString(ds.Tables[0].Rows[0]["CPHighlight3"]),
                SpecialRequest = Convert.ToString(ds.Tables[0].Rows[0]["CPSplRequest"]),

                InsyComments = Convert.ToString(ds.Tables[0].Rows[0]["InSyComments"]),
                Options= Convert.ToString(ds.Tables[0].Rows[0]["IorEorMS"]),
                Resolution= Convert.ToString(ds.Tables[0].Rows[0]["Resolution"]),
                Size= Convert.ToString(ds.Tables[0].Rows[0]["FileSize"]),
                caseID= Convert.ToString(ds.Tables[0].Rows[0]["CaseID"]),
                hr1 = Convert.ToString(ds.Tables[0].Rows[0]["hr1"]),
                hr2 = Convert.ToString(ds.Tables[0].Rows[0]["hr2"]),
                hr3 = Convert.ToString(ds.Tables[0].Rows[0]["hr3"]),
                hg1 = Convert.ToString(ds.Tables[0].Rows[0]["hg1"]),
                hg2 = Convert.ToString(ds.Tables[0].Rows[0]["hg2"]),
                hg3 = Convert.ToString(ds.Tables[0].Rows[0]["hg3"]),
                hb1 = Convert.ToString(ds.Tables[0].Rows[0]["hb1"]),
                hb2 = Convert.ToString(ds.Tables[0].Rows[0]["hb2"]),
                hb3 = Convert.ToString(ds.Tables[0].Rows[0]["hb3"]),
                snBody1 = Convert.ToString(ds.Tables[0].Rows[0]["snbody1"]),
                snBody2 = Convert.ToString(ds.Tables[0].Rows[0]["snbody2"]),
                snBody3 = Convert.ToString(ds.Tables[0].Rows[0]["snbody3"]),
                snBorder1 = Convert.ToString(ds.Tables[0].Rows[0]["snborder1"]),
                snBorder2 = Convert.ToString(ds.Tables[0].Rows[0]["snborder2"]),
                snBorder3 = Convert.ToString(ds.Tables[0].Rows[0]["snborder3"]),
                snhl1 = Convert.ToString(ds.Tables[0].Rows[0]["snhl1"]),
                snhl2 = Convert.ToString(ds.Tables[0].Rows[0]["snhl2"]),
                snhl3 = Convert.ToString(ds.Tables[0].Rows[0]["snhl3"]),
                remarks=Convert.ToString(ds.Tables[0].Rows[0]["Remarks"]),
                coloursused = Getcoloursused()


            };


            con.Close();
            return (model);
        }
        public List<ColorsUsed> Getcoloursused()
        {
            List<ColorsUsed> lstColourPalette = new List<ColorsUsed>();
            var ColorsUsed = new List<ColorsUsed>();
            string constr = ConfigurationManager.ConnectionStrings["Nerolacconstr"].ConnectionString;
            MySqlConnection con = new MySqlConnection(constr);
            MySqlDataAdapter da1 = new MySqlDataAdapter("select caseid from dpcaseidmaster where OpportunityNo='" + Session["ProjectID"] + "'", con);
            DataSet ds1 = new DataSet();
            DataSet ds = new DataSet();
            da1.Fill(ds1);

            if (ds1.Tables[0].Rows.Count > 0 && ds1.Tables[0].Rows[0][0] != null)
            {
                using (con = new MySqlConnection(constr))
                {
                    var dpcaseid = ds1.Tables[0].Rows[0][0].ToString();
                    MySqlCommand com = new MySqlCommand("Sp_GetColoursUsed", con);
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@Mode", 1);
                    com.Parameters.AddWithValue("@caseidno", ds1.Tables[0].Rows[0][0].ToString());

                    com.CommandTimeout = 1000;

                    con.Open();



                    using (MySqlDataReader sdr = com.ExecuteReader())
                    {
                        while (sdr.Read())
                        {

                            ColorsUsed.Add(new ColorsUsed
                            {
                                Option = String.IsNullOrEmpty(sdr["optionno"].ToString()) ? "-" : sdr["optionno"].ToString(),
                                Group = String.IsNullOrEmpty(sdr["colorfamily"].ToString()) ? "-" : sdr["colorfamily"].ToString(),
                                ShadeCode = String.IsNullOrEmpty(sdr["shadecode"].ToString()) ? "-" : sdr["shadecode"].ToString(),
                                ShadeName = String.IsNullOrEmpty(sdr["shadename"].ToString()) ? "-" : sdr["shadename"].ToString(),
                                hexacode = String.IsNullOrEmpty(sdr["hexacode"].ToString()) ? "-" : sdr["hexacode"].ToString()



                            });
                        }
                    }
                    con.Close();

                }


            }
            var list = ColorsUsed.ToList();
            return list;
        }
        public ActionResult Active_InActiveUserDetails(string sortOrder, string q, int page = 1, int pageSize = 1000)
        {
            ViewBag.searchQuery = String.IsNullOrEmpty(q) ? "" : q;

            page = page > 0 ? page : 1;
            pageSize = pageSize > 0 ? pageSize : 1000;

            ViewBag.IDSortParam = sortOrder == "ID" ? "ProjectID_desc" : "ID";
            ViewBag.StatusSortParam = sortOrder == "status" ? "status_desc" : "status";
            ViewBag.NameSortParam = sortOrder == "Name" ? "Name_desc" : "Name";
            ViewBag.ProjectcountSortParam = sortOrder == "ProjectCount" ? "ProjectCount_desc" : "ProjectCount";
            ViewBag.LPIDSortParam = sortOrder == "LastProjectID" ? "LastProjectID_desc" : "LastProjectID";
            ViewBag.DesignationSortParam = sortOrder == "Designation" ? "Designation_desc" : "Designation";
            ViewBag.ASMIDSortParam = sortOrder == "ASMID" ? "ASMID_desc" : "ASMID";
            ViewBag.ASMIDSortParam = sortOrder == "ASMName" ? "ASMName_desc" : "ASMName";




            ViewBag.CurrentSort = sortOrder;
            string constr = ConfigurationManager.ConnectionStrings["Nerolacconstr"].ConnectionString;

            using (MySqlConnection con = new MySqlConnection(constr))
            {
                DataSet ds = new DataSet();
                MySqlCommand com = new MySqlCommand("select * from active_inactive_users", con);
            
                com.CommandTimeout = 1000;

                con.Open();

                List<Active_InActiveUserDetails> lstdealers = new List<Active_InActiveUserDetails>();
                var Active_InActiveUserDetails = new List<Active_InActiveUserDetails>();

                using (MySqlDataReader sdr = com.ExecuteReader())
                {
                    while (sdr.Read())
                    {

                        Active_InActiveUserDetails.Add(new Active_InActiveUserDetails
                        {
                            ID = String.IsNullOrEmpty(sdr["ID"].ToString()) ? "-": sdr["ID"].ToString(),
                            Name = String.IsNullOrEmpty(sdr["Name"].ToString()) ? "-" : sdr["Name"].ToString(),
                            Designation = String.IsNullOrEmpty(sdr["Designation"].ToString()) ? "-" : sdr["Designation"].ToString(),
                            Region = String.IsNullOrEmpty(sdr["Region"].ToString()) ? "-" : sdr["Region"].ToString(),
                            Depotcode = String.IsNullOrEmpty(sdr["Depotcode"].ToString()) ? "-" : sdr["Depotcode"].ToString(),
                            Depotname = String.IsNullOrEmpty(sdr["Depotname"].ToString()) ? "-" : sdr["Depotname"].ToString(),
                            ASMID = String.IsNullOrEmpty(sdr["ASM ID"].ToString())?"-" :sdr["ASM ID"].ToString(),

                            ASMName = String.IsNullOrEmpty(sdr["ASM Name"].ToString()) ? "-" : sdr["ASM Name"].ToString(),
                            EmailID = String.IsNullOrEmpty(sdr["EmailID"].ToString()) ? "-" : sdr["EmailID"].ToString(),
                            ContactNo = String.IsNullOrEmpty(sdr["ContactNo"].ToString()) ? "-" : sdr["ContactNo"].ToString(),
                            Status = String.IsNullOrEmpty(sdr["Status"].ToString()) ? "-" : sdr["Status"].ToString(),
                            ProjectCount = String.IsNullOrEmpty(sdr["ProjectCount"].ToString()) ? "-" : sdr["ProjectCount"].ToString(),
                            LastProjectID = String.IsNullOrEmpty(sdr["LastProjectID"].ToString()) ? "-" : sdr["LastProjectID"].ToString(),
                            LastProjectDate = String.IsNullOrEmpty(sdr["LastProjectDate"].ToString()) ? "-" : sdr["LastProjectDate"].ToString(),
                            ASMContact= String.IsNullOrEmpty(sdr["ASM Contact"].ToString()) ? "-" : sdr["ASM Contact"].ToString()

                        });
                    }
                }
                var list = Active_InActiveUserDetails.ToList();

                if (!String.IsNullOrEmpty(q))
                {
                    q = q.ToLower();
                    list = list.Where(s => s.ID.ToLower().Contains(q) || s.Name.ToLower().Contains(q) || s.Designation.ToLower().Contains(q) || s.Region.ToLower().Contains(q) || s.Depotcode.ToLower().Contains(q) || s.Depotname.ToLower().Contains(q) || s.ASMID.ToLower().Contains(q) || s.ASMName.ToLower().Contains(q) || s.ASMContact.ToLower().Contains(q) || s.EmailID.ToLower().Contains(q)
                    || s.ContactNo.ToLower().Contains(q) || s.Status.ToLower().Contains(q) || s.ProjectCount.ToLower().Contains(q) || s.ProjectCount.ToLower().Contains(q) || s.LastProjectID.ToLower().Contains(q) || s.LastProjectDate.ToLower().Contains(q)).ToList();
                }
                switch (sortOrder)
                {
                    case "ProjectID_desc":
                        list = list.OrderByDescending(x => x.ID).ToList();
                        break;
                    case "ID":
                        list = list.OrderBy(x => x.ID).ToList();
                        break;

                    case "Name_desc":
                        list = list.OrderByDescending(x => x.Name).ToList();
                        break;
                    case "Name":
                        list = list.OrderBy(x => x.Name).ToList();
                        break;
                    case "status_desc":
                        list = list.OrderByDescending(x => x.Status).ToList();
                        break;
                    case "status":
                        list = list.OrderBy(x => x.Status).ToList();
                        break;
                        
                        case "ProjectCount_desc":
                        list = list.OrderByDescending(x => x.ProjectCount).ToList();
                        break;
                        case "ProjectCount":
                        list = list.OrderBy(x => x.ProjectCount).ToList();
                        break;
                    case "LastProjectID_desc":
                        list = list.OrderByDescending(x => x.LastProjectID).ToList();
                        break;
                    case "LastProjectID":
                        list = list.OrderBy(x => x.LastProjectID).ToList();
                        break;
                    case "Designation_desc":
                        list = list.OrderByDescending(x => x.Designation).ToList();
                        break;
                    case "Designation":
                        list = list.OrderBy(x => x.Designation).ToList();
                        break;
                    case "ASMID_desc":
                        list = list.OrderByDescending(x => x.ASMID).ToList();
                        break;
                    case "ASMID":
                        list = list.OrderBy(x => x.ASMID).ToList();
                        break;
                    case "ASMName_desc":
                        list = list.OrderByDescending(x => x.ASMName).ToList();
                        break;
                    case "ASMName":
                        list = list.OrderBy(x => x.ASMName).ToList();
                        break;

                    default:
                        break;
                }
               
                con.Close();
                return View(list.ToPagedList(page, pageSize));
            }
        }

        public ActionResult DealerDetails(int? Dealer_SAP_Code)
        {
            string constr = ConfigurationManager.ConnectionStrings["Nerolacconstr"].ConnectionString;
            MySqlConnection con = new MySqlConnection(constr);
            DataSet ds = new DataSet();
            MySqlCommand com = new MySqlCommand("select * from lkupdealer where Dealer_SAP_Code='"+ Dealer_SAP_Code + "'", con);
           
            con.Open();
            com.ExecuteNonQuery();
            MySqlDataAdapter ad = new MySqlDataAdapter(com);
            ad.Fill(ds);
            var DealerDetail = ds.Tables[0].AsEnumerable();

            DealerList model = new DealerList()
            {
                Dealer_SAP_Code = Convert.ToString(ds.Tables[0].Rows[0]["Dealer_SAP_Code"]),
                Dealer_Name = Convert.ToString(ds.Tables[0].Rows[0]["Dealer_Name"]),
                Address_Line1 = Convert.ToString(ds.Tables[0].Rows[0]["Address_Line1"]),
                Address_Line2 = Convert.ToString(ds.Tables[0].Rows[0]["Address_Line2"]),
               Street = Convert.ToString(ds.Tables[0].Rows[0]["Street"]),
               District = Convert.ToString(ds.Tables[0].Rows[0]["District"]),
                PostalCode = Convert.ToString(ds.Tables[0].Rows[0]["PostalCode"]),
                City = Convert.ToString(ds.Tables[0].Rows[0]["City"]),
                State = Convert.ToString(ds.Tables[0].Rows[0]["State"]),
                StateCode = Convert.ToString(ds.Tables[0].Rows[0]["StateCode"]),
                LAT = Convert.ToString(ds.Tables[0].Rows[0]["LAT"]),
                LONG = Convert.ToString(ds.Tables[0].Rows[0]["LONG"]),
                TelephoneNo = Convert.ToString(ds.Tables[0].Rows[0]["TelephoneNo"]),
                SMS_Mobile = Convert.ToString(ds.Tables[0].Rows[0]["SMS_Mobile"]),
                Language = Convert.ToString(ds.Tables[0].Rows[0]["Language"]),
                VR_User = Convert.ToString(ds.Tables[0].Rows[0]["VR_User"]),
               
                Remarks = Convert.ToString(ds.Tables[0].Rows[0]["Remarks"]),
                StatusYN = Convert.ToString(ds.Tables[0].Rows[0]["StatusYN"])
                
               
            };
            con.Close();
            return View(model);
        }

        [HttpPost]
        public ActionResult feedback(Feedback feedback)
        {
            if (feedback != null)
            {
                string constr = ConfigurationManager.ConnectionStrings["Nerolacconstr"].ConnectionString;
                MySqlConnection MyConn2 = new MySqlConnection(constr);
                MySqlCommand com = new MySqlCommand("Sp_InsertFeedback", MyConn2);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@Mode", 1);
                com.Parameters.AddWithValue("@ProjectID", feedback.ProjectName);
                com.Parameters.AddWithValue("@UserId", Session["UserID"].ToString());
                com.Parameters.AddWithValue("@FeedbackDate", System.DateTime.Now);
                com.Parameters.AddWithValue("@category", feedback.Category);
                com.Parameters.AddWithValue("@feedback", feedback.TSOInput);
                com.Parameters.AddWithValue("@ReplyGiven", String.Empty);
                com.Parameters.AddWithValue("@Replyby", String.Empty);
                com.Parameters.AddWithValue("@Rdate", DateTime.Now);

                MyConn2.Open();
                com.ExecuteNonQuery();
                MyConn2.Close();
                TempData["message"] = "Thank you for the Feedback!";
                
                return RedirectToAction("Index", "TSODashboard");
               
            }
            else
            {
                return View(feedback);
            }

        }

        [HttpGet]
        public ActionResult feedbackList()
        {
            String userid = Session["UserID"].ToString();
            string userlevel= Session["UserLevel"].ToString();
            var feedbackList = ListTSO.GetFeedbackList(userid, userlevel);
            return View(feedbackList);

        }


        [HttpGet]
        public ActionResult Help()
        {
            String userid = Session["UserID"].ToString();
            var helpList = ListTSO.GetHelpList(userid);
            return View(helpList);

        }
        public ActionResult DownloadFile(string fileName,HttpPostedFileBase file)
        {
            
            //Build the File Path.
            string path = Server.MapPath("~/Files/") + fileName;
            if (System.IO.File.Exists(path))
            {
                //Read the File data into Byte Array.
                byte[] bytes = System.IO.File.ReadAllBytes(path);
            

                //Send the File to Download.
                return File(bytes, "application/octet-stream", fileName);
            }
            else
            {
                //return Content("<script language='javascript' type='text/javascript'>alert('File Does Not Exist!');</script>");
                TempData["Message"] = "File Does Not Exist!";
                return RedirectToAction("Help");
            }
        }
        [HttpGet]
        public JsonResult getRemarks(string vizoption)
        {
            var remarks = ListTSO.GetRemarks(vizoption);
            return Json(remarks, JsonRequestBehavior.AllowGet);

        }

       
       
        public ActionResult Index_Post()
        {
            //Fill dataset with records
            DataSet dataSet = GetRecordsFromDatabase();

            StringBuilder sb = new StringBuilder();

            sb.Append("<table>");

            //LINQ to get Column names
            var columnName = dataSet.Tables[0].Columns.Cast<DataColumn>()
                                 .Select(x => x.ColumnName)
                                 .ToArray();
            sb.Append("<tr>");
            //Looping through the column names
            foreach (var col in columnName)
                sb.Append("<td>" + col + "</td>");
            sb.Append("</tr>");

            //Looping through the records
            foreach (DataRow dr in dataSet.Tables[0].Rows)
            {
                sb.Append("<tr>");
                foreach (DataColumn dc in dataSet.Tables[0].Columns)
                {
                    sb.Append("<td>" + dr[dc] + "</td>");
                }
                sb.Append("</tr>");
            }

            sb.Append("</table>");

            //Writing StringBuilder content to an excel file.
            Response.Clear();
            Response.ClearContent();
            Response.ClearHeaders();
            Response.Charset = "";
            Response.Buffer = true;
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("content-disposition", "attachment;filename=UserDetailReport.xls");
            Response.Write(sb.ToString());
            Response.Flush();
            Response.Close();

            return View();
        }

        DataSet GetRecordsFromDatabase()
        {
            DataSet dataSet = new DataSet();

            MySqlConnection conn = new MySqlConnection();
            string constr = ConfigurationManager.ConnectionStrings["Nerolacconstr"].ConnectionString;
            MySqlConnection MyConn2 = new MySqlConnection(constr);
            MySqlCommand cmd = new MySqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "Select ID,Name,Status,ProjectCount,LastProjectID,LastProjectDate,Designation,EMailID,ContactNo,Region,DepotCode,DepotName," +
                "`ASM ID`,`ASM Name`,`ASM Contact` FROM active_inactive_users";
            cmd.Connection = MyConn2;

            MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter();
            sqlDataAdapter.SelectCommand = cmd;
            sqlDataAdapter.Fill(dataSet);

            return dataSet;
        }
        

        public ActionResult ColourPalette(string sortOrder, string q, int page = 1, int pageSize = 1000)
        {
            ViewBag.searchQuery = String.IsNullOrEmpty(q) ? "" : q;

            page = page > 0 ? page : 1;
            pageSize = pageSize > 0 ? pageSize : 1000;

            ViewBag.GroupSortParam = sortOrder == "Group" ? "Group_desc" : "Group";
            ViewBag.CodeSortParam = sortOrder == "Code" ? "Code_desc" : "Code";
           
            



            ViewBag.CurrentSort = sortOrder;
            string constr = ConfigurationManager.ConnectionStrings["Nerolacconstr"].ConnectionString;

            using (MySqlConnection con = new MySqlConnection(constr))
            {
                DataSet ds = new DataSet();
                MySqlCommand com = new MySqlCommand("select RSN,ColorFamily,ShadeCode,ShadeName,concat('#',red_h,green_h,blue_h) as hexacode from ancptable order by ColorFamily,ShadeCode ASC", con);

                com.CommandTimeout = 1000;

                con.Open();

                List<ColourPalette> lstColourPalette = new List<ColourPalette>();
                var ColourPalette = new List<ColourPalette>();

                using (MySqlDataReader sdr = com.ExecuteReader())
                {
                    while (sdr.Read())
                    {

                        ColourPalette.Add(new ColourPalette
                        {
                             RSN= String.IsNullOrEmpty(sdr["RSN"].ToString()) ? "-" : sdr["RSN"].ToString(),
                            Group = String.IsNullOrEmpty(sdr["ColorFamily"].ToString()) ? "-" : sdr["ColorFamily"].ToString(),
                            Code = String.IsNullOrEmpty(sdr["ShadeCode"].ToString()) ? "-" : sdr["ShadeCode"].ToString(),
                            Name = String.IsNullOrEmpty(sdr["ShadeName"].ToString()) ? "-" : sdr["ShadeName"].ToString(),
                            hexacode = String.IsNullOrEmpty(sdr["hexacode"].ToString()) ? "-" : sdr["hexacode"].ToString()


                        });
                    }
                }
                var list = ColourPalette.ToList();

                if (!String.IsNullOrEmpty(q))
                {
                    q = q.ToLower();
                    list = list.Where(s => s.Group.ToLower().Contains(q) || s.Code.ToLower().Contains(q) || s.Name.ToLower().Contains(q)).ToList();
                }
                switch (sortOrder)
                {

                    case "Group_desc":
                        list = list.OrderByDescending(x => x.Group).ToList();
                        break;
                    case "Group":
                        list = list.OrderBy(x => x.Group).ToList();
                        break;
                    case "Code_desc":
                        list = list.OrderByDescending(x => x.Code).ToList();
                        break;
                    case "Code":
                        list = list.OrderBy(x => x.Code).ToList();
                        break;


                    default:
                        break;
                }

                con.Close();
                return View(list.ToPagedList(page, pageSize));
            }
        }
        public ActionResult PreviousColourOptionss(string sortOrder, string q, int page = 1, int pageSize = 1000)
        {
            ViewBag.searchQuery = String.IsNullOrEmpty(q) ? "" : q;

            page = page > 0 ? page : 1;
            pageSize = pageSize > 0 ? pageSize : 1000;

            ViewBag.GroupSortParam = sortOrder == "Group" ? "Group_desc" : "Group";
            ViewBag.CodeSortParam = sortOrder == "Code" ? "Code_desc" : "Code";





            ViewBag.CurrentSort = sortOrder;
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

                List<PreviousColourOptions> lstColourPalette = new List<PreviousColourOptions>();
                var PreviousColourOptions = new List<PreviousColourOptions>();

                using (MySqlDataReader sdr = com.ExecuteReader())
                {
                    while (sdr.Read())
                    {

                        PreviousColourOptions.Add(new PreviousColourOptions
                        {
                            RSN = String.IsNullOrEmpty(sdr["RSN"].ToString()) ? "-" : sdr["RSN"].ToString(),
                             Projectid= String.IsNullOrEmpty(sdr["ProjectID"].ToString()) ? "-" : sdr["ProjectID"].ToString(),
                            Optionno = String.IsNullOrEmpty(sdr["optionno"].ToString()) ? "-" : sdr["optionno"].ToString(),
                            code1 = String.IsNullOrEmpty(sdr["Shadecode1"].ToString()) ? "-" : sdr["Shadecode1"].ToString(),
                            Name1 = String.IsNullOrEmpty(sdr["shadename1"].ToString()) ? "-" : sdr["shadename1"].ToString(),
                            hcode1= String.IsNullOrEmpty(sdr["hcode1"].ToString()) ? "-" : sdr["hcode1"].ToString(),
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

                //if (!String.IsNullOrEmpty(q))
                //{
                //    q = q.ToLower();
                //    list = list.Where(s => s.Group.ToLower().Contains(q) || s.Code.ToLower().Contains(q) || s.Name.ToLower().Contains(q)).ToList();
                //}
                //switch (sortOrder)
                //{

                //    case "Group_desc":
                //        list = list.OrderByDescending(x => x.Group).ToList();
                //        break;
                //    case "Group":
                //        list = list.OrderBy(x => x.Group).ToList();
                //        break;
                //    case "Code_desc":
                //        list = list.OrderByDescending(x => x.Code).ToList();
                //        break;
                //    case "Code":
                //        list = list.OrderBy(x => x.Code).ToList();
                //        break;


                //    default:
                //        break;
                //}

                con.Close();
                return View(list.ToPagedList(page, pageSize));
            }
        }
        public ActionResult RecommendedCombinations(string sortOrder, string q, int page = 1, int pageSize = 1000)
        {
            ViewBag.searchQuery = String.IsNullOrEmpty(q) ? "" : q;

            page = page > 0 ? page : 1;
            pageSize = pageSize > 0 ? pageSize : 1000;

            //ViewBag.GroupSortParam = sortOrder == "Group" ? "Group_desc" : "Group";
            //ViewBag.CodeSortParam = sortOrder == "Code" ? "Code_desc" : "Code";





            ViewBag.CurrentSort = sortOrder;
            string constr = ConfigurationManager.ConnectionStrings["Nerolacconstr"].ConnectionString;

            using (MySqlConnection con = new MySqlConnection(constr))
            {
                DataSet ds = new DataSet();
                MySqlCommand com = new MySqlCommand("select RSN,combinationcode,shadecode,shadename,family,concat('#',Red_H,Green_H,Blue_H) as colorcode,ProductType from recommendedcombinations order by combinationcode", con);
                MySqlDataAdapter dacc = new MySqlDataAdapter("select distinct(combinationcode) as ptype from recommendedcombinations ", con);
                DataSet dscc = new DataSet();
                dacc.Fill(dscc);
                //if (Convert.ToInt32(dscc.Tables[0].Rows[0][0]) > 0)
                //{
                // Session["ptype"] = dscc.Tables[0].Rows[0]["ptype"].ToString(); ;
                //}
                com.CommandTimeout = 1000;

                con.Open();

                List<recommendedcombinations> lstrecommendedcombinations = new List<recommendedcombinations>();
                var recommendedcombinations = new List<recommendedcombinations>();

                using (MySqlDataReader sdr = com.ExecuteReader())
                {
                    while (sdr.Read())
                    {

                        recommendedcombinations.Add(new recommendedcombinations
                        {
                            RSN = String.IsNullOrEmpty(sdr["RSN"].ToString()) ? "-" : sdr["RSN"].ToString(),
                            combinationcode = String.IsNullOrEmpty(sdr["combinationcode"].ToString()) ? "-" : sdr["combinationcode"].ToString(),
                            shadecode = String.IsNullOrEmpty(sdr["shadecode"].ToString()) ? "-" : sdr["ShadeCode"].ToString(),
                            shadename = String.IsNullOrEmpty(sdr["ShadeName"].ToString()) ? "-" : sdr["ShadeName"].ToString(),
                            family = String.IsNullOrEmpty(sdr["family"].ToString()) ? "-" : sdr["family"].ToString(),
                            colorcode = String.IsNullOrEmpty(sdr["colorcode"].ToString()) ? "-" : sdr["colorcode"].ToString(),
                            ProductType = String.IsNullOrEmpty(sdr["ProductType"].ToString()) ? "-" : sdr["ProductType"].ToString(),
                            ptype = dscc.Tables[0].Rows[0]["ptype"].ToString()

                        });
                    }
                }
                var list = recommendedcombinations.ToList();

                ////if (!String.IsNullOrEmpty(q))
                ////{
                ////    q = q.ToLower();
                ////    list = list.Where(s => s.Group.ToLower().Contains(q) || s.Code.ToLower().Contains(q) || s.Name.ToLower().Contains(q)).ToList();
                ////}
                ////switch (sortOrder)
                ////{

                ////    case "Group_desc":
                ////        list = list.OrderByDescending(x => x.Group).ToList();
                ////        break;
                ////    case "Group":
                ////        list = list.OrderBy(x => x.Group).ToList();
                ////        break;
                ////    case "Code_desc":
                ////        list = list.OrderByDescending(x => x.Code).ToList();
                ////        break;
                ////    case "Code":
                ////        list = list.OrderBy(x => x.Code).ToList();
                ////        break;


                ////    default:
                ////        break;
                ////}

                con.Close();
                return View(list.ToPagedList(page, pageSize));
            }
        }
        public JsonResult RecommendedCombinations1(string q)
        {
            

           
            string constr = ConfigurationManager.ConnectionStrings["Nerolacconstr"].ConnectionString;

            using (MySqlConnection con = new MySqlConnection(constr))
            {
                DataSet ds = new DataSet();
                MySqlCommand com = new MySqlCommand("select RSN,combinationcode,shadecode,shadename,family,concat('#',Red_H,Green_H,Blue_H) as colorcode,ProductType from recommendedcombinations where ProductType='"+q+"' order by combinationcode", con);
                MySqlDataAdapter dacc = new MySqlDataAdapter("select distinct(producttype) as ptype from recommendedcombinations ", con);
                DataSet dscc = new DataSet();
                dacc.Fill(dscc);
                //if (Convert.ToInt32(dscc.Tables[0].Rows[0][0]) > 0)
                //{
                // Session["ptype"] = dscc.Tables[0].Rows[0]["ptype"].ToString(); ;
                //}
                com.CommandTimeout = 1000;

                con.Open();

                List<recommendedcombinations> lstrecommendedcombinations = new List<recommendedcombinations>();
                var recommendedcombinations = new List<recommendedcombinations>();

                using (MySqlDataReader sdr = com.ExecuteReader())
                {
                    while (sdr.Read())
                    {

                        recommendedcombinations.Add(new recommendedcombinations
                        {
                            RSN = String.IsNullOrEmpty(sdr["RSN"].ToString()) ? "-" : sdr["RSN"].ToString(),
                            combinationcode = String.IsNullOrEmpty(sdr["combinationcode"].ToString()) ? "-" : sdr["combinationcode"].ToString(),
                            shadecode = String.IsNullOrEmpty(sdr["shadecode"].ToString()) ? "-" : sdr["ShadeCode"].ToString(),
                            shadename = String.IsNullOrEmpty(sdr["ShadeName"].ToString()) ? "-" : sdr["ShadeName"].ToString(),
                            family = String.IsNullOrEmpty(sdr["family"].ToString()) ? "-" : sdr["family"].ToString(),
                            colorcode = String.IsNullOrEmpty(sdr["colorcode"].ToString()) ? "-" : sdr["colorcode"].ToString(),
                            ProductType = String.IsNullOrEmpty(sdr["ProductType"].ToString()) ? "-" : sdr["ProductType"].ToString(),
                            ptype = dscc.Tables[0].Rows[0]["ptype"].ToString()

                        });
                    }
                }
                var list = recommendedcombinations.ToList();
                var model = new recommendedcombinations();



                con.Close();
                return Json(list, JsonRequestBehavior.AllowGet);
            }
        }


    }
}