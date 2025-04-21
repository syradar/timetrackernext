using TimeTracking.Application.Models;

namespace TimeTracking.Application.Repositories;

public class ClientRepository : IClientRepository
{
    private readonly List<Client> _clients = new();

    public Task<bool> CreateAsync(Client client)
    {
        _clients.Add(client);
        return Task.FromResult(true);
    }

    public Task<Client?> GetByIdAsync(Guid id)
    {
        var client = _clients.FirstOrDefault(c => c.Id == id);
        return Task.FromResult(client);
    }

    public Task<Client?> GetBySlugAsync(string slug)
    {
        var client = _clients.FirstOrDefault(c => c.Slug == slug);
        return Task.FromResult(client);
    }

    public Task<IEnumerable<Client>> GetAllAsync()
    {
        return Task.FromResult<IEnumerable<Client>>(_clients);
    }

    public Task<bool> UpdateAsync(Client client)
    {
        var idx = _clients.FindIndex(c => c.Id == client.Id);

        if (idx < 0)
        {
            return Task.FromResult(false);
        }

        _clients[idx] = client;
        return Task.FromResult(true);
    }

    public Task<bool> DeleteByIdAsync(Guid id)
    {
        var removed = _clients.RemoveAll(c => c.Id == id) > 0;
        return Task.FromResult(removed);
    }
}