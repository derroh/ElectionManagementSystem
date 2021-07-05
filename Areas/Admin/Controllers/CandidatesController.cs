using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services;

namespace ElectionManagementSystem.Areas.Admin.Controllers
{
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
    }
}