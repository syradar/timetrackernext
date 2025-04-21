namespace TimeTracking.Contracts.Responses;

public class ClientsResponse
{
    public IEnumerable<ClientResponse> Items { get; init; } = [];
}