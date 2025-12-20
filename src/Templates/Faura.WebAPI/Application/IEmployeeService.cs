using Faura.WebAPI.Domain.Entities;

namespace Faura.WebAPI.Application;

public interface IEmployeeService
{
    Task<IEnumerable<Employee>> GetEmployeesAsync();
    Task<Employee> CreateEmployeeAsync(string firstName, string lastName, string email);
    Task<IEnumerable<Employee>> CreateMultipleEmployeesWithTransactionAsync(
        string firstName1,
        string lastName1,
        string email1,
        string firstName2,
        string lastName2,
        string email2
    );
}
