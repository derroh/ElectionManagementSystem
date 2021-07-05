using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Web;
using System.Web.Mvc;

namespace ElectionManagementSystem.Areas.Admin.Controllers
{
    [Authorize]
    public class PositionsController : Controller
    {
        private ElectionManagementSystemEntities _db = new ElectionManagementSystemEntities();
        // GET: Admin/Positions
        //[Authorize]
        public ActionResult Index()
        {
            return View(from positions in _db.ElectionPositions.Take(10)
                    select positions);
        }
        public ActionResult Create(ElectionPosition ep)
        {                
            return View();
        }
        public ActionResult CreatePosition(ElectionPosition ep)
        {
            string msg = "";
            string DocumentNo = null;

            try
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
                    ElectionId = DocumentNo= ep.ElectionId
                };

                using (ElectionManagementSystemEntities dbEntities = new ElectionManagementSystemEntities())
                {
                    dbEntities.Configuration.ValidateOnSaveEnabled = false;
                    dbEntities.ElectionPositions.Add(pos);
                    dbEntities.SaveChanges();

                    msg = "Position Created successfully";
                }

                //update last used number
                AppFunctions.UpdateNumberSeries(PositionsCode, DocumentNo);
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

        [HttpGet]
        public ActionResult Edit(string Id)
        {
            var std = _db.ElectionPositions.Where(s => s.PositionId == Id).FirstOrDefault();
            List<ElectionPosition> electionPosition = new List<ElectionPosition>();

            electionPosition.Add(new ElectionPosition
            {
                Name = std.Name,
                PositionId = std.PositionId
            });
           

            return View(electionPosition);
        }
        [HttpPost]
        public ActionResult UpdatePosition(ElectionPosition ep)
        {
            string msg = "";

            string test = ep.Name;

            try
            {
                if (_db.ElectionPositions.Any(o => o.Name == ep.Name))
                {
                    msg = "A position with similar name exists";
                }
                else
                {
                    ElectionPosition position = _db.ElectionPositions.Where(x => x.PositionId == ep.PositionId).SingleOrDefault();
                   
                    if(position != null)
                    {
                        position.Name = ep.Name;
                        _db.SaveChanges();

                        msg = "Position updated successfully";
                    }
                }

               
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