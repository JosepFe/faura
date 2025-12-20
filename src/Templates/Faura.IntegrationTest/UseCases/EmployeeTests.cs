using Faura.IntegrationTest.Configuration;
using Faura.WebAPI.Controllers;
using Faura.WebAPI.Domain.Entities;
using System.Net.Http.Json;
namespace Faura.IntegrationTest.UseCases;
public class EmployeeTests : IntegrationTestBase
{
    public EmployeeTests(CustomWebApplicationFactory factory) : base(factory) { }

    protected override async Task SeedTestDataAsync()
    {
        await EmployeeRepository.CreateAsync(
            new Employee("Custom", "Seed", "custom@example.com")
        );
        await EmployeeRepository.CreateAsync(
            new Employee("Another", "User", "another@example.com")
        );
    }

    [Fact]
    public async Task Should_Return_Employees()
    {
        // Act
        var response = await Client.GetAsync("/Employee");
        // Assert
        response.EnsureSuccessStatusCode();
        var data = await response.Content.ReadFromJsonAsync<IEnumerable<Employee>>();
        Assert.NotNull(data);
        Assert.NotEmpty(data);
    }

    [Fact]
    public async Task Should_Create_Employee()
    {
        // Arrange
        var request = new CreateEmployeeRequest("John", "Doe", "john.doe@example.com");
        // Act
        var response = await Client.PostAsJsonAsync("/Employee", request);
        // Assert
        response.EnsureSuccessStatusCode();
        var createdEmployee = await response.Content.ReadFromJsonAsync<Employee>();
        Assert.NotNull(createdEmployee);
        Assert.Equal("John", createdEmployee.FirstName);
        Assert.Equal("Doe", createdEmployee.LastName);
        Assert.Equal("john.doe@example.com", createdEmployee.Email);
    }

    [Fact]
    public async Task Should_Create_Multiple_Employees_With_Transaction()
    {
        // Arrange
        var request = new CreateMultipleEmployeesRequest(
            "Jane",
            "Smith",
            "jane.smith@example.com",
            "Bob",
            "Johnson",
            "bob.johnson@example.com"
        );
        // Act
        var response = await Client.PostAsJsonAsync("/Employee/multiple", request);
        // Assert
        response.EnsureSuccessStatusCode();
        var createdEmployees = await response.Content.ReadFromJsonAsync<IEnumerable<Employee>>();
        Assert.NotNull(createdEmployees);
        Assert.Equal(2, createdEmployees.Count());
        var employeeList = createdEmployees.ToList();
        Assert.Equal("Jane", employeeList[0].FirstName);
        Assert.Equal("Bob", employeeList[1].FirstName);
    }
}