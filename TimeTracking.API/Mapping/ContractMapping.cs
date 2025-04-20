using TimeTracking.Application.Models;
using TimeTracking.Contracts.Requests;

namespace TimeTracking.API.Mapping;

public static class ContractMapping
{
    public static TimeEntry MapToTimeEntry(this CreateTimeEntryRequest request)
    {
        return new TimeEntry
        {
            Date = request.Date,
            Description = request.Description,
            Hours = request.Hours,
            Tags = request.Tags.ToList(),
            Id = Guid.NewGuid()
        };
    }

    public static TimeEntry MapToTimeEntry(this UpdateTimeEntryRequest request, Guid id)
    {
        return new TimeEntry
        {
            Date = request.Date,
            Description = request.Description,
            Hours = request.Hours,
            Tags = request.Tags.ToList(),
            Id = id
        };
    }

    public static TimeEntryResponse MapToResponse(this TimeEntry entry)
    {
        return new TimeEntryResponse
        {
            Id = entry.Id,
            Description = entry.Description,
            Hours = entry.Hours,
            Tags = entry.Tags
        };
    }

    public static TimeEntriesResponse MapToResponse(this IEnumerable<TimeEntry> entries)
    {
        return new TimeEntriesResponse
        {
            Items = entries.Select(x => x.MapToResponse())
        };
    }
}