namespace Faura.WebAPI.Controllers;

using Faura.WebAPI.Domain;
using Faura.WebAPI.Domain.Entities;
using Faura.WebAPI.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IEmployeeUoW _uoW;

    public WeatherForecastController(ILogger<WeatherForecastController> logger, IEmployeeRepository employeeRepository, IEmployeeUoW uoW)
    {
        _logger = logger;
        _employeeRepository = employeeRepository;
        _uoW = uoW;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public async Task<IEnumerable<WeatherForecast>> Get()
    {
        var res = await _employeeRepository.GetAsync();

        var transaction = await _uoW.GetDbTransaction();

        var res2 = await _employeeRepository.CreateAsync(new Employee("Josep", "Ferrandis", "algo@example.com"), false, false);
        var res3 = await _employeeRepository.CreateAsync(new Employee("Josep", "Ferrandis", "algo@example.com"), false, false);

        // send event 1
        // send event 2

        await _uoW.CommitTransaction(transaction);

        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();
    }
}
