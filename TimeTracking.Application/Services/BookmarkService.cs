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

    public async Task<bool> BookmarkTimeEntryAsync(Guid userId, bool bookmark, Guid timeEntryId,
        CancellationToken token = default)
    {
        // if (!bookmark)
        // {
        //     throw new ValidationException([
        //         new ValidationFailure
        //         {
        //             PropertyName = "Bookmark",
        //             ErrorMessage = "You can't unbookmark a time entry",
        //         },
        //     ]);
        // }

        var timeEntryExists = await _timeEntryRepository.ExistsByIdAsync(timeEntryId, token);

        if (!timeEntryExists)
        {
            return false;
        }

        var result = await _bookmarkRepository.BookmarkTimeEntryAsync(
            timeEntryId,
            bookmark,
            userId,
            token);

        if (!bookmark)
        {
            return result;
        }

        return result;
    }
}