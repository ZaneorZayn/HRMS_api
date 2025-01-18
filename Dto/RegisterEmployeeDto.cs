using System.ComponentModel.DataAnnotations;

namespace HRMS_api.Dto
{
    public class RegisterEmployeeDto
    {
        [Required]
        public string FullName { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [MinLength(8)] // Enforce minimum password length
        public string Password { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        public DateTime? HiredDate { get; set; } // Optional hired date

        public int DepartmentId { get; set; } // Foreign key for department

        public int PositionId { get; set; } // Foreign key for position

        public int RoleId { get; set; } // Foreign key for role
    }
}
