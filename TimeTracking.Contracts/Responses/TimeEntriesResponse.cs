namespace TimeTracking.Contracts.Requests;

public class TimeEntriesResponse
{
    public required IEnumerable<TimeEntryResponse> Items { get; init; } = [];
}