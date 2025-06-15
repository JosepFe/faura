namespace Faura.IntegrationTest.UseCases;

using System.Net.Http.Json;
using Faura.IntegrationTest.Configuration;
using Faura.WebAPI.Controllers;
using Xunit;

public class WeatherForecastTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public WeatherForecastTests(CustomWebApplicationFactory<Program> factory)
        => _client = factory.CreateClient();

    [Fact]
    public async Task Should_Return_WeatherForecast()
    {
        // Act
        var response = await _client.GetAsync("/WeatherForecast");

        // Assert
        response.EnsureSuccessStatusCode();

        var data = await response.Content.ReadFromJsonAsync<IEnumerable<WeatherForecast>>();
        Assert.NotNull(data);
        Assert.NotEmpty(data);
    }
}
