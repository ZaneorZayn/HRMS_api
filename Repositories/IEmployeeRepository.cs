using HRMS_api.Model;

namespace HRMS_api.Repositories;

public interface IEmployeeRepository
{
    Task<IEnumerable<Employee>> GetAllEmployeesAsync();
    Task<Employee> GetEmployeeByIdAsync(int id);
    Task<Employee> GetEmployeeWithDetailsAsync(int id);
    Task AddEmployeeAsync(Employee employee);
    void UpdateEmployee(Employee employee);
    void DeleteEmployee(Employee employee);
    Task<bool> SaveChangesAsync();
}