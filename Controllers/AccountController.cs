//-----------------------------------------------------------------------
// <copyright file="AccountController.cs" company="None">
//     Copyright (c) Allow to distribute this code.
// </copyright>
// <author>Asma Khalid</author>
//-----------------------------------------------------------------------

namespace ElectionManagementSystem.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Web;
    using System.Web.Mvc;
    using Microsoft.AspNet.Identity;
    using Microsoft.Owin;
    using Microsoft.Owin.Security;
    using ElectionManagementSystem.Models;
    using System.Security.Cryptography;
    using System.Text;

    /// <summary>
    /// Account controller class.
    /// </summary>
    public class AccountController : Controller
    {
        #region Private Properties

        /// <summary>
        /// Database Store property.
        /// </summary>
        private ElectionManagementSystemEntities _db = new ElectionManagementSystemEntities();

        #endregion

        #region Default Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountController" /> class.
        /// </summary>
        public AccountController()
        {
        }

        #endregion

        #region Login methods

        /// <summary>
        /// GET: /Account/Login
        /// </summary>
        /// <param name="returnUrl">Return URL parameter</param>
        /// <returns>Return login view</returns>
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            try
            {
                // Verification.
                if (this.Request.IsAuthenticated)
                {
                    // Info.
                    try
                    {
                        // Setting.
                        var ctx = Request.GetOwinContext();
                        var authenticationManager = ctx.Authentication;

                        // Sign Out.
                        authenticationManager.SignOut();
                    }
                    catch (Exception ex)
                    {
                        // Info
                        throw ex;
                    }

                    // Info.
                    return this.RedirectToAction("Login", "Account");

                }
                else
                {
                    return View();
                }
            }
            catch (Exception ex)
            {
                // Info
                Console.Write(ex);
            }

            // Info.
            return this.View();
        }

        /// <summary>
        /// POST: /Account/Login
        /// </summary>
        /// <param name="model">Model parameter</param>
        /// <param name="returnUrl">Return URL parameter</param>
        /// <returns>Return login view</returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(UserLoginView model, string returnUrl)
        {
            try
            {
                // Verification.
                if (ModelState.IsValid)
                {
                    string email = model.Email;
                    string password = GetMD5(model.Password);

                    string Area = "Student";

                    var loginInfo = this._db.Users.Where(m => m.Email == email && m.Password == password).ToList();

                    // Verification.
                    if (loginInfo != null && loginInfo.Count() > 0)
                    {
                        // Initialization.
                        var logindetails = loginInfo.First();

                        // Login In.
                        this.SignInUser(logindetails.Email, logindetails.Role, false);

                        // setting.
                        string role = logindetails.Role;

                        this.Session["role_id"] = role.Trim();
                        this.Session["FirstName"] = logindetails.FirstName;

                        if (logindetails.Role.Trim() == "Admin")
                        {
                            Area = "Admin";
                        }

                        // Info.
                        
                        if (String.IsNullOrEmpty(returnUrl))
                        {                            
                            return RedirectToAction("Index", "Home", new { area = Area });
                        }
                        else
                        {
                            return this.RedirectToLocal(returnUrl);
                        }                            
                    }
                    else
                    {
                        // Setting.
                        ModelState.AddModelError(string.Empty, "Invalid username or password.");
                    }
                }
            }
            catch (Exception ex)
            {
                // Info
                Console.Write(ex);
            }

            // If we got this far, something failed, redisplay form
            return this.View(model);
        }

        #endregion

        #region Log Out method.

        /// <summary>
        /// POST: /Account/LogOff
        /// </summary>
        /// <returns>Return log off action</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            try
            {
                // Setting.
                var ctx = Request.GetOwinContext();
                var authenticationManager = ctx.Authentication;

                // Sign Out.
                authenticationManager.SignOut();
            }
            catch (Exception ex)
            {
                // Info
                throw ex;
            }

            // Info.
            return this.RedirectToAction("Login", "Account");
        }

        #endregion

        #region Helpers

        #region Sign In method.

        /// <summary>
        /// Sign In User method.
        /// </summary>
        /// <param name="username">Username parameter.</param>
        /// <param name="role_id">Role ID parameter</param>
        /// <param name="isPersistent">Is persistent parameter.</param>
        private void SignInUser(string username, string role_id, bool isPersistent)
        {
            // Initialization.
            var claims = new List<Claim>();

            try
            {
                // Setting
                claims.Add(new Claim(ClaimTypes.Name, username));
                claims.Add(new Claim(ClaimTypes.Role, role_id));
                var claimIdenties = new ClaimsIdentity(claims, DefaultAuthenticationTypes.ApplicationCookie);
                var ctx = Request.GetOwinContext();
                var authenticationManager = ctx.Authentication;

                // Sign In.
                authenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = isPersistent }, claimIdenties);
            }
            catch (Exception ex)
            {
                // Info
                throw ex;
            }
        }

        #endregion

        #region Redirect to local method.

        /// <summary>
        /// Redirect to local method.
        /// </summary>
        /// <param name="returnUrl">Return URL parameter.</param>
        /// <returns>Return redirection action</returns>
        private ActionResult RedirectToLocal(string returnUrl)
        {
            try
            {
                // Verification.
                if (Url.IsLocalUrl(returnUrl))
                {
                    // Info.
                    return this.Redirect(returnUrl);
                }
            }
            catch (Exception ex)
            {
                // Info
                throw ex;
            }

            // Info.
            return this.RedirectToAction("Index", "Home");
        }

        #endregion

        #endregion

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