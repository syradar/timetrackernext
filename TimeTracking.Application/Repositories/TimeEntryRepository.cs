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

    public async Task<TimeEntry?> GetByIdAsync(Guid id, CancellationToken token = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync(token);

        var timeEntry = await connection.QuerySingleOrDefaultAsync<TimeEntry>(
            new CommandDefinition(
                """
                select * from time_entries where id = @id
                """, new { id }, cancellationToken: token));

        if (timeEntry is null)
        {
            return null;
        }

        var comments = await connection.QueryAsync<string>(
            new CommandDefinition(
                """
                select comment from comments where timeEntryId = @id
                """, new { id }, cancellationToken: token));

        timeEntry.Comments.AddRange(comments);

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
            select count(1) from time_entries where id = @id
            """, new { id }, cancellationToken: token));
    }

    public async Task<IEnumerable<TimeEntry>> GetAllAsync(CancellationToken token = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync(token);

        var results = await connection.QueryAsync(
            new CommandDefinition(
                """
                select te.*, string_agg(c.comment, ',') as comments
                from time_entries te left join comments c on te.id = c.timeEntryId
                group by id
                """, cancellationToken: token));

        return results.Select(x => new TimeEntry
        {
            Id = x.id,
            Description = x.description,
            Hours = x.hours,
            Comments = Enumerable.ToList(x.comments.Split(',')),
            Date = x.date,
        });
    }
}