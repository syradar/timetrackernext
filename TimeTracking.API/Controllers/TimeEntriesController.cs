using Microsoft.AspNetCore.Mvc;
using TimeTracking.API.Mapping;
using TimeTracking.Application.Repositories;
using TimeTracking.Contracts.Requests;

namespace TimeTracking.API.Controllers;

[ApiController]
public class TimeEntriesController : ControllerBase
{
    private readonly ITimeEntryRepository _timeEntryRepository;

    public TimeEntriesController(ITimeEntryRepository timeEntryRepository)
    {
        _timeEntryRepository = timeEntryRepository;
    }

    [HttpPost(ApiEndpoints.TimeEntries.Create)]
    public async Task<IActionResult> Create([FromBody] CreateTimeEntryRequest request)
    {
        var timeEntry = request.MapToTimeEntry();

        await _timeEntryRepository.CreateAsync(timeEntry);

        return CreatedAtAction(nameof(Get), new { id = timeEntry.Id }, timeEntry);
    }

    [HttpGet(ApiEndpoints.TimeEntries.Get)]
    public async Task<IActionResult> Get([FromRoute] Guid id)
    {
        var timeEntry = await _timeEntryRepository.GetByIdAsync(id);

        if (timeEntry is null)
        {
            return NotFound();
        }

        var response = timeEntry.MapToResponse();
        return Ok(response);
    }

    [HttpGet(ApiEndpoints.TimeEntries.GetAll)]
    public async Task<IActionResult> GetAll()
    {
        var timeEntries = await _timeEntryRepository.GetAllAsync();
        var response = timeEntries.MapToResponse();
        return Ok(response);
    }

    [HttpPut(ApiEndpoints.TimeEntries.Update)]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateTimeEntryRequest request)
    {
        var timeEntry = request.MapToTimeEntry(id);

        var updated = await _timeEntryRepository.UpdateAsync(timeEntry);

        if (!updated)
        {
            return NotFound();
        }

        var response = timeEntry.MapToResponse();

        return Ok(response);
    }

    [HttpDelete(ApiEndpoints.TimeEntries.Delete)]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        var deleted = await _timeEntryRepository.DeleteByIdAsync(id);

        if (!deleted)
        {
            return NotFound();
        }

        return Ok();
    }
}