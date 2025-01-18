using HRMS_api.Data;
using HRMS_api.Dto;
using HRMS_api.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace HRMS_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly HrmsDbContext _context;
        private readonly IConfiguration _configuration;


        public AuthController(HrmsDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        [HttpPost("register")]

        public async Task<IActionResult> Register([FromBody] RegisterEmployeeDto registerEmployeeDto)
        {
            try
            {
                if (registerEmployeeDto == null)
                {
                    return BadRequest("Invalid employee data.");
                }

                var employee = new Employee
                {
                    FullName = registerEmployeeDto.FullName,
                    PhoneNumber = registerEmployeeDto.PhoneNumber,
                    Email = registerEmployeeDto.Email,

                    HiredDate = registerEmployeeDto.HiredDate ?? DateTime.Now,
                    PositionId = registerEmployeeDto.PositionId,
                    RoleId = registerEmployeeDto.RoleId,
                    DepartmentId = registerEmployeeDto.DepartmentId
                };

                // Hash the password using BCrypt.Net
                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(registerEmployeeDto.Password);
                employee.Password = hashedPassword;

                await _context.Employees.AddAsync(employee);
                await _context.SaveChangesAsync();

                return Created("Employee created", null);
            }
            catch (Exception e)
            {
                return StatusCode(500, "An error occurred while creating the employee.");
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginEmployeeDto loginEmployeeDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid login credentials.");
            }

            // Verify employee email
            var employee = await _context.Employees.FirstOrDefaultAsync(e => e.Email == loginEmployeeDto.Email);
            if (employee == null)
            {
                return Unauthorized("Invalid email or password.");
            }

            // Verify employee password
            if (!BCrypt.Net.BCrypt.Verify(loginEmployeeDto.Password, employee.Password))
            {
                return Unauthorized("Invalid email or password.");
            }

            // Generate JWT Token
            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, employee.EmployeeId.ToString()),
            new Claim(ClaimTypes.Email, employee.Email)
        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetValue<string>("Jwt:Key")!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.UtcNow.AddDays(1);

            var acesstoken = new JwtSecurityToken(
                issuer: _configuration.GetValue<string>("Jwt:Issuer"),
                audience: _configuration.GetValue<string>("Jwt:Audience"),
                claims: claims,
                expires: expires,
                signingCredentials: creds
            );

            var acesstokenHandler = new JwtSecurityTokenHandler();
            var tokenString = acesstokenHandler.WriteToken(acesstoken);

            //Refresh Token

            string refreshToken = GenerateRefreshtoken();
            employee.RefreshToken = refreshToken;
            employee.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                Token = tokenString,
                Expiration = (int)acesstoken.ValidTo.Subtract(DateTime.Now).TotalSeconds,
                RefreshToken = refreshToken,

            });
        }
        private string GenerateRefreshtoken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }


        }
    }
}
