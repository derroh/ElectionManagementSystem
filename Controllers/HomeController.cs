using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace ElectionManagementSystem.Controllers
{
    public class HomeController : Controller
    {
        private ElectionManagementSystemEntities _db = new ElectionManagementSystemEntities();          

        public ActionResult Register()
        {           
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(Models.RegisterUser _user)
        {
            if (ModelState.IsValid)
            {               
                var student = new Student { Email = _user.Email, Phone = _user.Phone, FirstName = _user.FirstName, LastName = _user.LastName, Name = _user.FirstName +" "+_user.LastName, StudentId = _user.StudentId };             

                using (ElectionManagementSystemEntities dbEntities = new ElectionManagementSystemEntities())
                {
                    dbEntities.Configuration.ValidateOnSaveEnabled = false;
                    dbEntities.Students.Add(student);
                    dbEntities.SaveChanges();
                }
                var user = new User
                {
                    Email = _user.Email,
                    Password = GetMD5(_user.Password),
                    FirstName = _user.FirstName,
                    LastName = _user.LastName,
                    Phone = _user.Phone,
                    Role = "Admin",
                    StudentId = _user.StudentId

                };

                using (ElectionManagementSystemEntities dbEntities = new ElectionManagementSystemEntities())
                {
                    dbEntities.Configuration.ValidateOnSaveEnabled = false;
                    dbEntities.Users.Add(user);
                    dbEntities.SaveChanges();
                }
                AppFunctions.SendTextMessage(_user.Phone, " Dear "+ _user.FirstName + ", your account on Bettie's voting system has been created at " + DateTime.Now.ToShortTimeString());

                ViewBag.Message = "Account Created successfully";
                return RedirectToAction("Index");
            }
            return View();
        }
        
        [AllowAnonymous]
        public ActionResult Reset()
        {
            return View();
        }

        //create a string MD5
        public static string GetMD5(string str)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] fromData = Encoding.UTF8.GetBytes(str);
            byte[] targetData = md5.ComputeHash(fromData);
            string byte2String = null;

            for (int i = 0; i < targetData.Length; i++)
            {
                byte2String += targetData[i].ToString("x2");

            }
            return byte2String;
        }

    }
}