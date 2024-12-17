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
        public string? PhoneNumber { get; set; }
        public DateTime HiredDate { get; set; }

        
        public int DepartmentId { get; set; }//foreign key


        public int PositionId { get; set; }//foreign key

        public int RoleId { get; set; }//foreign key
        public string? Status { get; set; }

        public Position Position { get; set; } // Navigation Properties
        public Role? Role { get; set; }  // Navigation Properties
        public Department? Department { get; set; }  // Navigation Properties
        public ICollection<Attendance>? Attendances { get; set; } // 1-to-Many with Attendance
        public ICollection<Payroll>? Payrolls { get; set; }       // 1-to-Many with Payroll
        public ICollection<Leave>? Leaves { get; set; }           // 1-to-Many with Leave
    }
}
