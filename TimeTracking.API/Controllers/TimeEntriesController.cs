using Microsoft.AspNetCore.Mvc;
using TimeTracking.API.Mapping;
using TimeTracking.Application.Services;
using TimeTracking.Contracts.Requests;

namespace TimeTracking.API.Controllers;

[ApiController]
public class TimeEntriesController : ControllerBase
{
    private readonly ITimeEntryService _timeEntryService;

    public TimeEntriesController(ITimeEntryService timeEntryService)
    {
        _timeEntryService = timeEntryService;
    }

    [HttpPost(ApiEndpoints.TimeEntries.Create)]
    public async Task<IActionResult> Create([FromBody] CreateTimeEntryRequest request)
    {
        var timeEntry = request.MapToTimeEntry();

        await _timeEntryService.CreateAsync(timeEntry);

        return CreatedAtAction(nameof(Get), new { id = timeEntry.Id }, timeEntry.MapToResponse());
    }

    [HttpGet(ApiEndpoints.TimeEntries.Get)]
    public async Task<IActionResult> Get([FromRoute] Guid id)
    {
        var timeEntry = await _timeEntryService.GetByIdAsync(id);

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
        var timeEntries = await _timeEntryService.GetAllAsync();
        var response = timeEntries.MapToResponse();
        return Ok(response);
    }

    [HttpPut(ApiEndpoints.TimeEntries.Update)]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateTimeEntryRequest request)
    {
        var timeEntry = request.MapToTimeEntry(id);

        var updatedTimeEntry = await _timeEntryService.UpdateAsync(timeEntry);

        if (updatedTimeEntry is null)
        {
            return NotFound();
        }

        var response = timeEntry.MapToResponse();

        return Ok(response);
    }

    [HttpDelete(ApiEndpoints.TimeEntries.Delete)]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        var deleted = await _timeEntryService.DeleteByIdAsync(id);

        if (!deleted)
        {
            return NotFound();
        }

        return Ok();
    }
}