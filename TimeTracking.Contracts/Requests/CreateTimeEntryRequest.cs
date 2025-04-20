namespace TimeTracking.Contracts.Requests;

public class CreateTimeEntryRequest
{
    public required string Description { get; init; }

    public required DateTime Date { get; init; }

    public required decimal Hours { get; init; }

    public required IEnumerable<string> Tags { get; init; } = [];
}