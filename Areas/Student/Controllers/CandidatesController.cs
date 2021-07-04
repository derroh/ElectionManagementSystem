using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ElectionManagementSystem.Areas.Student.Controllers
{
    [Authorize]
    public class CandidatesController : Controller
    {
        // GET: Student/Candidates
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }
    }
}