namespace TimeTracking.Contracts.Responses;

public class ClientResponse
{
    public required Guid Id { get; init; }

    public required string Name { get; init; }

    public required string Slug { get; init; }
}