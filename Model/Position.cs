using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HRMS_api.Model
{
    public class Position
    {
        [Key]
        [Required]
        public int PositionId { get; set; } // Primary Key

        [Required]
        public string PositionName { get; set; } // Position name

        public ICollection<Employee> Employees { get; set; } // Navigation property
    }
}
