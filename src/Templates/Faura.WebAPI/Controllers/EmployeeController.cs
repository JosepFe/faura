using Faura.WebAPI.Application;
using Faura.WebAPI.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Faura.WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class EmployeeController : ControllerBase
{
    private readonly IEmployeeService _employeeService;

    public EmployeeController(IEmployeeService employeeService)
    {
        _employeeService = employeeService;
    }

    [HttpGet(Name = "GetEmployees")]
    public async Task<IEnumerable<Employee>> Get()
    {
        return await _employeeService.GetEmployeesAsync();
    }

    [HttpPost(Name = "CreateEmployee")]
    public async Task<Employee> Create([FromBody] CreateEmployeeRequest request)
    {
        return await _employeeService.CreateEmployeeAsync(
            request.FirstName,
            request.LastName,
            request.Email
        );
    }

    [HttpPost("multiple", Name = "CreateMultipleEmployees")]
    public async Task<IEnumerable<Employee>> CreateMultiple(
        [FromBody] CreateMultipleEmployeesRequest request
    )
    {
        return await _employeeService.CreateMultipleEmployeesWithTransactionAsync(
            request.FirstName1,
            request.LastName1,
            request.Email1,
            request.FirstName2,
            request.LastName2,
            request.Email2
        );
    }
}

public record CreateEmployeeRequest(string FirstName, string LastName, string Email);

public record CreateMultipleEmployeesRequest(
    string FirstName1,
    string LastName1,
    string Email1,
    string FirstName2,
    string LastName2,
    string Email2
);
