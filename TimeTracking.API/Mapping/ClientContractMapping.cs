using TimeTracking.Application.Models;
using TimeTracking.Contracts.Requests;
using TimeTracking.Contracts.Responses;

namespace TimeTracking.API.Mapping;

public static partial class TimeEntryContractMapping
{
    public static Client MapToClient(this CreateClientRequest request)
    {
        return new Client
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
        };
    }

    public static Client MapToClient(this UpdateClientRequest request, Guid id)
    {
        return new Client
        {
            Id = id,
            Name = request.Name,
        };
    }

    public static ClientResponse MapToResponse(this Client client)
    {
        return new ClientResponse
        {
            Id = client.Id,
            Name = client.Name,
            Slug = client.Slug,
        };
    }

    public static ClientsResponse MapToResponse(this IEnumerable<Client> clients)
    {
        return new ClientsResponse
        {
            Items = clients.Select(c => c.MapToResponse()),
        };
    }
}