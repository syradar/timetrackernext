namespace TimeTracking.Application.Services;

public interface IBookmarkService
{
    Task<bool> BookmarkTimeEntryAsync(Guid userId, bool bookmark, Guid timeEntryId, CancellationToken token = default);
}