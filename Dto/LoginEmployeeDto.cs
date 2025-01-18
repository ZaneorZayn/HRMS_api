using System.ComponentModel.DataAnnotations;

namespace HRMS_api.Dto
{
    public class LoginEmployeeDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
