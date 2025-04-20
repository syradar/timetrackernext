namespace TimeTracking.Contracts.Requests;

public class TimeEntryResponse
{
    public required Guid Id { get; init; }

    public required string Description { get; init; }

    public required decimal Hours { get; init; }

    public required IEnumerable<string> Tags { get; init; } = [];
}