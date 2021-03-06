using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ElectionManagementSystem.Areas.Admin.Controllers
{
    using ElectionManagementSystem.Models;
    using ElectionManagementSystem.Areas.Admin.ViewModels;

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
            var Facultieslist = _db.Faculties.Take(10).ToList();
            ViewBag.Faculties = Facultieslist;
            return View();
        }
        public ActionResult Edit(string Id)
        {
            var Facultieslist = _db.Faculties.Take(10).ToList();
            ViewBag.Faculties = Facultieslist;

            ///fetch student by their ID
            ///
            var stud = _db.Students.Where(s => s.StudentId == Id).FirstOrDefault();

            var student = new ViewModels.StudentViewModel { Name = stud.Name, LastName = stud.LastName, FirstName = stud.FirstName, Phone = stud.Phone, YearOfStudy = (YearOfStudy)(Convert.ToInt32(stud.YearOfStudy)), Email = stud.Email,  Faculty = stud.Faculty, Gender = stud.Gender.Trim(), StudentId = stud .StudentId};

            return View(student);
        }
        public ActionResult ViewStudent(string Id)
        {
            var Facultieslist = _db.Faculties.Take(10).ToList();
            ViewBag.Faculties = Facultieslist;

            ///fetch student by their ID
            ///
            var stud = _db.Students.Where(s => s.StudentId == Id).FirstOrDefault();

            var student = new ViewModels.StudentViewModel { Name = stud.Name, LastName = stud.LastName, FirstName = stud.FirstName, Phone = stud.Phone, YearOfStudy = (YearOfStudy)(Convert.ToInt32(stud.YearOfStudy)), Email = stud.Email, Faculty = stud.Faculty, Gender = stud.Gender.Trim(), StudentId = stud.StudentId };

            return View(student);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateStudent(StudentViewModel student)
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
                    YearOfStudy =((int)student.YearOfStudy).ToString()

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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateStudent(StudentViewModel student)
        {
            string message = "", status = "";

            try
            {
                var studentrec = _db.Students.Where(s => s.StudentId == student.StudentId).SingleOrDefault();

                if(studentrec != null)
                {

                    studentrec.Email = student.Email;
                    studentrec.Faculty = student.Faculty;
                    studentrec.FirstName = student.FirstName;
                    studentrec.LastName = student.LastName;
                    studentrec.Gender = student.Gender.Trim();
                    studentrec.Name = student.Name;
                    studentrec.Phone = student.Phone;
                    studentrec.YearOfStudy = ((int)student.YearOfStudy).ToString();
                    _db.SaveChanges();
                    message = "Student updated successfully";
                    status = "000";
                }
                else
                {
                    message = "Student not found";
                    status = "900";
                }
            }
            catch (Exception es)
            {
                message = es.Message;
                status = "900";
            }
            var _RequestResponse = new Models.RequestResponse
            {
                Message = message,

                Status = status
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