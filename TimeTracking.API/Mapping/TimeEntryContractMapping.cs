using TimeTracking.Application.Models;
using TimeTracking.Contracts.Requests;
using TimeTracking.Contracts.Responses;

namespace TimeTracking.API.Mapping;

public static partial class TimeEntryContractMapping
{
    public static TimeEntry MapToTimeEntry(this CreateTimeEntryRequest request)
    {
        return new TimeEntry
        {
            Date = request.Date,
            Description = request.Description,
            Hours = request.Hours,
            Comments = request.Comments.ToList(),
            Id = Guid.NewGuid(),
        };
    }

    public static TimeEntry MapToTimeEntry(this UpdateTimeEntryRequest request, Guid id)
    {
        return new TimeEntry
        {
            Date = request.Date,
            Description = request.Description,
            Hours = request.Hours,
            Comments = request.Comments.ToList(),
            Id = id,
        };
    }

    public static TimeEntryResponse MapToResponse(this TimeEntry entry)
    {
        return new TimeEntryResponse
        {
            Id = entry.Id,
            Description = entry.Description,
            Hours = entry.Hours,
            Comments = entry.Comments,
            Date = entry.Date,
            BookmarkCount = entry.BookmarkCount,
            IsBookmarked = entry.IsBookmarkedByCurrentUser,
        };
    }

    public static TimeEntriesResponse MapToResponse(this IEnumerable<TimeEntry> entries)
    {
        return new TimeEntriesResponse
        {
            Items = entries.Select(x => x.MapToResponse()),
        };
    }
}