using FluentValidation;
using TimeTracking.Application.Models;
using TimeTracking.Application.Repositories;

namespace TimeTracking.Application.Services;

public class TimeEntryService : ITimeEntryService
{
    private readonly ITimeEntryRepository _timeEntryRepository;
    private readonly IValidator<TimeEntry> _timeEntryValidator;

    public TimeEntryService(ITimeEntryRepository timeEntryRepository, IValidator<TimeEntry> timeEntryValidator)
    {
        _timeEntryRepository = timeEntryRepository;
        _timeEntryValidator = timeEntryValidator;
    }

    public async Task<bool> CreateAsync(TimeEntry entry, CancellationToken token = default)
    {
        await _timeEntryValidator.ValidateAndThrowAsync(entry, token);
        return await _timeEntryRepository.CreateAsync(entry, token);
    }

    public Task<TimeEntry?> GetByIdAsync(Guid id, CancellationToken token = default)
    {
        return _timeEntryRepository.GetByIdAsync(id, token);
    }

    public async Task<TimeEntry?> UpdateAsync(TimeEntry entry, CancellationToken token = default)
    {
        await _timeEntryValidator.ValidateAndThrowAsync(entry, token);
        var timeEntryExists = await _timeEntryRepository.ExistsByIdAsync(entry.Id, token);

        if (!timeEntryExists)
        {
            return null;
        }

        await _timeEntryRepository.UpdateAsync(entry, token);
        return entry;
    }

    public Task<bool> DeleteByIdAsync(Guid id, CancellationToken token = default)
    {
        return _timeEntryRepository.DeleteByIdAsync(id, token);
    }

    public Task<IEnumerable<TimeEntry>> GetAllAsync(CancellationToken token = default)
    {
        return _timeEntryRepository.GetAllAsync(token);
    }
}