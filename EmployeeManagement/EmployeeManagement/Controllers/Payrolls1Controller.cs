using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Drawing.Printing;
using System.Linq;
using System.Net;
using System.Net.PeerToPeer;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;
using EmployeeManagement.Models;

namespace EmployeeManagement.Controllers
{
    public class Payrolls1Controller : Controller
    {
        private EmployeeEntities6 db = new EmployeeEntities6();

        // GET: Payrolls1

        [Authorize(Roles ="Owner")]
        public ActionResult Index()
        {
            PayrollRecord p = new PayrollRecord()
            {
                totalPay = 0
            };
            ViewBag.PayrollRecord = p;

            return View(db.Payrolls.ToList());
        }


        [HttpPost]
        public ActionResult Index(DateTime start, DateTime end)
        {
            

            int num1 = Convert.ToInt32(HttpContext.Request.Form["EmployeeID"].ToString());

            var check = db.Employees.FirstOrDefault(s => s.EmployeeID == num1);

            if(check != null)
            {
                if (HttpContext.Request.Form["EmployeeID"] == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

                }

                if (start == DateTime.MinValue || end == DateTime.MinValue)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                Payroll payroll = db.Payrolls.Find(num1);
                if (payroll == null)
                {
                    return HttpNotFound();
                }



                List<PayrollRecord> record = new List<PayrollRecord>();
                string start1 = start.ToShortDateString();
                string end1 = end.ToShortDateString();

                SqlConnection conn = new SqlConnection();
                SqlCommand cmd = new SqlCommand();
                conn.ConnectionString = ConfigurationManager.ConnectionStrings["aws"].ConnectionString;
                cmd.Connection = conn;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetTotalPay";
                cmd.Parameters.AddWithValue("@id", num1);
                cmd.Parameters.AddWithValue("@start", start);
                cmd.Parameters.AddWithValue("@end", end);



                cmd.Parameters.Add("@name", SqlDbType.VarChar, 100);
                cmd.Parameters["@name"].Direction = ParameterDirection.Output;

                cmd.Parameters.Add("@com", SqlDbType.Decimal);
                cmd.Parameters["@com"].Direction = ParameterDirection.Output;


                try
                {
                    conn.Open();
                    //Executing the SP
                    int i = cmd.ExecuteNonQuery();

                    PayrollRecord p = new PayrollRecord()
                    {
                        totalPay = Convert.ToDecimal(cmd.Parameters["@com"].Value)
                    };
                    ViewBag.PayrollRecord = p;


                }
                catch (Exception ex)
                {

                }
                finally
                {
                    conn.Close();
                }
                return View(db.GetSalary(start, end, num1));

             }  

            else
            {
                TempData["msg1"] = "Incorrect Username/Password ";
            }

            return View();

        }

        // GET: Payrolls1/Details/5
        public ActionResult Details(int? id)
               {
                            if (id == null)
                            {
                                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                            }
                            Payroll payroll = db.Payrolls.Find(id);
                            if (payroll == null)
                            {
                                return HttpNotFound();
               }
            return View(payroll);
        }

        // GET: Payrolls1/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Payrolls1/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PayrollID,EmployeeID,Fname,Daily_Salary,Annul_salary")] Payroll payroll)
        {
           
            if (ModelState.IsValid)
            {
                using (var context = new EmployeeEntities6())
                {
                    var data = db.Employees.Where(s => s.EmployeeID == payroll.EmployeeID && s.Fname == payroll.Fname).FirstOrDefault();
                    
                    if(data != null)
                    {

                        payroll.Date = DateTime.Now;
                        payroll.UserId = data.UserId;

                        db.Configuration.ValidateOnSaveEnabled = false;
                        db.Payrolls.Add(payroll);
                        db.SaveChanges();
                        return RedirectToAction("Index");

                    }
                    else
                    {
                        TempData["msg4"] = "not match!";


                    }


                }

               
            }

            return View(payroll);
        }

        // GET: Payrolls1/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Payroll payroll = db.Payrolls.Find(id);
            if (payroll == null)
            {
                return HttpNotFound();
            }
            return View(payroll);
        }

        // POST: Payrolls1/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PayrollID,EmployeeID,Fname,Date,Daily_Salary,Annul_salary")] Payroll payroll)
        {
            if (ModelState.IsValid)
            {
                db.Entry(payroll).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(payroll);
        }

        // GET: Payrolls1/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Payroll payroll = db.Payrolls.Find(id);
            if (payroll == null)
            {
                return HttpNotFound();
            }
            return View(payroll);
        }

        // POST: Payrolls1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Payroll payroll = db.Payrolls.Find(id);
            db.Payrolls.Remove(payroll);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
