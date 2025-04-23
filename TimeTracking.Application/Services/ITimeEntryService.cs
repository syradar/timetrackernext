using TimeTracking.Application.Models;

namespace TimeTracking.Application.Services;

public interface ITimeEntryService
{
    Task<bool> CreateAsync(TimeEntry entry, CancellationToken token = default);

    Task<TimeEntry?> GetByIdAsync(Guid id, Guid? userId = default, CancellationToken token = default);

    Task<IEnumerable<TimeEntry>> GetAllAsync(Guid? userId = default, CancellationToken token = default);

    Task<TimeEntry?> UpdateAsync(TimeEntry entry, Guid? userId = default, CancellationToken token = default);

    Task<bool> DeleteByIdAsync(Guid id, CancellationToken token = default);
}