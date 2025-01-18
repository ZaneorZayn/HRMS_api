using System.ComponentModel.DataAnnotations;

namespace HRMS_api.Dto
{
    public class TokenRequestDto
    {
        [Required]
        public string RefreshToken { get; set; }
    }
}
