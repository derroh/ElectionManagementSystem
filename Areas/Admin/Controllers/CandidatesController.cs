using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services;

namespace ElectionManagementSystem.Areas.Admin.Controllers
{
    public class CandidatesController : Controller
    {
        private static ElectionManagementSystemEntities _db = new ElectionManagementSystemEntities();
        // GET: Admin/Candidates        
        public ActionResult Index()
        {
            //return View(from candidate in _db.ElectionCandidates.Take(10)
            //        select candidate); // returns list as in table

            List<ElectionCandidate> electionCandidate = _db.ElectionCandidates.ToList();
            List<ElectionManagementSystem.Student> student = _db.Students.ToList();
            List <ElectionPosition> electionPosition = _db.ElectionPositions.ToList();

            var candidate = from ec in electionCandidate

                            join s in student on ec.StudentId equals s.StudentId into table1
                            from s in table1.ToList()

                            join ep in electionPosition on ec.PositionId equals ep.PositionId into table2
                            from ep in table2.ToList()

                            select new Models.ElectionCandidateView
                            {
                                electionCandidate = ec,
                                electionPosition = ep,
                                student = s
                            };

            return View(candidate);
        }

        public ActionResult Edit()
        {
            return View();
        }
        public ActionResult Create()
        {
            return View();
        }
        public ActionResult CreateCandidate(ElectionCandidate ec)
        {
            string msg = "";

            try
            {
                var candidate = new ElectionCandidate
                {
                    PositionId = ec.PositionId,
                    StudentId = ec.StudentId,
                    CandidateId = "4"
                    
                };

                using (ElectionManagementSystemEntities dbEntities = new ElectionManagementSystemEntities())
                {
                    dbEntities.Configuration.ValidateOnSaveEnabled = false;
                    dbEntities.ElectionCandidates.Add(candidate);
                    dbEntities.SaveChanges();

                    msg = "Candidate Created successfully";
                }
            }
            catch(Exception es)
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
        public JsonResult ListPositions()
        {
            List<ElectionPosition> positionlist = new List<ElectionPosition>();            

            using (ElectionManagementSystemEntities dbEntities = new ElectionManagementSystemEntities())
            {
                var positions = dbEntities.ElectionPositions.ToList();

                foreach (var position in positions)
                {
                    positionlist.Add(new ElectionPosition
                    {
                        Name = position.Name,
                        PositionId = position.PositionId
                    });
                }
            }
            return Json(JsonConvert.SerializeObject(positionlist), JsonRequestBehavior.AllowGet);
        }

    }
}