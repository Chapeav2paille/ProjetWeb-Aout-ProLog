using Dapper;
using Microsoft.Data.Sqlite;
using ProManLog.Core.DTOs;
using ProManLog.Core.Entities;
using ProManLog.Core.Interfaces;
using ProManLog.Infrastructure.Data;

namespace ProManLog.Infrastructure.Repositories;

public class VehiculeRepository : IVehiculeRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public VehiculeRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IEnumerable<Vehicule>> GetVehiculeAsync()
    {
        using var connection = _connectionFactory.CreateConnection();
        var sql = "SELECT * FROM Vehicule ORDER BY Immatriculation";
        return await connection.QueryAsync<Vehicule>(sql);
    }

    public async Task<Vehicule?> GetByIdAsync(int idVehicule)
    {
        using var connection = _connectionFactory.CreateConnection();
        var sql = "SELECT * FROM Vehicule WHERE IdVehicule = @IdVehicule";
        return await connection.QueryFirstOrDefaultAsync<Vehicule>(sql, new { IdVehicule = idVehicule });
    }

    public async Task<IEnumerable<Vehicule>> GetEcheancesAvantAsync(DateTime dateLimite)
    {
        using var connection = _connectionFactory.CreateConnection();
        var sql = @"SELECT * FROM Vehicule
                    WHERE ProchainEntretien <= @DateLimite
                       OR ProchainControleTechnique <= @DateLimite
                       OR FinAssurance <= @DateLimite";
        return await connection.QueryAsync<Vehicule>(sql, new { DateLimite = dateLimite });
    }

    public async Task<IEnumerable<StatutCompteDto>> CompterParStatutAsync()
    {
        using var connection = _connectionFactory.CreateConnection();
        var sql = "SELECT Statut, COUNT(*) AS Nombre FROM Vehicule GROUP BY Statut";
        return await connection.QueryAsync<StatutCompteDto>(sql);
    }

    public async Task<Vehicule> AddAsync(Vehicule vehicule)
    {
        using var connection = _connectionFactory.CreateConnection();
        var sql = @"INSERT INTO Vehicule (Immatriculation, TypeVehicule, CapaciteKg, Kilometrage, Statut,
                                          DernierEntretien, ProchainEntretien, ProchainControleTechnique, FinAssurance)
                    VALUES (@Immatriculation, @TypeVehicule, @CapaciteKg, @Kilometrage, @Statut,
                            @DernierEntretien, @ProchainEntretien, @ProchainControleTechnique, @FinAssurance);
                    SELECT last_insert_rowid();";
        var nouvelId = await connection.ExecuteScalarAsync<int>(sql, vehicule);
        vehicule.IdVehicule = nouvelId;
        return vehicule;
    }

    public async Task<bool> UpdateAsync(Vehicule vehicule)
    {
        using var connection = _connectionFactory.CreateConnection();
        var sql = @"UPDATE Vehicule
                    SET Immatriculation = @Immatriculation, TypeVehicule = @TypeVehicule,
                        CapaciteKg = @CapaciteKg, Kilometrage = @Kilometrage, Statut = @Statut,
                        DernierEntretien = @DernierEntretien, ProchainEntretien = @ProchainEntretien,
                        ProchainControleTechnique = @ProchainControleTechnique, FinAssurance = @FinAssurance
                    WHERE IdVehicule = @IdVehicule;";
        var resultat = await connection.ExecuteAsync(sql, vehicule);
        return resultat > 0;
    }

    public async Task<bool> DeleteAsync(int idVehicule)
    {
        using var connection = _connectionFactory.CreateConnection();
        var sql = "DELETE FROM Vehicule WHERE IdVehicule = @IdVehicule;";
        try
        {
            var resultat = await connection.ExecuteAsync(sql, new { IdVehicule = idVehicule });
            return resultat > 0;
        }
        catch (SqliteException exception) when (exception.SqliteErrorCode == 19)
        {
            return false;
        }
    }
}
