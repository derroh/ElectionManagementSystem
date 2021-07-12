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
        public ActionResult CreateElection(Election e)
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
                    StartDate = e.StartDate,
                    EndDate = e.EndDate

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
            return View();
        }
        public JsonResult ListElections()
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
            return Json(JsonConvert.SerializeObject(electionlist), JsonRequestBehavior.AllowGet);
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
    }
}