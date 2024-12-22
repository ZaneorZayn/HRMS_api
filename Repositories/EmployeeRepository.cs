using HRMS_api.Data;
using HRMS_api.Model;
using HRMS_api.Repositories;
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
            return await _context.Employees.ToListAsync();
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

        public async Task AddEmployeeAsync(Employee employee)
        {
            await _context.Employees.AddAsync(employee);
        }

        public void UpdateEmployee(Employee employee)
        {
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