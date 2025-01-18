using HRMS_api.Data;
using HRMS_api.Enum;
using HRMS_api.Model.EmployeeManagementAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace HRMS_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttendanceController : ControllerBase
    {

        private readonly HrmsDbContext _context;

        public AttendanceController(HrmsDbContext hrmsDbContext)
        {
            _context = hrmsDbContext;
        }

        [HttpPost("check-in")]
        public async Task<IActionResult> CheckIn(int employeeId)
        {
            try
            {
                var today = DateTime.Now.Date;

                // Find employee by ID
                var employee = await _context.Employees.FindAsync(employeeId);

                if (employee == null)
                {
                    return NotFound("Employee not found.");
                }

                // Check for existing attendance for today
                var attendance = await _context.Attendances
                    .FirstOrDefaultAsync(a => a.EmployeeId == employeeId && a.Date == today);

                if (attendance != null)
                {
                    return BadRequest("Employee already checked in today.");
                }

                // Create new attendance
                attendance = new Attendance
                {
                    EmployeeId = employeeId,
                    Date = today,
                    CheckInTime = DateTime.Now.TimeOfDay
                };

                _context.Attendances.Add(attendance);
                await _context.SaveChangesAsync();
                

                return Ok(new {message = "Check-in Successfully", data = attendance});
            }
            catch (Exception ex)
            {
                // Log the exception for debugging (optional)
                // _logger.LogError(ex, "An error occurred during check-in.");

                return StatusCode(500, "An error occurred while checking in.");
            }
        }

        [HttpPost("check-out")]
        public async Task<IActionResult> CheckOut(int employeeId)
        {
            try
            {
                var today = DateTime.Now.Date;


                var employee = await _context.Employees.FindAsync(employeeId);
                if (employee == null)
                {
                    return BadRequest(new { message = "Employee not found." });
                }
                // Find attendance record for today
                var attendance = await _context.Attendances
                    .FirstOrDefaultAsync(a => a.EmployeeId == employeeId && a.Date == today);

                if (attendance == null)
                {
                    return NotFound("User has not checked in today.");
                }

                if (attendance.CheckOutTime.HasValue)
                {
                    return BadRequest("User already checked out today.");
                }

                attendance.CheckOutTime = DateTime.Now.TimeOfDay;

                _context.Attendances.Update(attendance);
                await _context.SaveChangesAsync();

                if (attendance.CheckInTime.HasValue)
                {
                    attendance.Status = Status.Present;
                }
                else
                {
                    attendance.Status = Status.Absent;
                }
                _context.Attendances.Update(attendance);
                await _context.SaveChangesAsync();

                return Ok(new {message = "Check out Successfully"});
            }
            catch (Exception ex)
            {
                // Log the exception for debugging (optional)
                // _logger.LogError(ex, "An error occurred during check-out.");

                return StatusCode(500, "An error occurred while checking out.");
            }
        }

        [HttpGet("id")]

        public async Task<IActionResult> GetAttendanceById(int id)
        {
            var attendance = await _context.Attendances.FindAsync(id);
            if (attendance == null)
            {
                return NotFound("Attendance not found.");
            }

            return Ok(attendance);
        }

        [HttpGet]

        public async Task<IEnumerable<Attendance>> GetAllAttendanceAsync()
        {
            return await _context.Attendances.ToListAsync();
        }
    }
}