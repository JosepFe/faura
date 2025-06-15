namespace Faura.WebAPI.Controllers;

using Faura.Infrastructure.Logger.Extensions;
using Faura.WebAPI.Domain.Entities;
using Faura.WebAPI.Domain.Repositories;
using Faura.WebAPI.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

[ApiController]
[Route("[controller]")]
public class EmployeeController(
    ILogger<EmployeeController> logger,
    IEmployeeRepository employeeRepository,
    IEmployeeUoW uoW)
    : ControllerBase
{
    [HttpGet(Name = "GetEmployees")]
    public async Task<IActionResult> Get()
    {
        logger.LogFauraInformation("Getting all employees");
        var employees = await employeeRepository.GetAsync();
        return Ok(employees);
    }

    [HttpPost("batch")]
    public async Task<IActionResult> CreateBatch()
    {
        logger.LogFauraInformation("Creating two employees in a transaction");

        var transaction = await uoW.GetDbTransaction();

        await employeeRepository.CreateAsync(new Employee("Josep", "Ferrandis", "josep1@example.com"));
        await employeeRepository.CreateAsync(new Employee("Josep", "Ferrandis", "josep2@example.com"));

        await uoW.CommitTransaction(transaction);

        return Ok("Two employees created in transaction.");
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Employee employee)
    {
        await employeeRepository.CreateAsync(employee);
        return CreatedAtAction(nameof(GetById), new { id = employee.Id }, employee);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(long id)
    {
        var employee = await employeeRepository.GetByIdAsync(id);
        return employee is not null ? Ok(employee) : NotFound();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(long id)
    {
        var employee = await employeeRepository.GetByIdAsync(id);
        if (employee is null)
            return NotFound();

        await employeeRepository.DeleteAsync(employee);
        return NoContent();
    }
}
