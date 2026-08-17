using System.Text;
using Dapper;
using ProManLog.Core.Entities;
using ProManLog.Core.Interfaces;
using ProManLog.Infrastructure.Data;

namespace ProManLog.Infrastructure.Repositories;

public class HistoriqueRepository : IHistoriqueRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public HistoriqueRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IEnumerable<Historique>> GetHistoriqueAsync(DateTime? du, DateTime? au, string? typeAction)
    {
        using var connection = _connectionFactory.CreateConnection();
        var sql = new StringBuilder("SELECT * FROM Historique WHERE 1 = 1");

        if (du is not null) sql.Append(" AND DateAction >= @Du");
        if (au is not null) sql.Append(" AND DateAction < date(@Au, '+1 day')");
        if (!string.IsNullOrEmpty(typeAction)) sql.Append(" AND TypeAction = @TypeAction");

        sql.Append(" ORDER BY DateAction DESC, IdHistorique DESC");

        var parametres = new
        {
            Du = du?.ToString("yyyy-MM-dd"),
            Au = au?.ToString("yyyy-MM-dd"),
            TypeAction = typeAction
        };
        return await connection.QueryAsync<Historique>(sql.ToString(), parametres);
    }

    public async Task<IEnumerable<Historique>> GetDernieresAsync(int nombre)
    {
        using var connection = _connectionFactory.CreateConnection();
        var sql = "SELECT * FROM Historique ORDER BY DateAction DESC, IdHistorique DESC LIMIT @Nombre";
        return await connection.QueryAsync<Historique>(sql, new { Nombre = nombre });
    }

    public async Task AddAsync(Historique historique)
    {
        using var connection = _connectionFactory.CreateConnection();
        var sql = @"INSERT INTO Historique (DateAction, TypeAction, TypeEntite, IdEntite, Description)
                    VALUES (@DateAction, @TypeAction, @TypeEntite, @IdEntite, @Description);";
        await connection.ExecuteAsync(sql, historique);
    }
}
