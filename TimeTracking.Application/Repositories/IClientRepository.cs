using TimeTracking.Application.Models;

namespace TimeTracking.Application.Repositories;

public interface IClientRepository
{
    Task<bool> CreateAsync(Client client, CancellationToken token = default);

    Task<Client?> GetByIdAsync(Guid id, CancellationToken token = default);

    Task<Client?> GetBySlugAsync(string slug, CancellationToken token = default);

    Task<IEnumerable<Client>> GetAllAsync(CancellationToken token = default);

    Task<bool> UpdateAsync(Client client, CancellationToken token = default);

    Task<bool> DeleteByIdAsync(Guid id, CancellationToken token = default);

    Task<bool> ExistsByIdAsync(Guid id, CancellationToken token = default);
}