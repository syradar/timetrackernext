using FluentValidation;
using TimeTracking.Application.Models;
using TimeTracking.Application.Repositories;

namespace TimeTracking.Application.Services;

public class TimeEntryService : ITimeEntryService
{
    private readonly IBookmarkRepository _bookmarkRepository;
    private readonly ITimeEntryRepository _timeEntryRepository;
    private readonly IValidator<TimeEntry> _timeEntryValidator;

    public TimeEntryService(ITimeEntryRepository timeEntryRepository, IValidator<TimeEntry> timeEntryValidator,
        IBookmarkRepository bookmarkRepository)
    {
        _timeEntryRepository = timeEntryRepository;
        _timeEntryValidator = timeEntryValidator;
        _bookmarkRepository = bookmarkRepository;
    }

    public async Task<bool> CreateAsync(TimeEntry entry, CancellationToken token = default)
    {
        await _timeEntryValidator.ValidateAndThrowAsync(entry, token);
        return await _timeEntryRepository.CreateAsync(entry, token);
    }

    public Task<TimeEntry?> GetByIdAsync(Guid id, Guid? userId = default, CancellationToken token = default)
    {
        return _timeEntryRepository.GetByIdAsync(id, userId, token);
    }

    public async Task<TimeEntry?> UpdateAsync(TimeEntry entry, Guid? userId = default,
        CancellationToken token = default)
    {
        await _timeEntryValidator.ValidateAndThrowAsync(entry, token);
        var timeEntryExists = await _timeEntryRepository.ExistsByIdAsync(entry.Id, token);

        if (!timeEntryExists)
        {
            return null;
        }

        await _timeEntryRepository.UpdateAsync(entry, token);

        if (!userId.HasValue)
        {
            var bookmarkCount = await _bookmarkRepository.GetBookmarkCountAsync(entry.Id, token);
            entry.BookmarkCount = bookmarkCount;
            return entry;
        }

        var bookmarks = await _bookmarkRepository.GetBookmarkAsync(entry.Id, userId.Value, token);
        entry.BookmarkCount = bookmarks.BookmarkCount;
        entry.IsBookmarkedByCurrentUser = bookmarks.IsBookmarkedByCurrentUser;

        return entry;
    }

    public Task<bool> DeleteByIdAsync(Guid id, CancellationToken token = default)
    {
        return _timeEntryRepository.DeleteByIdAsync(id, token);
    }

    public Task<IEnumerable<TimeEntry>> GetAllAsync(Guid? userId = default, CancellationToken token = default)
    {
        return _timeEntryRepository.GetAllAsync(userId, token);
    }
}