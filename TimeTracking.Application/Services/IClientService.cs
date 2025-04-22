using TimeTracking.Application.Models;

namespace TimeTracking.Application.Services;

public interface IClientService
{
    Task<bool> CreateAsync(Client client, CancellationToken token = default);

    Task<Client?> GetByIdAsync(Guid id, CancellationToken token = default);

    Task<Client?> GetBySlugAsync(string slug, CancellationToken token = default);

    Task<IEnumerable<Client>> GetAllAsync(CancellationToken token = default);

    Task<Client?> UpdateAsync(Client client, CancellationToken token = default);

    Task<bool> DeleteByIdAsync(Guid id, CancellationToken token = default);
}