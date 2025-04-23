using Dapper;
using TimeTracking.Application.Database;
using TimeTracking.Application.Models;

namespace TimeTracking.Application.Repositories;

public class TimeEntryRepository : ITimeEntryRepository
{
    private readonly IDbConnectionFactory _dbConnectionFactory;

    public TimeEntryRepository(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }

    public async Task<bool> CreateAsync(TimeEntry entry, CancellationToken token = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync(token);
        using var transaction = connection.BeginTransaction();

        var result = await connection.ExecuteAsync(new CommandDefinition(
            """
            insert into time_entries (id, description, date, hours)
            values (@Id, @Description, @Date, @Hours)
            """, entry, cancellationToken: token));

        if (result > 0)
        {
            foreach (var comment in entry.Comments)
            {
                await connection.ExecuteAsync(new CommandDefinition(
                    """
                    insert into comments (timeEntryId, comment)
                    values (@TimeEntryId, @Comment)
                    """, new { TimeEntryId = entry.Id, comment }, cancellationToken: token));
            }
        }

        transaction.Commit();

        return result > 0;
    }

    public async Task<TimeEntry?> GetByIdAsync(Guid id, Guid? userId = default, CancellationToken token = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync(token);

        var timeEntryRaw = await connection.QuerySingleOrDefaultAsync<TimeEntryRaw>(
            new CommandDefinition(
                """
                select 
                    te.id,
                    te.description,
                    te.date,
                    te.hours,
                   -- comments
                (
                    select string_agg(c.comment, ',') 
                    from comments c
                    where c.timeEntryId = te.id
                ) as comments,
                -- bookmark count
                (
                    select count(*)
                    from bookmarks b
                    where b.timeEntryId = te.id
                ) as bookmarkcount,
                -- is bookmarked by the current user
                exists (
                    select 1 
                    from bookmarks b2
                    where b2.timeEntryId = te.id 
                        and b2.userid = @userId
                ) as isbookmarkedbycurrentuser
                from time_entries te 
                where te.id = @id
                """, new { id, userId }, cancellationToken: token));

        if (timeEntryRaw is null)
        {
            return null;
        }

        var timeEntry = new TimeEntry
        {
            Id = timeEntryRaw.id,
            Description = timeEntryRaw.description,
            Hours = timeEntryRaw.hours,
            Date = timeEntryRaw.date,
            Comments = string.IsNullOrWhiteSpace(timeEntryRaw.comments)
                ? []
                : timeEntryRaw.comments.Split(',').ToList(),
            BookmarkCount = timeEntryRaw.bookmarkcount,
            IsBookmarkedByCurrentUser = timeEntryRaw.isbookmarkedbycurrentuser,
        };

        return timeEntry;
    }

    public async Task<bool> UpdateAsync(TimeEntry entry, CancellationToken token = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync(token);
        using var transaction = connection.BeginTransaction();

        await connection.ExecuteAsync(new CommandDefinition(
            """
            delete from comments where timeEntryId = @id
            """, new { id = entry.Id }, cancellationToken: token));

        foreach (var comment in entry.Comments)
        {
            await connection.ExecuteAsync(new CommandDefinition(
                """
                insert into comments (timeEntryId, comment)
                values (@TimeEntryId, @Comment)
                """, new { TimeEntryId = entry.Id, comment }, cancellationToken: token));
        }

        var result = await connection.ExecuteAsync(new CommandDefinition(
            """
            update time_entries set description = @Description, date = @Date, hours = @Hours
            where id = @Id
            """, entry, cancellationToken: token));

        transaction.Commit();
        return result > 0;
    }

    public async Task<bool> DeleteByIdAsync(Guid id, CancellationToken token = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync(token);
        using var transaction = connection.BeginTransaction();

        await connection.ExecuteAsync(new CommandDefinition(
            """
            delete from comments where timeEntryId = @id
            """, new { id }, cancellationToken: token));

        var result = await connection.ExecuteAsync(new CommandDefinition(
            """
            delete from time_entries where id = @id
            """, new { id }, cancellationToken: token));

        transaction.Commit();

        return result > 0;
    }

    public async Task<bool> ExistsByIdAsync(Guid id, CancellationToken token = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync(token);

        return await connection.ExecuteScalarAsync<bool>(new CommandDefinition(
            """
            select count(1)
            from time_entries 
            where id = @id
            """, new { id }, cancellationToken: token));
    }

    public async Task<IEnumerable<TimeEntry>> GetAllAsync(Guid? userId = default, CancellationToken token = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync(token);

        var results = await connection.QueryAsync<TimeEntryRaw>(
            new CommandDefinition(
                """
                select 
                    te.id, 
                    te.description, 
                    te.hours, 
                    te.date, 
                    -- comments
                    (
                        select string_agg(c.comment, ',') 
                        from comments c
                        where c.timeEntryId = te.id
                    ) as comments,
                    -- bookmark count
                    (
                        select count(*)
                        from bookmarks b
                        where b.timeEntryId = te.id
                    ) as bookmarkcount,
                    -- is bookmarked by the current user
                    exists (
                        select 1 
                        from bookmarks b2
                        where b2.timeEntryId = te.id 
                            and b2.userid = @userId
                    ) as isbookmarkedbycurrentuser
                from time_entries te 
                """, new { userId }, cancellationToken: token));

        return results.Select(x => new TimeEntry
        {
            Id = x.id,
            Description = x.description,
            Hours = x.hours,
            Date = x.date,
            Comments = string.IsNullOrWhiteSpace(x.comments)
                ? []
                : x.comments.Split(',').ToList(),
            BookmarkCount = x.bookmarkcount,
            IsBookmarkedByCurrentUser = x.isbookmarkedbycurrentuser,
        });
    }
}

internal class TimeEntryRaw
{
    public Guid id { get; init; }

    public string description { get; init; }

    public DateTime date { get; init; }

    public decimal hours { get; init; }

    public string? comments { get; init; }

    public int bookmarkcount { get; init; }

    public bool isbookmarkedbycurrentuser { get; init; }
}