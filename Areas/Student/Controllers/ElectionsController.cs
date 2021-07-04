using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ElectionManagementSystem.Areas.Student.Controllers
{
    public class ElectionsController : Controller
    {
        // GET: Student/Elections
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }
    }
}