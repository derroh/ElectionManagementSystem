using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Web;
using System.Web.Mvc;

namespace ElectionManagementSystem.Areas.Admin.Controllers
{
    using ElectionManagementSystem.Models;
    using ElectionManagementSystem.Areas.Admin.ViewModels;
    [Authorize]
    public class PositionsController : Controller
    {
        private ElectionManagementSystemEntities _db = new ElectionManagementSystemEntities();
        // GET: Admin/Positions
        //[Authorize]
        public ActionResult Index()
        {
            List<ElectionPositionListViewModel> positionslist = new List <ElectionPositionListViewModel>();

            using (ElectionManagementSystemEntities dbEntities = new ElectionManagementSystemEntities())
            {
                var positions = dbEntities.ElectionPositions.Take(10).OrderBy(ep => ep.Sequence).ToList();

                foreach (var position in positions)
                {
                    positionslist.Add(new ElectionPositionListViewModel
                    {
                        Name = position.Name,
                        Sequence =  AppFunctions.AddOrdinal(position.Sequence),
                        ElectionId = position.ElectionId,
                        PositionId = position.PositionId

                    });
                }
            }

            return View(positionslist);
        }        
        public ActionResult Create(ElectionPositionViewModel ep)
        {
            List<Election> electionlist = new List<Election>();

            using (ElectionManagementSystemEntities dbEntities = new ElectionManagementSystemEntities())
            {
                var elections = dbEntities.Elections.ToList();

                foreach (var election in elections)
                {
                    electionlist.Add(new ElectionManagementSystem.Election
                    {
                        Name = election.Name,
                        ElectionId = election.ElectionId,
                        EndDate = election.EndDate,
                        StartDate = election.StartDate

                    });
                }
            }
            ViewBag.Elections = electionlist;

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreatePosition(ElectionPositionViewModel ep)
        {
            string message = "", status = "";
            string DocumentNo = null;

            try
            {

                if (_db.ElectionPositions.Any(o => o.Name == ep.Name))
                {
                    message = "A position with similar name exists";
                    status = "900";
                }
                else if (_db.ElectionPositions.Any(o => o.Sequence == ep.Sequence))
                {
                    message = "A position with similar sequence exists";
                    status = "900";
                }
                else
                {
                    //Get No Series here

                    var settings = _db.Settings.Where(s => s.Id == 1).SingleOrDefault();

                    string PositionsCode = settings.PositionsSeriesCode;

                    var NumberSeriesData = _db.NumberSeries.Where(s => s.Code == PositionsCode).SingleOrDefault();

                    string LastUsedNumber = NumberSeriesData.LastUsedNumber;

                    if (LastUsedNumber != "")
                    {
                        DocumentNo = AppFunctions.GetNewDocumentNumber(PositionsCode.Trim(), LastUsedNumber.Trim());
                    }

                    var pos = new ElectionPosition
                    {
                        PositionId = DocumentNo,
                        Name = ep.Name,
                        Sequence = ep.Sequence,
                        ElectionId = ep.ElectionId
                    };

                    using (ElectionManagementSystemEntities dbEntities = new ElectionManagementSystemEntities())
                    {
                        dbEntities.Configuration.ValidateOnSaveEnabled = false;
                        dbEntities.ElectionPositions.Add(pos);
                        dbEntities.SaveChanges();

                        message = "Position Created successfully";
                    }

                    //update last used number
                    AppFunctions.UpdateNumberSeries(PositionsCode, DocumentNo);
                }               
            }
            catch(Exception es)
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

        [HttpGet]
        public ActionResult Edit(string Id)
        {          
            List<Election> electionlist = new List<Election>();

            using (ElectionManagementSystemEntities dbEntities = new ElectionManagementSystemEntities())
            {
                var elections = dbEntities.Elections.ToList();

                foreach (var election in elections)
                {
                    electionlist.Add(new ElectionManagementSystem.Election
                    {
                        Name = election.Name,
                        ElectionId = election.ElectionId,
                        EndDate = election.EndDate,
                        StartDate = election.StartDate

                    });
                }
            }
            ViewBag.Elections = electionlist;

            //
            var std = _db.ElectionPositions.Where(s => s.PositionId == Id).FirstOrDefault();

            var _ElectionPosition = new ElectionPositionViewModel
            {                
                Name = std.Name,
                Sequence = std.Sequence,
                ElectionId = std.ElectionId,
                PositionId = std.PositionId
            };           


            return View(_ElectionPosition);
        }
        public ActionResult ViewPosition(string Id)
        {
            List<Election> electionlist = new List<Election>();

            using (ElectionManagementSystemEntities dbEntities = new ElectionManagementSystemEntities())
            {
                var elections = dbEntities.Elections.ToList();

                foreach (var election in elections)
                {
                    electionlist.Add(new Election
                    {
                        Name = election.Name,
                        ElectionId = election.ElectionId,
                        EndDate = election.EndDate,
                        StartDate = election.StartDate

                    });
                }
            }
            ViewBag.Elections = electionlist;

            //
            var std = _db.ElectionPositions.Where(s => s.PositionId == Id).FirstOrDefault();

            var _ElectionPosition = new ElectionPositionViewModel
            {
                Name = std.Name,
                Sequence = std.Sequence,
                ElectionId = std.ElectionId,
                PositionId = std.PositionId
            };


            return View(_ElectionPosition);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdatePosition(ElectionPositionViewModel ep)
        {
            string message = "", status = "";

            string test = ep.Name;

            try
            {
                if (_db.ElectionPositions.Any(o => o.Name == ep.Name && o.Name != ep.Name))
                {
                    message = "A position with similar name exists";
                    status = "900";
                }
                else if (_db.ElectionPositions.Any(o => o.Sequence == ep.Sequence && o.PositionId != ep.PositionId))
                {
                    message = "A position with similar sequence exists";
                    status = "900";
                }
                else
                {
                    ElectionPosition position = _db.ElectionPositions.Where(x => x.PositionId == ep.PositionId).SingleOrDefault();
                   
                    if(position != null)
                    {
                        position.Name = ep.Name;
                        position.ElectionId = ep.ElectionId;
                        position.Sequence = ep.Sequence;
                        _db.SaveChanges();

                        message = "Position updated successfully";
                        status = "000";
                    }
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
        public ActionResult Delete(string DocumentNo)
        {
            string status = "", message = "";

            try
            {
                using (var db = new ElectionManagementSystemEntities())
                {
                    var position = db.ElectionPositions.Where(x => x.PositionId == DocumentNo).SingleOrDefault();

                    if (position != null)
                    {
                        db.ElectionPositions.Remove(position);
                        db.SaveChanges();
                        status = "000";
                        message = "Delete Success! for position " + DocumentNo;
                    }
                    else
                    {
                        status = "900";
                        message = "Coouldn't find position " + DocumentNo;
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