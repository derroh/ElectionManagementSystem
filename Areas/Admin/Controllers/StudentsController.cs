using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ElectionManagementSystem.Areas.Admin.Controllers
{
    public class StudentsController : Controller
    {
        private ElectionManagementSystemEntities _db = new ElectionManagementSystemEntities();
        // GET: Admin/Students
        // [Authorize]
        public ActionResult Index()
        {
            return View(from students in _db.Students.Take(10)
                        select students);
        }
        public ActionResult Create()
        {
            return View();
        }
    }
}