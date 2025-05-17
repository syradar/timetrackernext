using TimeTracking.Application.Models;
using TimeTracking.Application.Repositories;

namespace TimeTracking.Application.Services;

public class BookmarkService : IBookmarkService
{
    private readonly IBookmarkRepository _bookmarkRepository;
    private readonly ITimeEntryRepository _timeEntryRepository;

    public BookmarkService(IBookmarkRepository bookmarkRepository, ITimeEntryRepository timeEntryRepository)
    {
        _bookmarkRepository = bookmarkRepository;
        _timeEntryRepository = timeEntryRepository;
    }

    public async Task<bool> BookmarkTimeEntryAsync(Guid timeEntryId, Guid userId, bool bookmark,
        CancellationToken token = default)
    {
        var timeEntryExists = await _timeEntryRepository.ExistsByIdAsync(timeEntryId, token);

        if (!timeEntryExists)
        {
            return false;
        }

        var result = await _bookmarkRepository.BookmarkTimeEntryAsync(
            timeEntryId,
            userId,
            bookmark,
            token);

        return result;
    }

    public Task<bool> DeleteBookmarkAsync(Guid timeEntryId, Guid userId, CancellationToken token = default)
    {
        return _bookmarkRepository.DeleteBookmarkAsync(timeEntryId, userId, token);
    }

    public Task<IEnumerable<TimeEntryBookmark>> GetBookmarksForUserAsync(Guid userId, CancellationToken token = default)
    {
        return _bookmarkRepository.GetBookmarksForUserAsync(userId, token);
    }
}