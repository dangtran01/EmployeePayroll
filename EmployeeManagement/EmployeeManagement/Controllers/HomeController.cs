using EmployeeManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace EmployeeManagement.Controllers
{
    public class HomeController : Controller
    {
        private EmployeeEntities6 db = new EmployeeEntities6();

        public ActionResult About()
        {
            ViewBag.Message = "User Guide";

            return View();
        }

        public ActionResult Index()
        {
           
            return View();
        }
      
       
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

    }
}