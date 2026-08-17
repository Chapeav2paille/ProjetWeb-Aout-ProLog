using Dapper;
using ProManLog.Core.Entities;
using ProManLog.Core.Interfaces;
using ProManLog.Infrastructure.Data;

namespace ProManLog.Infrastructure.Repositories;

public class ChargeRepository : IChargeRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public ChargeRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IEnumerable<Charge>> GetDepuisAsync(DateTime periodeDebut)
    {
        using var connection = _connectionFactory.CreateConnection();
        var sql = "SELECT * FROM Charge WHERE Periode >= @PeriodeDebut ORDER BY Periode";
        return await connection.QueryAsync<Charge>(sql, new { PeriodeDebut = periodeDebut.ToString("yyyy-MM-dd") });
    }
}
