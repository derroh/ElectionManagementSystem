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
    using System.Dynamic;
    using ElectionManagementSystem;

    [Authorize]
    public class ElectionsController : Controller
    {
        private static ElectionManagementSystemEntities _db = new ElectionManagementSystemEntities();
        // GET: Admin/Elections
        // [Authorize]
        public ActionResult Index()
        {
            return View(from elections in _db.Elections.Take(10)
                        select elections);
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateElection(ElectionViewModel e)
        {
            string msg = "", DocumentNo = "";

            try
            {
                var settings = _db.Settings.Where(s => s.Id == 1).SingleOrDefault();

                string ElectionsCode = settings.ElectionsSeriesCode;

                var NumberSeriesData = _db.NumberSeries.Where(s => s.Code == ElectionsCode).SingleOrDefault();

                string LastUsedNumber = NumberSeriesData.LastUsedNumber;

                if (LastUsedNumber != "")
                {
                    DocumentNo = AppFunctions.GetNewDocumentNumber(ElectionsCode.Trim(), LastUsedNumber.Trim());
                }

                var election = new Election
                {
                    ElectionId = DocumentNo,
                    Name = e.Name,
                    StartDate = Convert.ToDateTime(e.StartDate),
                    EndDate = Convert.ToDateTime(e.EndDate),
                    Status = (int)ElectionStatus.Created

                };

                using (ElectionManagementSystemEntities dbEntities = new ElectionManagementSystemEntities())
                {
                    dbEntities.Configuration.ValidateOnSaveEnabled = false;
                    dbEntities.Elections.Add(election);
                    dbEntities.SaveChanges();

                    msg = "Election Created successfully";
                }

                //update last used number
                AppFunctions.UpdateNumberSeries(ElectionsCode, DocumentNo);
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

        public ActionResult Edit(string Id)
        {
            var election = _db.Elections.Where(x => x.ElectionId == Id).SingleOrDefault();
            var ElectionViewModel = new ElectionViewModel
            {
                Name = election.Name,
                ElectionId = election.ElectionId,
                StartDate = election.StartDate.ToString(),
                EndDate = election.EndDate.ToString()

            };
            return View(ElectionViewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateElection(ElectionViewModel ep)
        {
            string message = "", status = "";

            try
            {
                var election = _db.Elections.Where(x => x.ElectionId == ep.ElectionId).SingleOrDefault();

                if (election != null)
                {
                    election.Name = ep.Name;
                    election.ElectionId = ep.ElectionId;
                    election.StartDate = Convert.ToDateTime(ep.StartDate);
                    election.EndDate = Convert.ToDateTime(ep.EndDate);
                    _db.SaveChanges();

                    message = "Election updated successfully";
                    status = "000";
                }


            }
            catch (Exception es)
            {
                message = es.Message;
            }

            var _RequestResponse = new RequestResponse
            {
                Message = message,

                Status = status
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
                    var election = db.Elections.Where(x => x.ElectionId == DocumentNo).SingleOrDefault();

                    if (election != null)
                    {
                        db.Elections.Remove(election);
                        db.SaveChanges();
                        status = "000";
                        message = "Delete Success! for election " + DocumentNo;
                    }
                    else
                    {
                        status = "900";
                        message = "Couldn't find election " + DocumentNo;
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
        public ActionResult ViewElection(string Id)
        {
            var election = _db.Elections.Where(x => x.ElectionId == Id).SingleOrDefault();
            var ElectionViewModel = new ElectionViewModel
            {
                Name = election.Name,
                ElectionId = election.ElectionId,
                StartDate = election.StartDate.ToString(),
                EndDate = election.EndDate.ToString()

            };
            return View(ElectionViewModel);
        }
        public ActionResult Design(string Id)
        {
            List<Models.PositionsOnBallot> pob = new List<Models.PositionsOnBallot>();
            List<Models.Candidates> candi = new List<Models.Candidates>();

            var election = _db.Elections.Where(e => e.ElectionId == Id).SingleOrDefault();

            if (election != null)
            {
                int items = _db.ElectionPositions.Where(ep => ep.ElectionId == election.ElectionId).ToList().Count;

                foreach (var item in _db.ElectionPositions.Where(ep => ep.ElectionId == election.ElectionId).ToList())
                {
                    foreach (var candidate in _db.ElectionCandidates.Where(e => e.ElectionId == election.ElectionId && e.PositionId == item.PositionId).ToList())
                    {
                        if (candidate.PositionId == item.PositionId)
                        {
                            candi.Add(new Models.Candidates
                            {
                                CandidateID = candidate.CandidateId,
                                CandidateName = GetCandidateName(candidate.StudentId), //change this, add Foreign key rshps
                                CandidateStudentId = candidate.StudentId,
                                ElectralPositionId = candidate.PositionId
                            });

                            pob.Add(new Models.PositionsOnBallot
                            {
                                Candidates = candi.ToArray(),
                                PositionId = item.PositionId,
                                PositionSequence = item.Sequence,
                                PositionName = item.Name
                            });
                        }
                    }
                }
            }



            Models.Vote v = new Models.Vote
            {
                ElectionID = election.ElectionId,
                StartDate = election.StartDate.ToString(),
                EndDate = election.EndDate.ToString(),
                Positions = pob.Count(),
                PositionsOnBallot = pob.ToArray()
            };

            dynamic mymodel = new ExpandoObject();
            mymodel.Vote = v;
            mymodel.CandidatesCount = candi.Count;
            mymodel.VoterTabs = pob.Count + 1;
            return View(mymodel);
        }

        public ActionResult Open(string ElectionId)
        {
            string status = "", message = "";

            try
            {
                using (var db = new ElectionManagementSystemEntities())
                {
                    var election = db.Elections.Where(x => x.ElectionId == ElectionId).SingleOrDefault();

                    if (election != null)
                    {
                        election.Status = (int)ElectionStatus.Open;
                        db.SaveChanges();
                        status = "000";
                        message = "Open Success! for election " + election.Name;
                    }
                    else
                    {
                        status = "900";
                        message = "Couldn't find election " + ElectionId;
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
        private string GetCandidateName(string studentId)
        {
            var check = _db.Students.FirstOrDefault(s => s.StudentId == studentId);

            return check.Name;
        }

    }
}