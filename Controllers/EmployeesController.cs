using HRMS_api.Data;
using HRMS_api.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace HRMS_api.Controllers
{   
    [Route("api/[controller]")]
    [ApiController]
    
    public class EmployeesController : ControllerBase
    {
        private readonly HrmsDbContext _context;

        public EmployeesController(HrmsDbContext  context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllEmployees()
        {
            var employees = await _context.Employees
                                          .Include(e => e.Department)
                                          .Include(e => e.Position)
                                          .Include(e => e.Role)
                                          .Include(e => e.Attendances) // Include Attendances
                                          .Select(e => new
                                          {
                                              e.EmployeeId,
                                              e.FullName,
                                              e.PhoneNumber,
                                              e.HiredDate,
                                              Department = e.Department != null ? e.Department.DepartmentName : null,
                                              Position = e.Position != null ? e.Position.PositionName : null,
                                              Role = e.Role != null ? e.Role.RoleName : null,
                                              e.Status,
                                              Attendances = e.Attendances.Select(a => new
                                              {
                                                  a.AttendanceId,
                                                  a.Date,
                                                  Status = a.Status.ToString(),
                                                  a.CheckInTime,
                                                  a.CheckOutTime,
                                              })
                                          })
                                          .ToListAsync();

            return Ok(employees);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmployeeById(int id)
        {
            var employee = await _context.Employees.Include(e => e.Department)
                                                   .Include(e => e.Role)
                                                   .Include(e => e.Position)
                                                   .FirstOrDefaultAsync(e => e.EmployeeId == id);
            if (employee == null) return NotFound();

            return Ok(employee);
        }

        [HttpPost]
        public async Task<IActionResult> CreateEmployee([FromBody] Employee employee)
        {
            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetEmployeeById), new { id = employee.EmployeeId }, employee);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEmployee(int id, [FromBody] Employee employee)
        {
            if (id != employee.EmployeeId) return BadRequest();

            _context.Entry(employee).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null) return NotFound();

            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
