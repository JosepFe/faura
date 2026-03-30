using Faura.WebAPI.Application;
using Faura.WebAPI.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Faura.WebAPI.Controllers;

/// <summary>
/// API Controller for Sample entity operations.
/// Demonstrates RESTful API design with CRUD operations.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class SamplesController(ISampleService sampleService) : ControllerBase
{
    /// <summary>
    /// Gets all samples.
    /// </summary>
    [HttpGet(Name = "GetSamples")]
    [ProducesResponseType(typeof(IEnumerable<Sample>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetAll()
    {
        var samples = await sampleService.GetSamplesAsync();
        return Ok(samples);
    }

    /// <summary>
    /// Gets a sample by ID.
    /// </summary>
    [HttpGet("{id}", Name = "GetSampleById")]
    [ProducesResponseType(typeof(Sample), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> GetById(long id)
    {
        var sample = await sampleService.GetSampleByIdAsync(id);
        if (sample == null)
            return NotFound();

        return Ok(sample);
    }

    /// <summary>
    /// Creates a new sample.
    /// </summary>
    [HttpPost(Name = "CreateSample")]
    [ProducesResponseType(typeof(Sample), (int)HttpStatusCode.Created)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateSampleRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
            return BadRequest("Name is required");

        var sample = await sampleService.CreateSampleAsync(
            request.Name,
            request.Description,
            request.Category
        );

        return CreatedAtRoute("GetSampleById", new { id = sample.Id }, sample);
    }

    /// <summary>
    /// Updates an existing sample.
    /// </summary>
    [HttpPut("{id}", Name = "UpdateSample")]
    [ProducesResponseType(typeof(Sample), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> Update(long id, [FromBody] UpdateSampleRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
            return BadRequest("Name is required");

        var sample = await sampleService.UpdateSampleAsync(
            id,
            request.Name,
            request.Description,
            request.Category
        );

        if (sample == null)
            return NotFound();

        return Ok(sample);
    }

    /// <summary>
    /// Deletes a sample.
    /// </summary>
    [HttpDelete("{id}", Name = "DeleteSample")]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> Delete(long id)
    {
        var deleted = await sampleService.DeleteSampleAsync(id);
        if (!deleted)
            return NotFound();

        return NoContent();
    }

    /// <summary>
    /// Creates multiple samples within a transaction.
    /// If one fails, all are rolled back (demonstrates transaction behavior).
    /// </summary>
    [HttpPost("multiple", Name = "CreateMultipleSamples")]
    [ProducesResponseType(typeof(IEnumerable<Sample>), (int)HttpStatusCode.Created)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> CreateMultiple([FromBody] CreateMultipleSamplesRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Name1) || string.IsNullOrWhiteSpace(request.Name2))
            return BadRequest("Both names are required");

        var samples = await sampleService.CreateMultipleSamplesWithTransactionAsync(
            request.Name1,
            request.Description1,
            request.Category1,
            request.Name2,
            request.Description2,
            request.Category2
        );

        return Created(string.Empty, samples);
    }
}

/// <summary>
/// Request model for creating a sample.
/// </summary>
public record CreateSampleRequest(string Name, string Description, string Category);

/// <summary>
/// Request model for updating a sample.
/// </summary>
public record UpdateSampleRequest(string Name, string Description, string Category);

/// <summary>
/// Request model for creating multiple samples in a transaction.
/// </summary>
public record CreateMultipleSamplesRequest(
    string Name1,
    string Description1,
    string Category1,
    string Name2,
    string Description2,
    string Category2
);
