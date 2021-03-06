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
    using ElectionManagementSystem.Models;
    public class HomeController : Controller
    {
        private ElectionManagementSystemEntities _db = new ElectionManagementSystemEntities();          

        public ActionResult Register()
        {           
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterUser _user)
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ForgotPassword(PasswordResetViewModel p)
        {
            if (ModelState.IsValid)
            {
                using (ElectionManagementSystemEntities dbEntities = new ElectionManagementSystemEntities())
                {
                    var user = dbEntities.Users.Where(u => u.Email == p.Email).SingleOrDefault();

                    if(user != null)
                    {
                        string resetCode = Guid.NewGuid().ToString();
                        var verifyUrl = "/Account/ResetPassword/" + resetCode;
                        var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, verifyUrl);

                        user.ResetPasswordCode = resetCode;

                        //This line I have added here to avoid confirm password not match issue , as we had added a confirm password property 

                        dbEntities.Configuration.ValidateOnSaveEnabled = false;
                        dbEntities.SaveChanges();

                        string domainName = Request.Url.GetLeftPart(UriPartial.Authority);

                        string EmailBody = EmailFunctions.EmailBody(
                            "Password Reset Success!", 
                            "Hi "+user.FirstName+", we are sorry that you forgot your password",
                            "Please click on the button below to reset it!",
                            domainName+verifyUrl,
                            "Reset"
                            );

                        EmailFunctions.SendMail(p.Email, user.FirstName, "Password Reset", EmailBody);


                        ViewBag.Message = "Reset password link has been sent to your email id.";
                    }
                }
            }
            return this.RedirectToAction("Login", "Account");
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