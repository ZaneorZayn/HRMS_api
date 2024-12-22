using HRMS_api.Model.EmployeeManagementAPI.Models;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Text.Json.Serialization;

namespace HRMS_api.Model
{
    public class Employee
    {
        [Key]
        [Required]
        public int EmployeeId { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime HiredDate { get; set; }

        
       public int DepartmentId { get; set; }//foreign key


        public int PositionId { get; set; }//foreign key

        public int RoleId { get; set; }//foreign key
        public string? Status { get; set; }
        [System.Text.Json.Serialization.JsonIgnore]
        public Position? Position { get; set; } // Navigation Properties
        [System.Text.Json.Serialization.JsonIgnore]
        public Role? Role { get; set; }  // Navigation Properties

        [System.Text.Json.Serialization.JsonIgnore]
        public virtual Department? Department { get; set; }  // Navigation Properties
        //[JsonProperty(NullValueHandling = NullValueHandling.Include)]
        public ICollection<Attendance>? Attendances { get; set; } // 1-to-Many with Attendance
        //[JsonProperty(NullValueHandling = NullValueHandling.Include)]
        public ICollection<Payroll>? Payrolls { get; set; }       // 1-to-Many with Payroll
        //[JsonProperty(NullValueHandling = NullValueHandling.Include)]
        public ICollection<Leave>? Leaves { get; set; }           // 1-to-Many with Leave
    }
}
