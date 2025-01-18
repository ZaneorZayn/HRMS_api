using HRMS_api.Dto;
using HRMS_api.Model;
using HRMS_api.Repositories;
using HRMS_api.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HRMS_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeesController(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllEmployees()
        {
            var employees = await _employeeRepository.GetAllEmployeesAsync();
            return Ok(employees);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmployeeById(int id)
        {
            var employee = await _employeeRepository.GetEmployeeWithDetailsAsync(id);
            if (employee == null) return NotFound();

            return Ok(employee);
        }

        [HttpPost]
        public async Task<IActionResult> CreateEmployee([FromBody] CreateEmployeeDto createEmployeeDto)
        {
            try
            {
                if (createEmployeeDto == null)
                {
                    return BadRequest("Invalid employee data.");
                }

                await _employeeRepository.AddEmployeeAsync(createEmployeeDto);
                var success = await _employeeRepository.SaveChangesAsync();
                if (!success) return StatusCode(500, "Error saving employee.");

                return Created("Employee created", null);


            }
            catch (Exception e)
            {

                // Return a user-friendly error message to the client
                return StatusCode(500, "An error occurred while creating the employee.");
            }
        }

        [HttpPut("{id}")]

        public async Task<IActionResult> UpdateEmployee(int id, [FromBody] UpdateEmployeeDto updateEmployeeDto)
        {
            try
            {
                if (updateEmployeeDto == null)
                {
                    return BadRequest("Invalid employee data.");
                }

                await _employeeRepository.UpdateEmployee(id, updateEmployeeDto);
                var success = await _employeeRepository.SaveChangesAsync();

                if (!success) return StatusCode(500, "Error updating employee.");
                return Ok("Employee updated successfully.");
            }
            catch (KeyNotFoundException)
            {

                // Return a user-friendly error message to the client
                return NotFound("Employee not found.");
            }
            catch (Exception e)
            {
                return StatusCode(500, "An error occurred while updating the employee.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var employee = await _employeeRepository.GetEmployeeByIdAsync(id);
            if (employee == null) 
                return NotFound();

            _employeeRepository.DeleteEmployee(employee);
            var success = await _employeeRepository.SaveChangesAsync();
            if (!success) return StatusCode(500, "Error deleting employee.");

            return NoContent();
        }
    }
}
