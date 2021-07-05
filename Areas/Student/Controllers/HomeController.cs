using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ElectionManagementSystem.Areas.Student.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        [Authorize(Roles = "Student")]
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult LogOff()
        {
            try
            {
                // Setting.
                var ctx = Request.GetOwinContext();
                var authenticationManager = ctx.Authentication;

                // Sign Out.
                authenticationManager.SignOut();
            }
            catch (Exception ex)
            {
                // Info
                throw ex;
            }

            // Info.
            return Redirect("~/");
        }
    }
}