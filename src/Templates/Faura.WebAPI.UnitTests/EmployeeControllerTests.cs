namespace Faura.WebAPI.Tests.Controllers;

using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Faura.WebAPI.Controllers;
using Faura.WebAPI.Domain.Entities;
using Faura.WebAPI.Domain.Repositories;
using Faura.WebAPI.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

public class EmployeeControllerTests
{
    private readonly ILogger<EmployeeController> _logger = Substitute.For<ILogger<EmployeeController>>();
    private readonly IEmployeeRepository _repository = Substitute.For<IEmployeeRepository>();
    private readonly IEmployeeUoW _uow = Substitute.For<IEmployeeUoW>();
    private readonly EmployeeController _controller;

    public EmployeeControllerTests()
        => _controller = new EmployeeController(_logger, _repository, _uow);

    [Fact]
    public async Task Get_ReturnsEmployees()
    {
        // Arrange
        var employees = new List<Employee> { new("Josep", "Ferrandis", "test@example.com") };
        _repository.GetAsync().Returns(employees);

        // Act
        var result = await _controller.Get();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(employees, okResult.Value);
    }

    [Fact]
    public async Task Create_ReturnsCreatedAtAction()
    {
        // Arrange
        var employee = new Employee("Test", "User", "test@domain.com");
        await _repository.CreateAsync(employee);

        // Act
        var result = await _controller.Create(employee);

        // Assert
        var created = Assert.IsType<CreatedAtActionResult>(result);
        Assert.Equal(nameof(_controller.GetById), created.ActionName);
        Assert.Equal(employee, created.Value);
    }

    [Fact]
    public async Task CreateBatch_CreatesTwoEmployeesAndCommitsTransaction()
    {
        // Arrange
        var fakeTransaction = Substitute.For<IDbTransaction>();
        _uow.GetDbTransaction(Arg.Any<int>()).Returns(fakeTransaction);

        // Act
        var result = await _controller.CreateBatch();

        // Assert
        await _repository.Received(2).CreateAsync(Arg.Is<Employee>(e =>
            e.FirstName == "Josep" && e.LastName == "Ferrandis" && e.Email.StartsWith("josep")));

        await _uow.Received(1).CommitTransaction(fakeTransaction);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal("Two employees created in transaction.", okResult.Value);
    }

    [Fact]
    public async Task GetById_EmployeeExists_ReturnsOk()
    {
        // Arrange
        var id = 1;
        var employee = new Employee("A", "B", "a@b.com") { Id = id };
        _repository.GetByIdAsync(id).Returns(employee);

        // Act
        var result = await _controller.GetById(id);

        // Assert
        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(employee, ok.Value);
    }

    [Fact]
    public async Task GetById_EmployeeNotFound_ReturnsNotFound()
    {
        var id = 1;
        _repository.GetByIdAsync(id).Returns((Employee?)null);

        var result = await _controller.GetById(id);

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task Delete_EmployeeExists_DeletesAndReturnsNoContent()
    {
        var id = 1;
        var employee = new Employee("Del", "Ete", "del@ete.com") { Id = id };
        _repository.GetByIdAsync(id).Returns(employee);

        var result = await _controller.Delete(id);

        await _repository.Received(1).DeleteAsync(employee);
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task Delete_EmployeeNotFound_ReturnsNotFound()
    {
        var id = 1;
        _repository.GetByIdAsync(id).Returns((Employee?)null);

        var result = await _controller.Delete(id);

        Assert.IsType<NotFoundResult>(result);
    }
}
