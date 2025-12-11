using Faura.Infrastructure.Logger.Extensions;
using Faura.WebAPI.Domain;
using Faura.WebAPI.Domain.Entities;
using Faura.WebAPI.Infrastructure.Persistence;

namespace Faura.WebAPI.Application;

public class EmployeeService : IEmployeeService
{
    private readonly ILogger<EmployeeService> _logger;
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IEmployeeUoW _uoW;

    public EmployeeService(
        ILogger<EmployeeService> logger,
        IEmployeeRepository employeeRepository,
        IEmployeeUoW uoW
    )
    {
        _logger = logger;
        _employeeRepository = employeeRepository;
        _uoW = uoW;
    }

    public async Task<IEnumerable<Employee>> GetEmployeesAsync()
    {
        _logger.LogFauraInformation("Starting Get Employees");
        return await _employeeRepository.GetAsync();
    }

    public async Task<Employee> CreateEmployeeAsync(string firstName, string lastName, string email)
    {
        _logger.LogFauraInformation($"Creating employee: {firstName} {lastName}");
        var employee = new Employee(firstName, lastName, email);
        var result = await _employeeRepository.CreateAsync(employee);
        return result;
    }

    public async Task<IEnumerable<Employee>> CreateMultipleEmployeesWithTransactionAsync(
        string firstName1,
        string lastName1,
        string email1,
        string firstName2,
        string lastName2,
        string email2
    )
    {
        _logger.LogFauraInformation("Creating multiple employees with transaction");
        
        var transaction = await _uoW.GetDbTransaction();

        var employee1 = await _employeeRepository.CreateAsync(
            new Employee(firstName1, lastName1, email1),
            false,
            false
        );
        
        var employee2 = await _employeeRepository.CreateAsync(
            new Employee(firstName2, lastName2, email2),
            false,
            false
        );

        // send event 1
        // send event 2

        await _uoW.CommitTransaction(transaction);

        return [employee1, employee2];
    }
}
