//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EmployeeManagement.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class Payroll
    {
        public int PayrollID { get; set; }

     
        public Nullable<int> EmployeeID { get; set; }
        public string Fname { get; set; }
        public Nullable<System.DateTime> Date { get; set; }

        [Required]
        public Nullable<decimal> Daily_Salary { get; set; }
        public Nullable<decimal> Annul_salary { get; set; }
        public Nullable<int> UserId { get; set; }
    
        public virtual Employee Employee { get; set; }
        public virtual User User { get; set; }
    }
}