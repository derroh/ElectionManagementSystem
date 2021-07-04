using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ElectionManagementSystem.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        private ElectionManagementSystemEntities _db = new ElectionManagementSystemEntities();
        // GET: Admin/Home
        // [Authorize]
        public ActionResult Index()
        {
            var Positions = _db.ElectionPositions.ToList();
            var Candidates = _db.ElectionCandidates.ToList();
            var Students = _db.Students.ToList();
            var Ballots = _db.Ballots.ToList();

            dynamic mymodel = new ExpandoObject();
            mymodel.Positions = Positions.Count;
            mymodel.Candidates = Candidates.Count;
            mymodel.RegisteredVoters = Students.Count;
            mymodel.VotesCast = Ballots.Count;
            return View(mymodel);
        }
        //[Authorize]
        public ActionResult Logout()
        {
            Session.Clear();//remove session
            return Redirect("~/");
        }
    }
}