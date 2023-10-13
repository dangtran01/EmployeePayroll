using EmployeeManagement.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Web;
using System.Web.Mvc;


namespace EmployeeManagement.Controllers
{
    public class RecordController : Controller
    {
        private EmployeeEntities6 db = new EmployeeEntities6();
        public static int id; // Modifiable
        public static String name = ""; // Modifiable

        // GET: Record
        [Authorize(Roles = "User")]

        public ActionResult Index()
        {
            String ID = (String)Session["ID"];
            name = (String)Session["Name"];
             id = Convert.ToInt32(ID);

            dynamic model = new ExpandoObject();
            model.record2 = GetPayroll(id);
            model.record = GetEmployee(id);


            PayrollRecord p = new PayrollRecord()
            {
                Fname = name,
                totalPay = 0
            };
            ViewBag.PayrollRecord = p;


            return View(model);
        }

        [HttpPost]

        public ActionResult Index(DateTime start, DateTime end)
        {
            String id = (String)Session["ID"];
            int id1 = Convert.ToInt32(id);


            if (start == DateTime.MinValue || end == DateTime.MinValue)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            dynamic model = new ExpandoObject();
            model.record2 = GetPayroll2(id1, start, end);
            model.record = GetEmployee(id1);





            List<PayrollRecord> record = new List<PayrollRecord>();
            string start1 = start.ToShortDateString();
            string end1 = end.ToShortDateString();
            var data = db.Payrolls.Where(s => s.UserId == id1).FirstOrDefault();


            SqlConnection conn = new SqlConnection();
            SqlCommand cmd = new SqlCommand();
            conn.ConnectionString = ConfigurationManager.ConnectionStrings["aws"].ConnectionString;
            cmd.Connection = conn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "GetTotalPay";
            cmd.Parameters.AddWithValue("@id", data.EmployeeID);
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
                    Fname = name,
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


           
            return View(model);

        }


        public ActionResult totalPay(int id1, DateTime start, DateTime end)
        {

          
            return View();

        }






        public ActionResult NewRecord()
        {
            return View();

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NewRecord([Bind(Include = "Date,Daily_Salary")] Payroll payroll)
        {

            if (ModelState.IsValid)
            {
                using (var context = new EmployeeEntities6())
                {
                    var data = db.Employees.Where(s => s.UserId == id).FirstOrDefault();
                    
                    if(data != null)
                    {
                        payroll.EmployeeID = data.EmployeeID;
                        payroll.Fname = data.Fname;
                        payroll.UserId = id;
                        payroll.Date = DateTime.Now;
                        db.Configuration.ValidateOnSaveEnabled = false;


                        db.Payrolls.Add(payroll);
                        db.SaveChanges();
                        return RedirectToAction("Index");


                    }
                    else
                    {
                        TempData["msg7"] = "Emoloyee Record does not exist!";

                    }

                }


            }

            return View(payroll);
        }



            private static List<EmployeeRecord> GetEmployee(int id1)
        {

            List<EmployeeRecord> record = new List<EmployeeRecord>();
            string query = "SELECT * FROM dbo.Employee Where UserID =" + id1;

            string constr = ConfigurationManager.ConnectionStrings["aws"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            record.Add(new EmployeeRecord
                            {
                                EmployeeID = sdr["EmployeeID"].ToString(),
                                Fname = sdr["Fname"].ToString(),
                                Lname = sdr["Lname"].ToString(),
                                Address = sdr["Address"].ToString(),
                                Phone = sdr["Phone"].ToString(),
                                Nalicense = sdr["Nalicense"].ToString(),
                                UserId = sdr["UserId"].ToString()

                            });


                        }

                    }

                }
                con.Close();
                return record;

            }

        }

        private static List<PayrollRecord> GetPayroll(int id1)
        {

            List<PayrollRecord> record = new List<PayrollRecord>();
            string query = "SELECT * FROM dbo.Payroll Where UserID =" + id1;

            string constr = ConfigurationManager.ConnectionStrings["aws"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            record.Add(new PayrollRecord
                            {
                                EmployeeID = (int)sdr["EmployeeID"],
                                Fname = sdr["Fname"].ToString(),
                                Daily_Salary = (decimal)sdr["Daily_Salary"],
                                Annul_salary = (decimal?)sdr["Annul_salary"],
                                Date = (DateTime)sdr["Date"],
                                UserId = (int)sdr["UserId"]

                            });


                        }

                    }

                }
                con.Close();
                return record;
            }


        }


        private static List<PayrollRecord> GetPayroll2(int id1, DateTime start, DateTime end)
        {


            List<PayrollRecord> record = new List<PayrollRecord>();
            string start1 = start.ToString("yyyy-MM-dd");
            string end1 = end.ToString("yyyy-MM-dd");
            string query = "SELECT * FROM dbo.Payroll Where UserID =" + id1 + " and Date BETWEEN " + "'" + start1 + "'" + " AND " + "'" +end1 + "'";

            string constr = ConfigurationManager.ConnectionStrings["aws"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            record.Add(new PayrollRecord
                            {
                                EmployeeID = (int)sdr["EmployeeID"],
                                Fname = sdr["Fname"].ToString(),
                                Daily_Salary = (decimal)sdr["Daily_Salary"],
                                Annul_salary = (decimal?)sdr["Annul_salary"],
                                Date = (DateTime)sdr["Date"],
                                UserId = (int)sdr["UserId"]

                            });


                        }

                    }

                }
                con.Close();
                return record;
            }


        }





    }
}
