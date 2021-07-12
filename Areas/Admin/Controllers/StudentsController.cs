using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ElectionManagementSystem.Areas.Admin.Controllers
{
    using ElectionManagementSystem.Models;

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
        [HttpPost]
        public ActionResult CreateStudent(ElectionManagementSystem.Student student)
        {
            string msg = "", DocumentNo = "";

            try
            {
                var settings = _db.Settings.Where(s => s.Id == 1).SingleOrDefault();

                string StudentCode = settings.StudentsSeriesCode;

                var NumberSeriesData = _db.NumberSeries.Where(s => s.Code == StudentCode).SingleOrDefault();

                string LastUsedNumber = NumberSeriesData.LastUsedNumber;

                if (LastUsedNumber != "")
                {
                    DocumentNo = AppFunctions.GetNewDocumentNumber(StudentCode.Trim(), LastUsedNumber.Trim());
                }

                var stud = new ElectionManagementSystem.Student
                {

                    StudentId = DocumentNo,
                    Email = student.Email,
                    Faculty = student.Faculty,
                    FirstName = student.FirstName,
                    LastName = student.LastName,
                    Gender = student.Gender,
                    Name = student.Name,
                    Phone = student.Phone,
                    YearOfStudy = student.YearOfStudy

                };

                using (ElectionManagementSystemEntities dbEntities = new ElectionManagementSystemEntities())
                {
                    dbEntities.Configuration.ValidateOnSaveEnabled = false;
                    dbEntities.Students.Add(stud);
                    dbEntities.SaveChanges();

                    msg = "Student Created successfully";
                }

                //update last used number
                AppFunctions.UpdateNumberSeries(StudentCode, DocumentNo);
            }
            catch (Exception es)
            {
                msg = es.Message;
            }
            var _RequestResponse = new Models.RequestResponse
            {
                Message = msg,

                Status = "000"
            };

            return Json(JsonConvert.SerializeObject(_RequestResponse), JsonRequestBehavior.AllowGet);
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
        public ActionResult Delete(string DocumentNo)
        {
            string status = "", message = "";

            try
            {
                using (var db = new ElectionManagementSystemEntities())
                {
                    var student = db.Students.Where(x => x.StudentId == DocumentNo).SingleOrDefault();

                    if (student != null)
                    {
                        db.Students.Remove(student);
                        db.SaveChanges();
                        status = "000";
                        message = "Delete Success! for student " + DocumentNo;
                    }
                    else
                    {
                        status = "900";
                        message = "Couldn't find student " + DocumentNo;
                    }
                }
            }
            catch (Exception es)
            {
                message = es.Message;
            }

            var _RequestResponse = new RequestResponse
            {
                Status = status,
                Message = message
            };

            return Json(JsonConvert.SerializeObject(_RequestResponse), JsonRequestBehavior.AllowGet);
        }
    }
}