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

    public async Task<bool> CreateAsync(TimeEntry entry)
    {
        await _timeEntryValidator.ValidateAndThrowAsync(entry);
        return await _timeEntryRepository.CreateAsync(entry);
    }

    public Task<TimeEntry?> GetByIdAsync(Guid id)
    {
        return _timeEntryRepository.GetByIdAsync(id);
    }

    public Task<IEnumerable<TimeEntry>> GetAllAsync()
    {
        return _timeEntryRepository.GetAllAsync();
    }

    public async Task<TimeEntry?> UpdateAsync(TimeEntry entry)
    {
        await _timeEntryValidator.ValidateAndThrowAsync(entry);
        var timeEntryExists = await _timeEntryRepository.ExistsByIdAsync(entry.Id);

        if (!timeEntryExists)
        {
            return null;
        }

        await _timeEntryRepository.UpdateAsync(entry);
        return entry;
    }

    public Task<bool> DeleteByIdAsync(Guid id)
    {
        return _timeEntryRepository.DeleteByIdAsync(id);
    }
}