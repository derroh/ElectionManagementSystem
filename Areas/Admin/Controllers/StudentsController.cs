using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ElectionManagementSystem.Areas.Admin.Controllers
{
    [Authorize]
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
        public JsonResult ListStudents()
        {
            List<ElectionManagementSystem.Student> studentlist = new List<ElectionManagementSystem.Student>();

            using (ElectionManagementSystemEntities dbEntities = new ElectionManagementSystemEntities())
            {
                var students = dbEntities.Students.ToList();

                foreach (var student in students)
                {
                    studentlist.Add(new ElectionManagementSystem.Student
                    {
                        Name = student.Name,
                        StudentId = student.StudentId
                    });
                }

            }

            return Json(JsonConvert.SerializeObject(studentlist), JsonRequestBehavior.AllowGet);
        }
    }
}