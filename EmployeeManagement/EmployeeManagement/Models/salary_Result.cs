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
    
    public partial class salary_Result
    {
        public int PayrollID { get; set; }
        public int EmployeeID { get; set; }
        public string Fname { get; set; }
        public System.DateTime Date { get; set; }
        public decimal Daily_Salary { get; set; }
        public Nullable<decimal> Annul_salary { get; set; }
        public Nullable<int> UserId { get; set; }
        public Nullable<decimal> totalPay { get; set; }
    }
}