namespace Faura.IntegrationTest.UseCases;

using Faura.IntegrationTest.Configuration;
using Faura.WebAPI.Domain.Entities;
using System.Net;
using System.Net.Http.Json;

/// <summary>
/// Integration tests for Sample API endpoints.
/// Tests cover CRUD operations and transaction handling.
/// </summary>
public class SampleTests(CustomWebApplicationFactory factory) : IntegrationTestBase(factory)
{
    /// <summary>
    /// Seeds custom test data for this test class.
    /// This runs before each test via IAsyncLifetime.
    /// </summary>
    protected override async Task SeedTestDataAsync()
    {
        await SampleRepository.CreateAsync(
            new Sample("Test Sample", "Test description", "TestCategory")
        );
        await SampleRepository.CreateAsync(
            new Sample("Another Sample", "Another description", "CategoryB")
        );
    }

    #region GET Tests

    [Fact]
    public async Task GetAll_ShouldReturnAllSamples()
    {
        // Act
        var response = await Client.GetAsync("/api/samples");
        
        // Assert
        response.EnsureSuccessStatusCode();
        var data = await response.Content.ReadFromJsonAsync<IEnumerable<Sample>>();
        
        Assert.NotNull(data);
        Assert.NotEmpty(data);
        // At least 2 from seed + 3 from global seeder
        Assert.True(data.Count() >= 5);
    }

    [Fact]
    public async Task GetById_ShouldReturnSample_WhenExists()
    {
        // Arrange
        var sample = await SampleRepository.CreateAsync(
            new Sample("GetById Test", "Test description", "CategoryA")
        );
        
        // Act
        var response = await Client.GetAsync($"/api/samples/{sample.Id}");
        
        // Assert
        response.EnsureSuccessStatusCode();
        var data = await response.Content.ReadFromJsonAsync<Sample>();
        
        Assert.NotNull(data);
        Assert.Equal(sample.Id, data.Id);
        Assert.Equal("GetById Test", data.Name);
    }

    [Fact]
    public async Task GetById_ShouldReturnNotFound_WhenNotExists()
    {
        // Act
        var response = await Client.GetAsync("/api/samples/999999");
        
        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    #endregion

    #region POST Tests

    [Fact]
    public async Task Create_ShouldCreateSample_WithValidData()
    {
        // Arrange
        var request = new { name = "New Sample", description = "New description", category = "CategoryA" };
        
        // Act
        var response = await Client.PostAsJsonAsync("/api/samples", request);
        
        // Assert
        response.EnsureSuccessStatusCode();
        
        var createdSample = await response.Content.ReadFromJsonAsync<Sample>();
        Assert.NotNull(createdSample);
        Assert.Equal("New Sample", createdSample.Name);
        Assert.Equal("New description", createdSample.Description);
        Assert.Equal("CategoryA", createdSample.Category);
        
        // Verify it was actually saved to database
        var allSamples = await SampleRepository.GetAsync();
        Assert.Contains(allSamples, s => s.Name == "New Sample");
    }

    [Fact]
    public async Task Create_ShouldReturnBadRequest_WithInvalidData()
    {
        // Arrange - Empty name
        var request = new { name = "", description = "Description", category = "CategoryA" };
        
        // Act
        var response = await Client.PostAsJsonAsync("/api/samples", request);
        
        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    #endregion

    #region PUT Tests

    [Fact]
    public async Task Update_ShouldUpdateSample_WhenExists()
    {
        // Arrange
        var sample = await SampleRepository.CreateAsync(
            new Sample("Original Name", "Original description", "CategoryA")
        );
        var updateRequest = new { name = "Updated Name", description = "Updated description", category = "CategoryB" };
        
        // Act
        var response = await Client.PutAsJsonAsync($"/api/samples/{sample.Id}", updateRequest);
        
        // Assert
        response.EnsureSuccessStatusCode();
        
        var updated = await SampleRepository.GetFirstOrDefaultAsync(s => s.Id == sample.Id);
        Assert.NotNull(updated);
        Assert.Equal("Updated Name", updated.Name);
        Assert.Equal("Updated description", updated.Description);
        Assert.Equal("CategoryB", updated.Category);
    }

    [Fact]
    public async Task Update_ShouldReturnNotFound_WhenNotExists()
    {
        // Arrange
        var updateRequest = new { name = "Updated", description = "Description", category = "CategoryA" };
        
        // Act
        var response = await Client.PutAsJsonAsync("/api/samples/999999", updateRequest);
        
        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    #endregion

    #region DELETE Tests

    [Fact]
    public async Task Delete_ShouldRemoveSample_WhenExists()
    {
        // Arrange
        var sample = await SampleRepository.CreateAsync(
            new Sample("To Delete", "Will be deleted", "CategoryA")
        );
        
        // Act
        var response = await Client.DeleteAsync($"/api/samples/{sample.Id}");
        
        // Assert
        response.EnsureSuccessStatusCode();
        
        var deleted = await SampleRepository.GetFirstOrDefaultAsync(s => s.Id == sample.Id);
        Assert.Null(deleted);
    }

    [Fact]
    public async Task Delete_ShouldReturnNotFound_WhenNotExists()
    {
        // Act
        var response = await Client.DeleteAsync("/api/samples/999999");
        
        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    #endregion

    #region Direct Repository Tests

    [Fact]
    public async Task DirectRepositoryAccess_ShouldWorkInTests()
    {
        // This test demonstrates direct repository usage within tests
        // Useful for seeding data or verifying database state
        
        // Arrange & Act
        var sample = await SampleRepository.CreateAsync(
            new Sample("Direct Access", "Direct access test", "CategoryA")
        );

        // Assert
        Assert.NotNull(sample);
        Assert.NotEqual(0, sample.Id);
        
        var allSamples = await SampleRepository.GetAsync();
        Assert.Contains(allSamples, s => s.Name == "Direct Access");
    }

    [Fact]
    public async Task FilterByCategory_ShouldReturnMatchingSamples()
    {
        // Arrange
        await SampleRepository.CreateAsync(new Sample("Cat A 1", "Description", "CategoryA"));
        await SampleRepository.CreateAsync(new Sample("Cat A 2", "Description", "CategoryA"));
        await SampleRepository.CreateAsync(new Sample("Cat B 1", "Description", "CategoryB"));

        // Act
        var categoryASamples = await SampleRepository.GetAsync(s => s.Category == "CategoryA");
        
        // Assert
        Assert.NotNull(categoryASamples);
        Assert.True(categoryASamples.Count >= 2);
        Assert.All(categoryASamples, s => Assert.Equal("CategoryA", s.Category));
    }

    #endregion
}