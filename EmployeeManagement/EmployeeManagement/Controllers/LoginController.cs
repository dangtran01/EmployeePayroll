using EmployeeManagement.Models;
using Microsoft.AspNetCore.Http.Authentication;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace EmployeeManagement.Controllers
{
    public class LoginController : Controller
    {

        private EmployeeEntities6 db = new EmployeeEntities6();

        // GET: Login
    
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(User u, String returnUrl)
        {
            if(u.Password == null)
            {
                TempData["msg3"] = "Username/Password"; 

            }
            else
            {

                string hashpassword = GetMD5(u.Password);
                var data = db.Users.Where(s => s.Username == u.Username && s.Password.Equals(hashpassword)).FirstOrDefault();
                if (ModelState.IsValid)
                {

                    if (data != null)
                    {

                        FormsAuthentication.SetAuthCookie(u.Username, false);
                        //add session
                        Session["Name"] = u.Username.ToString();
                        Session["ID"] = data.UserId.ToString();

                        if (returnUrl == null)
                        {
                            return RedirectToAction("About", "Home");
                        }
                        else
                        {
                            return RedirectToAction("Login");
                        }

                    }
                    else
                    {
                        TempData["msg"] = "Incorrect Username/Password ";
                    }
                }


            }




            return View();
        }
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


        public ActionResult logout()
        {
           
            FormsAuthentication.SignOut();
            HttpCookie cookie = HttpContext.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (cookie != null)
            {
                
                Response.Cookies.Remove(FormsAuthentication.FormsCookieName);
                HttpContext.Session.Clear();
                Session["Name"] = null;
                Session["ID"] = null;
                Session.Abandon();
                cookie.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(cookie);
                //Removes all keys and values from the session-state collection.
               
            }
            
           

            return RedirectToAction("Login","Login");
        }




    }
}
