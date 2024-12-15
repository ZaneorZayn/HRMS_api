using HRMS_api.Model.EmployeeManagementAPI.Models;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace HRMS_api.Model
{
    public class Employee
    {
        [Key]
        [Required]
        public int EmployeeId { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public DateTime HiredDate { get; set; }
        public int DepartmentId { get; set; }
        public Department? Department { get; set; }
        public int PositionId { get; set; }
        public Position Position { get; set; }
        public int RoleId { get; set; }
        public Role? Role { get; set; }
        //public int PositionId { get; set; }
        //public Position? Position { get; set; }
        public string? Status { get; set; }

        public ICollection<Attendance> Attendances { get; set; } // 1-to-Many with Attendance
        public ICollection<Payroll> Payrolls { get; set; }       // 1-to-Many with Payroll
        public ICollection<Leave> Leaves { get; set; }           // 1-to-Many with Leave
    }
}
