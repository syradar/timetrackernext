using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TimeTracking.API.Auth;
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

    [Authorize(AuthConstants.TrustedUserPolicyName)]
    [HttpPost(ApiEndpoints.TimeEntries.Create)]
    public async Task<IActionResult> Create([FromBody] CreateTimeEntryRequest request, CancellationToken token)
    {
        var timeEntry = request.MapToTimeEntry();

        await _timeEntryService.CreateAsync(timeEntry, token);

        return CreatedAtAction(nameof(Get), new { id = timeEntry.Id }, timeEntry.MapToResponse());
    }

    [HttpGet(ApiEndpoints.TimeEntries.Get)]
    public async Task<IActionResult> Get([FromRoute] Guid id, CancellationToken token)
    {
        var usedId = HttpContext.GetUserId();
        var timeEntry = await _timeEntryService.GetByIdAsync(id, usedId, token);

        if (timeEntry is null)
        {
            return NotFound();
        }

        var response = timeEntry.MapToResponse();
        return Ok(response);
    }

    [HttpGet(ApiEndpoints.TimeEntries.GetAll)]
    public async Task<IActionResult> GetAll(CancellationToken token)
    {
        var userId = HttpContext.GetUserId();
        var timeEntries = await _timeEntryService.GetAllAsync(userId, token);
        var response = timeEntries.MapToResponse();
        return Ok(response);
    }

    [Authorize(AuthConstants.TrustedUserPolicyName)]
    [HttpPut(ApiEndpoints.TimeEntries.Update)]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateTimeEntryRequest request,
        CancellationToken token)
    {
        var userId = HttpContext.GetUserId();

        var timeEntry = request.MapToTimeEntry(id);

        var updatedTimeEntry = await _timeEntryService.UpdateAsync(timeEntry, userId, token);

        if (updatedTimeEntry is null)
        {
            return NotFound();
        }

        var response = timeEntry.MapToResponse();

        return Ok(response);
    }

    [Authorize(AuthConstants.AdminUserPolicyName)]
    [HttpDelete(ApiEndpoints.TimeEntries.Delete)]
    public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken token)
    {
        var deleted = await _timeEntryService.DeleteByIdAsync(id, token);

        if (!deleted)
        {
            return NotFound();
        }

        return Ok();
    }
}