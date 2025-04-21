namespace TimeTracking.Contracts.Responses;

public class TimeEntriesResponse
{
    public required IEnumerable<TimeEntryResponse> Items { get; init; } = [];
}