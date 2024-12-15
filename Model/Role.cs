using System.ComponentModel.DataAnnotations;

namespace HRMS_api.Model
{
    public class Role
    {
        [Key]
        [Required]
        public int Id {get;set;}
        public string RoleName { get; set; }
        public ICollection<Employee> Employees { get; set; }
    }
}
