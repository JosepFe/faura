namespace Faura.WebAPI.Controllers;

using Faura.Infrastructure.Logger.Extensions;
using Faura.WebAPI.Domain;
using Faura.WebAPI.Domain.Entities;
using Faura.WebAPI.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController(
    ILogger<WeatherForecastController> logger,
    IEmployeeRepository employeeRepository,
    IEmployeeUoW uoW)
    : ControllerBase
{
    [HttpGet(Name = "GetWeatherForecast")]
    public async Task<IActionResult> Get()
    {
        logger.LogFauraInformation("Starting Get");

        await employeeRepository.GetAsync();

        var transaction = await uoW.GetDbTransaction();

        await employeeRepository.CreateAsync(
            new Employee("Josep", "Ferrandis", "algo@example.com"));

        await employeeRepository.CreateAsync(
            new Employee("Josep", "Ferrandis", "algo@example.com"));

        await uoW.CommitTransaction(transaction);

        return Ok();
    }
}
