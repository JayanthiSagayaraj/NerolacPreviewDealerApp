using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Drawing;

namespace NerolacPreviewApp.Controllers
{
    public class DisplayImageController : Controller
    {
        // GET: DisplayImage
      
        public ActionResult Index(string image,string i1,int pid1,string pname1,string imagename,string hlptxt,HttpPostedFileBase file)
        {
            //if (i1 != null)
            //{
                if (System.IO.File.Exists(image))
                {
                    string imageformat = Path.GetExtension(i1);
                    string str = imageformat.Replace(".", String.Empty);
                    imageformat = "image/" + str;
                    var luceneDir = new DirectoryInfo("~/TempImage/");
                    String physicalPath = Server.MapPath("~/TempImage/" + i1);
                    var filename = Path.GetFileName(i1);
                    System.IO.File.Delete(physicalPath);
                    System.IO.File.Copy(image,physicalPath);
                    luceneDir.Refresh();
                    ViewBag.image=i1;
                    ViewBag.pid = pid1;
                    ViewBag.pname = pname1;
                    ViewBag.imagename = imagename;
                    ViewBag.hpltxt = hlptxt;
                    return View();
                }
                else
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('File Does Not Exist!');</script>");

                }
            //}
            //else
            //{
            //    return Content("<script language='javascript' type='text/javascript'>alert('Sorry. It seems you have not submitted multiple images');</script>");
                
            //}
                    
        }
        //public ActionResult sis(string image, string i1,string ws)
        //{
        //    if (ws!="00")
        //    {
        //        string imageformat = Path.GetExtension(i1);
        //        string str = imageformat.Replace(".", String.Empty);
        //        imageformat = "image/" + str;
        //        return File(image, imageformat);
        //    }
        //    else
        //    {
        //        return Content("<script language='javascript' type='text/javascript'>alert('Please wait! The submitted image(s) are yet to be qualified!');</script>");

        //    }

        //}

    }
}