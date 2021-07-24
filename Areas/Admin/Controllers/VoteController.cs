using Microsoft.Reporting.WebForms;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace ElectionManagementSystem.Areas.Admin.Controllers
{
    [Authorize]
    public class VoteController : Controller
    {
        private ElectionManagementSystemEntities _db = new ElectionManagementSystemEntities();
        // GET: Admin/Vote
        // [Authorize]
        public ActionResult Index()
        {
            return View(from elections in _db.Elections.Where(e => e.Status == (int)ElectionStatus.Open).Take(10)
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
                var electionPositions = _db.ElectionPositions.Where(ep => ep.ElectionId == election.ElectionId).ToList();

                foreach (var item in electionPositions)
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

                        }                       
                    }

                    pob.Add(new Models.PositionsOnBallot
                    {
                        Candidates = candi.ToArray(),
                        PositionId = item.PositionId,
                        PositionSequence = item.Sequence,
                        PositionName = item.Name
                    });
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

              //  AppFunctions.SendTextMessage(phone, msg);
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

        //report
   
        public ActionResult ReportEmployee()
        {
            try
            {
                ReportViewer reportViewer = new ReportViewer();
                //C:\Users\Derrick\Documents\Visual Studio 2019\Projects\C#\ASP.NET\ElectionManagementSystem\Areas\Admin\Reports\VotesCasted.rdlc
                reportViewer.ProcessingMode = ProcessingMode.Local;
                reportViewer.SizeToReportContent = true;
                //reportViewer.Width = Unit.Percentage(900);
                //reportViewer.Height = Unit.Percentage(900);

                reportViewer.LocalReport.ReportPath = Server.MapPath("\\Reports\\VotesCasted.rdlc");
                ElectionManagementSystemEntities entities = new ElectionManagementSystemEntities();
                ReportDataSource datasource = new ReportDataSource("Students", (from students in entities.Students.Take(10)
                                                                                select students));
                reportViewer.LocalReport.DataSources.Clear();
                reportViewer.LocalReport.DataSources.Add(datasource);


                ViewBag.ReportViewer = reportViewer;
            }
            catch (Exception es)
            {
                es.ToString();
            }


            return View();
        }

        private string GetCandidateName(string studentId)
        {
            var check = _db.Students.FirstOrDefault(s => s.StudentId == studentId);
            
            return check.Name;
        }
    }
}