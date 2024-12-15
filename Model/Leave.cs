using HRMS_api.Enum;
using System.ComponentModel.DataAnnotations;

namespace HRMS_api.Model
{
    public class Leave
    {
        [Key]
        [Required]
        public int LeaveId { get; set; }
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }
        public string LeaveType { get; set; } // Enum: Sick, Casual, etc.
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Leave_Enum Status{ get; set; } // Enum: Pending, Approved, Rejected
    }
}
