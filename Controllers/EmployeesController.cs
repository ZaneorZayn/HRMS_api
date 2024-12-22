using HRMS_api.Dto;
using HRMS_api.Model;
using HRMS_api.Repositories;
using HRMS_api.Repository;
using Microsoft.AspNetCore.Mvc;

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
            if (createEmployeeDto == null)
            {
                return BadRequest("Invalid employee data.");
            }

            var newEmployee = new Employee
            {
                FullName = createEmployeeDto.FullName,
                PhoneNumber = createEmployeeDto.PhoneNumber,
                HiredDate = createEmployeeDto.HiredDate,
                PositionId = createEmployeeDto.PositionId,
                RoleId = createEmployeeDto.RoleId,
                DepartmentId = createEmployeeDto.DepartmentId
            };

            await _employeeRepository.AddEmployeeAsync(newEmployee);
            var success = await _employeeRepository.SaveChangesAsync();
            if (!success) return StatusCode(500, "Error saving employee.");

            return CreatedAtAction(nameof(GetEmployeeById), new { id = newEmployee.EmployeeId }, newEmployee);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEmployee(int id, [FromBody] UpdateEmployeeDto updateEmployeeDto)
        {
            if (updateEmployeeDto == null)
            {
                return BadRequest("Invalid employee data.");
            }

            var existingEmployee = await _employeeRepository.GetEmployeeByIdAsync(id);
            if (existingEmployee == null)
            {
                return NotFound("Employee not found.");
            }

            existingEmployee.FullName = updateEmployeeDto.FullName;
            existingEmployee.PhoneNumber = updateEmployeeDto.PhoneNumber;
            existingEmployee.HiredDate = updateEmployeeDto.HiredDate;
            existingEmployee.PositionId = updateEmployeeDto.PositionId;
            existingEmployee.RoleId = updateEmployeeDto.RoleId;
            existingEmployee.DepartmentId = updateEmployeeDto.DepartmentId;

            _employeeRepository.UpdateEmployee(existingEmployee);
            var success = await _employeeRepository.SaveChangesAsync();
            if (!success) return StatusCode(500, "Error updating employee.");

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var employee = await _employeeRepository.GetEmployeeByIdAsync(id);
            if (employee == null) return NotFound();

            _employeeRepository.DeleteEmployee(employee);
            var success = await _employeeRepository.SaveChangesAsync();
            if (!success) return StatusCode(500, "Error deleting employee.");

            return NoContent();
        }
    }
}
