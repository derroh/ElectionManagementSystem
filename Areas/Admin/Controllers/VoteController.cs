using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ElectionManagementSystem.Areas.Admin.Controllers
{
    public class VoteController : Controller
    {
        private ElectionManagementSystemEntities _db = new ElectionManagementSystemEntities();
        // GET: Admin/Vote
        // [Authorize]
        public ActionResult Index()
        {
            return View(from elections in _db.Elections.Take(10)
                        select elections);
        }
        public ActionResult CastVote(string Id)
        {
            List<Models.PositionsOnBallot> pob = new List<Models.PositionsOnBallot>();
            List<Models.Candidates> candi = new List<Models.Candidates>();

            var election = _db.Elections.Where(e => e.ElectionId == Id).SingleOrDefault();

            if(election != null)
            {
                int items = _db.ElectionPositions.Where(ep => ep.ElectionId == election.ElectionId).ToList().Count;
             
                foreach (var item in _db.ElectionPositions.Where(ep => ep.ElectionId == election.ElectionId).ToList())
                {
                    foreach (var candidate in _db.ElectionCandidates.Where(e => e.ElectionId == election.ElectionId && e.PositionId == item.PositionId).ToList())
                    {
                        if(candidate.PositionId == item.PositionId)
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
        public ActionResult CastBallot(Models.NameValue[] formVars)
        {
            string msg = "";
            string studentId = "1";

            List<Models.Ballot> votes = new List<Models.Ballot>();
            Models.Ballot ballot;

            foreach (Models.NameValue nv in formVars)
            {
                string Position = "";
                string ChoiceSelected = "";

                Position = nv.name;
                ChoiceSelected = nv.value;

                ballot = new Models.Ballot
                {
                    ChoiceSelected = ChoiceSelected,
                    Position = Position
                };

                votes.Add(ballot);
            }


            try
            {
                string responses = JsonConvert.SerializeObject(votes, Newtonsoft.Json.Formatting.Indented);
                //loop
                foreach(var vote in votes)
                {
                    //save vote
                    var ballotvote = new Ballot
                    {
                      // BallotId = 1,
                       ElectionId = "1",
                       PositionId = vote.Position,
                       CandidateId = vote.ChoiceSelected,
                       StudentId = studentId, //Session["StudentId"].ToString(),
                       Vote =1
                    };

                    using (ElectionManagementSystemEntities dbEntities = new ElectionManagementSystemEntities())
                    {
                        dbEntities.Configuration.ValidateOnSaveEnabled = false;
                        dbEntities.Database.ExecuteSqlCommand("SET IDENTITY_INSERT [dbo].[Ballots] ON");
                        dbEntities.Ballots.Add(ballotvote);
                        dbEntities.SaveChanges();

                        msg = "Votes casted successfully";
                    }
                }


                //notify 
                var students = _db.Students.Where(s => s.StudentId == studentId).SingleOrDefault();

                string phone = students.Phone;

                AppFunctions.SendTextMessage(phone, msg);
            }
            catch (Exception es)
            {
                msg = es.ToString();
            }

            var _RequestResponse = new Models.RequestResponse
            {
                Message = msg,

                Status = "000"
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