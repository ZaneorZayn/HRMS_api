using System.ComponentModel.DataAnnotations;

namespace HRMS_api.Model
{
    public class Department
    {
        [Key]
        [Required]
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public int ManagerId { get; set; }
        public ICollection<Employee> Employees { get; set; }
    }
}
