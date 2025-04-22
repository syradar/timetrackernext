using TimeTracking.Application.Models;

namespace TimeTracking.Application.Services;

public interface IClientService
{
    Task<bool> CreateAsync(Client client);

    Task<Client?> GetByIdAsync(Guid id);

    Task<Client?> GetBySlugAsync(string slug);

    Task<IEnumerable<Client>> GetAllAsync();

    Task<Client?> UpdateAsync(Client client);

    Task<bool> DeleteByIdAsync(Guid id);
}