using System.ComponentModel.DataAnnotations;

namespace HRMS_api.Model
{
    public class Payroll
    {
        [Key]
        [Required]
        public int PayrollId { get; set; }
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }
        public decimal BasicSalary { get; set; }
        public decimal Deduction { get; set; }
        public decimal NetSalary { get; set; }
        public DateTime PaymentDate { get; set; }
    }
}
