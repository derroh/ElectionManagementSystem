using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ElectionManagementSystem.Controllers
{
    public class ErrorController : Controller
    {
        // GET: Error
        public ActionResult Index()
        {
            return View();
        }
       // [ActionName("404")]
        public ActionResult PageNotFound()
        {
            if (this.Request.IsAuthenticated)
            {
                return RedirectToAction("PageNotFound", "Error", new { area = "Admin" });
               
            }
            else
            {
                return View("Error");
            }
            
        }

        [ActionName("500")]
        public ActionResult ServerError()
        {
            return View();
        }
    }
}