using Dapper;
using ProManLog.Core.Entities;
using ProManLog.Core.Interfaces;
using ProManLog.Infrastructure.Data;

namespace ProManLog.Infrastructure.Repositories;

public class UtilisateurRepository : IUtilisateurRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public UtilisateurRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<Utilisateur?> GetParNomUtilisateurAsync(string nomUtilisateur)
    {
        using var connection = _connectionFactory.CreateConnection();
        var sql = "SELECT * FROM Utilisateur WHERE NomUtilisateur = @NomUtilisateur";
        return await connection.QueryFirstOrDefaultAsync<Utilisateur>(sql, new { NomUtilisateur = nomUtilisateur });
    }
}
