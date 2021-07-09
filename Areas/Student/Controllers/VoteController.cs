using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ElectionManagementSystem.Areas.Student.Controllers
{
    public class VoteController : Controller
    {
        // GET: Student/Vote
        public ActionResult Index()
        {
            return View();
        }
    }
}