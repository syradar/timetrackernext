namespace TimeTracking.Application.Repositories;

public interface IBookmarkRepository
{
    Task<bool> BookmarkTimeEntryAsync(Guid timeEntryId, bool bookmark, Guid userId, CancellationToken token = default);

    Task<int?> GetBookmarkCountAsync(Guid timeEntryId, CancellationToken token = default);

    Task<(int? BookmarkCount, bool? IsBookmarkedByCurrentUser)> GetBookmarkAsync(Guid timeEntryId, Guid userId,
        CancellationToken token = default);
}