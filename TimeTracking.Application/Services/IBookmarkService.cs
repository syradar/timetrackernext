using TimeTracking.Application.Models;

namespace TimeTracking.Application.Services;

public interface IBookmarkService
{
    Task<bool> BookmarkTimeEntryAsync(Guid timeEntryId, Guid userId, bool bookmark, CancellationToken token = default);

    public Task<bool> DeleteBookmarkAsync(Guid timeEntryId, Guid userId, CancellationToken token = default);

    public Task<IEnumerable<TimeEntryBookmark>>
        GetBookmarksForUserAsync(Guid userId, CancellationToken token = default);
}