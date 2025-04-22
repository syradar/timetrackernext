using TimeTracking.Application.Models;

namespace TimeTracking.Application.Repositories;

public interface ITimeEntryRepository
{
    Task<bool> CreateAsync(TimeEntry entry, CancellationToken token = default);

    Task<TimeEntry?> GetByIdAsync(Guid id, CancellationToken token = default);

    Task<IEnumerable<TimeEntry>> GetAllAsync(CancellationToken token = default);

    Task<bool> UpdateAsync(TimeEntry entry, CancellationToken token = default);

    Task<bool> DeleteByIdAsync(Guid id, CancellationToken token = default);

    Task<bool> ExistsByIdAsync(Guid id, CancellationToken token = default);
}