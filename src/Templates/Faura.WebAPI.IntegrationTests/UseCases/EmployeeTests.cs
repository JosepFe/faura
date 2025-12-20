namespace Faura.IntegrationTest.UseCases;

using System.Net;
using System.Net.Http.Json;
using Faura.IntegrationTest.Configuration;
using Faura.WebAPI.Domain.Entities;
using Xunit;

public class EmployeeTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public EmployeeTests(CustomWebApplicationFactory<Program> factory)
        => _client = factory.CreateClient();

    [Fact]
    public async Task Should_Get_Employees()
    {
        // Act
        var response = await _client.GetAsync("/Employee");

        // Assert
        response.EnsureSuccessStatusCode();
        var employees = await response.Content.ReadFromJsonAsync<IEnumerable<Employee>>();

        Assert.NotNull(employees);
    }

    [Fact]
    public async Task Should_Create_Employee()
    {
        // Arrange
        var newEmployee = new Employee("Test", "User", "test.user@example.com");

        // Act
        var response = await _client.PostAsJsonAsync("/Employee", newEmployee);

        // Assert
        response.EnsureSuccessStatusCode();
        var created = await response.Content.ReadFromJsonAsync<Employee>();

        Assert.NotNull(created);
        Assert.Equal(newEmployee.Email, created?.Email);
    }

    [Fact]
    public async Task Should_Get_Employee_By_Id()
    {
        // Arrange
        var employee = new Employee("Get", "ById", "get.byid@example.com");
        var post = await _client.PostAsJsonAsync("/Employee", employee);
        var created = await post.Content.ReadFromJsonAsync<Employee>();

        // Act
        var response = await _client.GetAsync($"/Employee/{created!.Id}");

        // Assert
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<Employee>();
        Assert.Equal(created.Id, result?.Id);
    }

    [Fact]
    public async Task Should_Delete_Employee()
    {
        // Arrange
        var employee = new Employee("To", "Delete", "to.delete@example.com");
        var post = await _client.PostAsJsonAsync("/Employee", employee);
        var created = await post.Content.ReadFromJsonAsync<Employee>();

        // Act
        var delete = await _client.DeleteAsync($"/Employee/{created!.Id}");

        // Assert
        Assert.Equal(HttpStatusCode.NoContent, delete.StatusCode);

        var get = await _client.GetAsync($"/Employee/{created.Id}");
        Assert.Equal(HttpStatusCode.NotFound, get.StatusCode);
    }

    [Fact]
    public async Task Should_Create_Two_Employees_In_Transaction()
    {
        // Act
        var response = await _client.PostAsync("/Employee/batch", null);

        // Assert
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadAsStringAsync();
        Assert.Equal("Two employees created in transaction.", result.Trim());
    }
}
