using TimeTracking.Application.Models;

namespace TimeTracking.Application.Repositories;

public class TimeEntryRepository : ITimeEntryRepository
{
    private readonly List<TimeEntry> _timeEntries = [];

    public Task<bool> CreateAsync(TimeEntry entry)
    {
        _timeEntries.Add(entry);
        return Task.FromResult(true);
    }

    public Task<TimeEntry?> GetByIdAsync(Guid id)
    {
        var entry = _timeEntries.FirstOrDefault(x => x.Id == id);
        return Task.FromResult(entry);
    }

    public Task<IEnumerable<TimeEntry>> GetAllAsync()
    {
        return Task.FromResult(_timeEntries.AsEnumerable());
    }

    public Task<bool> UpdateAsync(TimeEntry entry)
    {
        var timeEntryIndex = _timeEntries.FindIndex(x => x.Id == entry.Id);

        if (timeEntryIndex == -1)
        {
            return Task.FromResult(false);
        }

        _timeEntries[timeEntryIndex] = entry;
        return Task.FromResult(true);
    }

    public Task<bool> DeleteByIdAsync(Guid id)
    {
        var removedCount = _timeEntries.RemoveAll(x => x.Id == id);
        var timeEntriesRemoved = removedCount > 0;
        return Task.FromResult(timeEntriesRemoved);
    }
}