using Dapper;
using TimeTracking.Application.Database;
using TimeTracking.Application.Models;

namespace TimeTracking.Application.Repositories;

public class ClientRepository : IClientRepository
{
    private readonly IDbConnectionFactory _dbConnectionFactory;

    public ClientRepository(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }

    public async Task<bool> CreateAsync(Client client, CancellationToken token = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync(token);
        using var transaction = connection.BeginTransaction();

        var result = await connection.ExecuteAsync(new CommandDefinition(
            """
            insert into clients (id, name, slug)
            values (@Id, @Name, @Slug)
            """, client, cancellationToken: token));
        transaction.Commit();

        return result > 0;
    }

    public async Task<Client?> GetByIdAsync(Guid id, CancellationToken token = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync(token);

        var client = await connection.QuerySingleOrDefaultAsync<Client>(
            new CommandDefinition(
                """
                select * from clients where id = @id
                """, new { id }, cancellationToken: token));

        return client;
    }

    public async Task<Client?> GetBySlugAsync(string slug, CancellationToken token = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync(token);

        var client = await connection.QuerySingleOrDefaultAsync<Client>(
            new CommandDefinition(
                """
                select * from clients where slug = @slug
                """, new { slug }, cancellationToken: token));

        return client;
    }

    public async Task<bool> UpdateAsync(Client client, CancellationToken token = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync(token);
        using var transaction = connection.BeginTransaction();

        var result = await connection.ExecuteAsync(new CommandDefinition(
            """
            update clients set name = @Name, slug = @Slug
            where id = @Id
            """, client, cancellationToken: token));

        transaction.Commit();

        return result > 0;
    }

    public async Task<bool> DeleteByIdAsync(Guid id, CancellationToken token = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync(token);
        using var transaction = connection.BeginTransaction();

        var result = await connection.ExecuteAsync(new CommandDefinition(
            """
            delete from clients where id = @id
            """, new { id }, cancellationToken: token));

        transaction.Commit();

        return result > 0;
    }

    public async Task<bool> ExistsByIdAsync(Guid id, CancellationToken token = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync(token);

        return await connection.ExecuteScalarAsync<bool>(new CommandDefinition(
            """
            select count(1) from clients where id = @id
            """, new { id }, cancellationToken: token));
    }

    public async Task<IEnumerable<Client>> GetAllAsync(CancellationToken token = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync(token);

        var results = await connection.QueryAsync<Client>(
            new CommandDefinition(
                """
                select *
                from clients
                """, cancellationToken: token));

        return results;
    }
}