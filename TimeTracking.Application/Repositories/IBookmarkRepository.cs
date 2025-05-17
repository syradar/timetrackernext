using TimeTracking.Application.Models;

namespace TimeTracking.Application.Repositories;

public interface IBookmarkRepository
{
    Task<bool> BookmarkTimeEntryAsync(Guid timeEntryId, Guid userId, bool bookmark, CancellationToken token = default);

    Task<int?> GetBookmarkCountAsync(Guid timeEntryId, CancellationToken token = default);

    Task<(int? BookmarkCount, bool? IsBookmarkedByCurrentUser)> GetBookmarkAsync(Guid timeEntryId, Guid userId,
        CancellationToken token = default);
    
    Task<bool> DeleteBookmarkAsync(Guid timeEntryId, Guid userId, CancellationToken token = default);

    Task<IEnumerable<TimeEntryBookmark>> GetBookmarksForUserAsync(Guid userId, CancellationToken token = default);
}