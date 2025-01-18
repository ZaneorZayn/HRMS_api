using HRMS_api.Dto;
using HRMS_api.Model;

namespace HRMS_api.Repositories;

public interface IEmployeeRepository
{
    Task<IEnumerable<Employee>> GetAllEmployeesAsync();
    Task<Employee> GetEmployeeByIdAsync(int id);
    Task<Employee> GetEmployeeWithDetailsAsync(int id);
    Task AddEmployeeAsync(CreateEmployeeDto createEmployeeDto);
    Task UpdateEmployee(int id ,UpdateEmployeeDto updateEmployeeDto);
    void DeleteEmployee(Employee employee);
    Task<bool> SaveChangesAsync();
}