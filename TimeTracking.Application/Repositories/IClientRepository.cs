using TimeTracking.Application.Models;

namespace TimeTracking.Application.Repositories;

public interface IClientRepository
{
    Task<bool> CreateAsync(Client client);

    Task<Client?> GetByIdAsync(Guid id);

    Task<Client?> GetBySlugAsync(string slug);

    Task<IEnumerable<Client>> GetAllAsync();

    Task<bool> UpdateAsync(Client client);

    Task<bool> DeleteByIdAsync(Guid id);
}