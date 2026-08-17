using Dapper;
using Microsoft.Data.Sqlite;
using ProManLog.Core.Entities;
using ProManLog.Core.Interfaces;
using ProManLog.Infrastructure.Data;

namespace ProManLog.Infrastructure.Repositories;

public class ClientRepository : IClientRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public ClientRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IEnumerable<Client>> GetClientAsync()
    {
        using var connection = _connectionFactory.CreateConnection();
        var sql = "SELECT * FROM Client ORDER BY Nom";
        return await connection.QueryAsync<Client>(sql);
    }

    public async Task<Client?> GetByIdAsync(int idClient)
    {
        using var connection = _connectionFactory.CreateConnection();
        var sql = "SELECT * FROM Client WHERE IdClient = @IdClient";
        return await connection.QueryFirstOrDefaultAsync<Client>(sql, new { IdClient = idClient });
    }

    public async Task<Client> AddAsync(Client client)
    {
        using var connection = _connectionFactory.CreateConnection();
        var sql = @"INSERT INTO Client (TypeClient, Nom, Contact, Email, Telephone, Adresse)
                    VALUES (@TypeClient, @Nom, @Contact, @Email, @Telephone, @Adresse);
                    SELECT last_insert_rowid();";
        var nouvelId = await connection.ExecuteScalarAsync<int>(sql, client);
        client.IdClient = nouvelId;
        return client;
    }

    public async Task<bool> UpdateAsync(Client client)
    {
        using var connection = _connectionFactory.CreateConnection();
        var sql = @"UPDATE Client
                    SET TypeClient = @TypeClient, Nom = @Nom, Contact = @Contact,
                        Email = @Email, Telephone = @Telephone, Adresse = @Adresse
                    WHERE IdClient = @IdClient;";
        var resultat = await connection.ExecuteAsync(sql, client);
        return resultat > 0;
    }

    public async Task<bool> DeleteAsync(int idClient)
    {
        using var connection = _connectionFactory.CreateConnection();
        var sql = "DELETE FROM Client WHERE IdClient = @IdClient;";
        try
        {
            var resultat = await connection.ExecuteAsync(sql, new { IdClient = idClient });
            return resultat > 0;
        }
        catch (SqliteException exception) when (exception.SqliteErrorCode == 19)
        {
            return false;
        }
    }
}
