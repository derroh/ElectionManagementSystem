using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ElectionManagementSystem.Areas.Admin.Controllers
{
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
            string msg = "";

            try
            {
                var election = new Election
                {
                    ElectionId = "1",
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
    }
}