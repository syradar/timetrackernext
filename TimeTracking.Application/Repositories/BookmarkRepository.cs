using Dapper;
using TimeTracking.Application.Database;

namespace TimeTracking.Application.Repositories;

public class BookmarkRepository : IBookmarkRepository
{
    private readonly IDbConnectionFactory _dbConnectionFactory;

    public BookmarkRepository(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }

    public async Task<int?> GetBookmarkCountAsync(Guid timeEntryId, CancellationToken token = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync(token);

        return await connection.QuerySingleOrDefaultAsync<int?>(new CommandDefinition(
            """
                select count(*) 
                from bookmarks 
                where timeEntryId = @timeEntryId
            """, new { timeEntryId }, cancellationToken: token));
    }

    public async Task<(int? BookmarkCount, bool? IsBookmarkedByCurrentUser)> GetBookmarkAsync(Guid timeEntryId, Guid
            userId,
        CancellationToken token = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync(token);

        return await connection.QuerySingleOrDefaultAsync<(int, bool)>(new CommandDefinition(
            """
            select 
                -- total count
                (select count(*)
                from bookmarks
                where timeEntryId = @timeEntryId
                ) as bookmarkcount,
                -- is bookmarked by the current user
                exists (
                select 1 
                from bookmarks 
                where timeEntryId = @timeEntryId 
                  and userid = @userId
                ) as isbookmarkedbycurrentuser
            """, new { timeEntryId, userId }, cancellationToken: token));
    }

    public async Task<bool> BookmarkTimeEntryAsync(Guid timeEntryId, bool bookmark, Guid userId,
        CancellationToken token = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync(token);

        if (bookmark)
        {
            await connection.ExecuteAsync(new CommandDefinition(
                """
                insert into bookmarks (userid, timeEntryId)
                values (@userId, @timeEntryId)
                on conflict (userid, timeEntryId) do nothing;
                """,
                new { userId, timeEntryId }, cancellationToken: token));

            return true;
        }

        var deleteResult = await connection.ExecuteAsync(new CommandDefinition(
            """
            delete from bookmarks
            where userid = @userId 
                and timeEntryId = @timeEntryId;
            """, new { userId, timeEntryId }, cancellationToken: token));

        return deleteResult > 0;
    }
}