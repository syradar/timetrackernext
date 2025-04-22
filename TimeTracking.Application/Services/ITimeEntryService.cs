using TimeTracking.Application.Models;

namespace TimeTracking.Application.Services;

public interface ITimeEntryService
{
    Task<bool> CreateAsync(TimeEntry entrym, CancellationToken token = default);

    Task<TimeEntry?> GetByIdAsync(Guid id, CancellationToken token = default);

    Task<IEnumerable<TimeEntry>> GetAllAsync(CancellationToken token = default);

    Task<TimeEntry?> UpdateAsync(TimeEntry entry, CancellationToken token = default);

    Task<bool> DeleteByIdAsync(Guid id, CancellationToken token = default);
}