using TimeTracking.Application.Models;

namespace TimeTracking.Application.Repositories;

public interface ITimeEntryRepository
{
    Task<bool> CreateAsync(TimeEntry entry);

    Task<TimeEntry?> GetByIdAsync(Guid id);

    Task<IEnumerable<TimeEntry>> GetAllAsync();

    Task<bool> UpdateAsync(TimeEntry entry);

    Task<bool> DeleteByIdAsync(Guid id);
}