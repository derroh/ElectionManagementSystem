using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services;

namespace ElectionManagementSystem.Areas.Admin.Controllers
{
    using ElectionManagementSystem.Models;
    using ElectionManagementSystem.Areas.Admin.ViewModels;

    [Authorize]
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

        public ActionResult Edit(string Id)
        {
            var elections = _db.Elections.ToList();
            var positionlist = _db.ElectionPositions.ToList();
            var studentlist = _db.Students.ToList();

            ViewBag.Elections = elections;
            ViewBag.Positions = positionlist;
            ViewBag.Students = studentlist;
            
            var candidate = _db.ElectionCandidates.Where(s => s.CandidateId == Id).FirstOrDefault();

            var _ElectionCandidate = new CandidateViewModel
            {
                CandidateId = candidate.CandidateId,
                ElectionId = candidate.ElectionId,
                PositionId = candidate.PositionId,
                StudentId = candidate.StudentId
            };

            return View(_ElectionCandidate);
        }
        public ActionResult Candidate(string Id)
        {
            var elections = _db.Elections.ToList();
            var positionlist = _db.ElectionPositions.ToList();

            //
            var notFoundItems = _db.Students.Where(c => !_db.ElectionCandidates.Any(x => x.StudentId == c.StudentId)).ToList();

            var studentlist = _db.Students.ToList();

            ViewBag.Elections = elections;
            ViewBag.Positions = positionlist;
            ViewBag.Students = studentlist;

            var candidate = _db.ElectionCandidates.Where(s => s.CandidateId == Id).FirstOrDefault();

            var _ElectionCandidate = new CandidateViewModel
            {
                CandidateId = candidate.CandidateId,
                ElectionId = candidate.ElectionId,
                PositionId = candidate.PositionId,
                StudentId = candidate.StudentId
            };

            return View(_ElectionCandidate);
        }
        public ActionResult Create()
        {
            var elections = _db.Elections.ToList();
            var positionlist = _db.ElectionPositions.ToList();
            var studentlist = _db.Students.ToList();

            ViewBag.Elections = elections;
            ViewBag.Positions = positionlist;
            ViewBag.Students = studentlist;

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateCandidate(CandidateViewModel ec)
        {
            string msg = "", DocumentNo = "";

            try
            {
                var settings = _db.Settings.Where(s => s.Id == 1).SingleOrDefault();

                string CandidatesCode = settings.CandidatesSeriesCode;

                var NumberSeriesData = _db.NumberSeries.Where(s => s.Code == CandidatesCode).SingleOrDefault();

                string LastUsedNumber = NumberSeriesData.LastUsedNumber;

                if (LastUsedNumber != "")
                {
                    DocumentNo = AppFunctions.GetNewDocumentNumber(CandidatesCode.Trim(), LastUsedNumber.Trim());
                }

                var candidate = new ElectionCandidate
                {
                    PositionId = ec.PositionId,
                    StudentId = ec.StudentId,
                    ElectionId = ec.ElectionId,
                    CandidateId = DocumentNo

                };

                using (ElectionManagementSystemEntities dbEntities = new ElectionManagementSystemEntities())
                {
                    dbEntities.Configuration.ValidateOnSaveEnabled = false;
                    dbEntities.ElectionCandidates.Add(candidate);
                    dbEntities.SaveChanges();

                    msg = "Candidate Created successfully";
                }

                //update last used number
                AppFunctions.UpdateNumberSeries(CandidatesCode, DocumentNo);
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
        public ActionResult Delete(string DocumentNo)
        {
            string status = "", message = "";
            
            try
            {
                using (var db = new ElectionManagementSystemEntities())
                {
                    var candidate = db.ElectionCandidates.Where(x => x.CandidateId == DocumentNo).SingleOrDefault();

                    if (candidate != null)
                    {
                        db.ElectionCandidates.Remove(candidate);
                        db.SaveChanges();
                        status = "000";
                        message = "Delete Success! for Candidate " + DocumentNo;
                    }
                    else
                    {                     
                        status = "900";
                        message = "Coouldn't find Candidate " + DocumentNo;
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateCandidate(CandidateViewModel ec)
        {
            string message = "", status = "";

            try
            {
               
                var candidate = _db.ElectionCandidates.Where(x => x.CandidateId == ec.CandidateId).SingleOrDefault();

                if (candidate != null)
                {
                    candidate.ElectionId = ec.ElectionId;
                    candidate.StudentId = ec.StudentId;
                    candidate.PositionId = ec.PositionId;
                    _db.SaveChanges();

                    message = "Candidate updated successfully";
                    status = "000";
                }
                else
                {
                    message = "Candidate not found";
                    status = "900";
                }


            }
            catch (Exception es)
            {
                message = es.Message;
            }

            var _RequestResponse = new Models.RequestResponse
            {
                Message = message,

                Status = status
            };

            return Json(JsonConvert.SerializeObject(_RequestResponse), JsonRequestBehavior.AllowGet);
        }
    }
}