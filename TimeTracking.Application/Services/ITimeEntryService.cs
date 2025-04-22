using TimeTracking.Application.Models;

namespace TimeTracking.Application.Services;

public interface ITimeEntryService
{
    Task<bool> CreateAsync(TimeEntry entry);

    Task<TimeEntry?> GetByIdAsync(Guid id);

    Task<IEnumerable<TimeEntry>> GetAllAsync();

    Task<TimeEntry?> UpdateAsync(TimeEntry entry);

    Task<bool> DeleteByIdAsync(Guid id);
}