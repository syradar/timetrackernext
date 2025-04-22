namespace TimeTracking.Application.Models;

public class TimeEntry
{
    public required Guid Id { get; init; }

    public required DateTime Date { get; init; }

    public required string Description { get; set; }

    public required decimal Hours { get; set; }

    public required List<string> Comments { get; init; } = [];
}