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

    public async Task<bool> CreateAsync(Client client)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();
        using var transaction = connection.BeginTransaction();

        var result = await connection.ExecuteAsync(new CommandDefinition(
            """
            insert into clients (id, name, slug)
            values (@Id, @Name, @Slug)
            """, client));
        transaction.Commit();

        return result > 0;
    }

    public async Task<Client?> GetByIdAsync(Guid id)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();

        var client = await connection.QuerySingleOrDefaultAsync<Client>(
            new CommandDefinition(
                """
                select * from clients where id = @id
                """, new { id }));

        return client;
    }

    public async Task<Client?> GetBySlugAsync(string slug)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();

        var client = await connection.QuerySingleOrDefaultAsync<Client>(
            new CommandDefinition(
                """
                select * from clients where slug = @slug
                """, new { slug }));

        return client;
    }

    public async Task<IEnumerable<Client>> GetAllAsync()
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();

        var results = await connection.QueryAsync<Client>(
            new CommandDefinition(
                """
                select *
                from clients
                """));

        return results;
    }

    public async Task<bool> UpdateAsync(Client client)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();
        using var transaction = connection.BeginTransaction();

        var result = await connection.ExecuteAsync(new CommandDefinition(
            """
            update clients set name = @Name, slug = @Slug
            where id = @Id
            """, client));

        transaction.Commit();

        return result > 0;
    }

    public async Task<bool> DeleteByIdAsync(Guid id)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();
        using var transaction = connection.BeginTransaction();

        var result = await connection.ExecuteAsync(new CommandDefinition(
            """
            delete from clients where id = @id
            """, new { id }));

        transaction.Commit();

        return result > 0;
    }

    public async Task<bool> ExistsByIdAsync(Guid id)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();

        return await connection.ExecuteScalarAsync<bool>(new CommandDefinition(
            """
            select count(1) from clients where id = @id
            """, new { id }));
    }
}