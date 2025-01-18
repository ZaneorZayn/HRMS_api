using HRMS_api.Data;
using HRMS_api.Dto;
using HRMS_api.Model;
using HRMS_api.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace HRMS_api.Repository
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly HrmsDbContext _context;

        public EmployeeRepository(HrmsDbContext context)
        {
            _context = context;
        }
      
        public async Task<IEnumerable<Employee>> GetAllEmployeesAsync()
        {
            return await _context.Employees.Include(e => e.Attendances).ToListAsync();
        }

        public async Task<Employee> GetEmployeeByIdAsync(int id)
        {
            return await _context.Employees.FindAsync(id);
        }

        public async Task<Employee> GetEmployeeWithDetailsAsync(int id)
        {
            return await _context.Employees
                .Include(e => e.Attendances)
                .FirstOrDefaultAsync(e => e.EmployeeId == id);
        }

        public async Task AddEmployeeAsync(CreateEmployeeDto createEmployeeDto)
        {
            var employee = new Employee
            {
                FullName = createEmployeeDto.FullName,
                PhoneNumber = createEmployeeDto.PhoneNumber,
                HiredDate = createEmployeeDto.HiredDate,
                PositionId = createEmployeeDto.PositionId,
                RoleId = createEmployeeDto.RoleId,
                DepartmentId = createEmployeeDto.DepartmentId
            };

            await _context.Employees.AddAsync(employee);
        }

        public async Task UpdateEmployee(int id, UpdateEmployeeDto updateEmployeeDto)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {

                throw new KeyNotFoundException("Employee not found");
            }
            employee.FullName = updateEmployeeDto.FullName;
            employee.PhoneNumber = updateEmployeeDto.PhoneNumber;
            employee.HiredDate = updateEmployeeDto.HiredDate;
            employee.DepartmentId = updateEmployeeDto.DepartmentId;
            employee.PositionId = updateEmployeeDto.PositionId;
            employee.RoleId = updateEmployeeDto.RoleId;


            _context.Employees.Update(employee);
        }
    

        

        public void DeleteEmployee(Employee employee)
        {
            _context.Employees.Remove(employee);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}