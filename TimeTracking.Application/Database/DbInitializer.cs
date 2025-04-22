using Dapper;

namespace TimeTracking.Application.Database;

public class DbInitializer
{
    private readonly IDbConnectionFactory _dbConnectionFactory;

    public DbInitializer(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }

    public async Task InitializeAsync()
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();

        await connection.ExecuteAsync(
            """
            create table if not exists time_entries (
            id uuid primary key,
            description text not null,
            date timestamp not null,
            hours decimal not null);
            """);

        await connection.ExecuteAsync(
            """
            create table if not exists clients (
                id uuid primary key,
                name text not null,
                slug text not null);
            """);

        await connection.ExecuteAsync(
            """
            create unique index concurrently if not exists clients_slug_index 
            on clients
            using btree(slug);
            """
        );

        await connection.ExecuteAsync(
            """
            create table if not exists comments (
            timeEntryId uuid references time_entries(id),
            comment text not null);
            """);
    }
}