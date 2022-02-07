using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NerolacPreviewApp.Controllers
{
    public class ErrorController : Controller
    {
        // GET: Error
        public ViewResult Error()
        {
            return View("Error");
        }
        //public ViewResult NotFound()
        //{
        //    Response.StatusCode = 404;  //you may want to set this to 200
        //    return View("NotFound");
        //}
    }
}