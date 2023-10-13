using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmployeeManagement.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class PayrollRecord
    {
        [Key]
        public String PayrollID { get; set; }
        public int EmployeeID { get; set; }
        public string Fname { get; set; }
        public System.DateTime Date { get; set; }
        public decimal Daily_Salary { get; set; }
        public Nullable<decimal> Annul_salary { get; set; }
        public Nullable<decimal> totalPay { get; set; }

        public int UserId { get; set; }
    }
}