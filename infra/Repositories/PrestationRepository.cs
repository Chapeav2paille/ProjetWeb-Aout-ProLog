using Dapper;
using ProManLog.Core.DTOs;
using ProManLog.Core.Entities;
using ProManLog.Core.Interfaces;
using ProManLog.Infrastructure.Data;

namespace ProManLog.Infrastructure.Repositories;

public class PrestationRepository : IPrestationRepository
{
    private const string SelectAvecLibelles = @"
        SELECT p.*,
               c.Nom AS NomClient,
               v.Immatriculation AS ImmatriculationVehicule,
               (e.Prenom || ' ' || e.Nom) AS NomEmploye
        FROM Prestation p
        JOIN Client c ON c.IdClient = p.IdClient
        LEFT JOIN Vehicule v ON v.IdVehicule = p.IdVehicule
        LEFT JOIN Employe e ON e.IdEmploye = p.IdEmploye";

    private readonly IDbConnectionFactory _connectionFactory;

    public PrestationRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IEnumerable<Prestation>> GetPrestationAsync(string? statut)
    {
        using var connection = _connectionFactory.CreateConnection();
        var sql = SelectAvecLibelles;
        if (!string.IsNullOrEmpty(statut)) sql += " WHERE p.Statut = @Statut";
        sql += " ORDER BY p.DateHeure DESC";
        return await connection.QueryAsync<Prestation>(sql, new { Statut = statut });
    }

    public async Task<IEnumerable<Prestation>> GetByClientAsync(int idClient)
    {
        using var connection = _connectionFactory.CreateConnection();
        var sql = SelectAvecLibelles + " WHERE p.IdClient = @IdClient ORDER BY p.DateHeure DESC";
        return await connection.QueryAsync<Prestation>(sql, new { IdClient = idClient });
    }

    public async Task<Prestation?> GetByIdAsync(int idPrestation)
    {
        using var connection = _connectionFactory.CreateConnection();
        var sql = SelectAvecLibelles + " WHERE p.IdPrestation = @IdPrestation";
        return await connection.QueryFirstOrDefaultAsync<Prestation>(sql, new { IdPrestation = idPrestation });
    }

    public async Task<IEnumerable<Prestation>> GetDuMoisAsync(int annee, int mois)
    {
        using var connection = _connectionFactory.CreateConnection();
        var sql = SelectAvecLibelles +
                  " WHERE strftime('%Y', p.DateHeure) = @Annee AND strftime('%m', p.DateHeure) = @Mois" +
                  " ORDER BY p.DateHeure DESC";
        return await connection.QueryAsync<Prestation>(sql, new { Annee = annee.ToString(), Mois = mois.ToString("00") });
    }

    public async Task<IEnumerable<ChiffreAffaireMois>> GetChiffreAffaireParMoisAsync(DateTime depuis)
    {
        using var connection = _connectionFactory.CreateConnection();
        var sql = @"SELECT CAST(strftime('%Y', DateHeure) AS INTEGER) AS Annee,
                           CAST(strftime('%m', DateHeure) AS INTEGER) AS Mois,
                           SUM(Prix) AS Montant
                    FROM Prestation
                    WHERE Statut = 'Terminee' AND DateHeure >= @Depuis
                    GROUP BY strftime('%Y-%m', DateHeure)
                    ORDER BY Annee, Mois";
        return await connection.QueryAsync<ChiffreAffaireMois>(sql, new { Depuis = depuis.ToString("yyyy-MM-dd") });
    }

    public async Task<IEnumerable<StatutCompteDto>> CompterParStatutDuMoisAsync(int annee, int mois)
    {
        using var connection = _connectionFactory.CreateConnection();
        var sql = @"SELECT Statut, COUNT(*) AS Nombre
                    FROM Prestation
                    WHERE strftime('%Y', DateHeure) = @Annee AND strftime('%m', DateHeure) = @Mois
                    GROUP BY Statut";
        return await connection.QueryAsync<StatutCompteDto>(sql, new { Annee = annee.ToString(), Mois = mois.ToString("00") });
    }

    public async Task<Prestation> AddAsync(Prestation prestation)
    {
        using var connection = _connectionFactory.CreateConnection();
        var sql = @"INSERT INTO Prestation (IdClient, IdVehicule, IdEmploye, AdresseDepart, AdresseArrivee,
                                            DateHeure, TypeService, Prix, Statut)
                    VALUES (@IdClient, @IdVehicule, @IdEmploye, @AdresseDepart, @AdresseArrivee,
                            @DateHeure, @TypeService, @Prix, @Statut);
                    SELECT last_insert_rowid();";
        var nouvelId = await connection.ExecuteScalarAsync<int>(sql, prestation);
        prestation.IdPrestation = nouvelId;
        return prestation;
    }

    public async Task<bool> UpdateAsync(Prestation prestation)
    {
        using var connection = _connectionFactory.CreateConnection();
        var sql = @"UPDATE Prestation
                    SET IdClient = @IdClient, IdVehicule = @IdVehicule, IdEmploye = @IdEmploye,
                        AdresseDepart = @AdresseDepart, AdresseArrivee = @AdresseArrivee,
                        DateHeure = @DateHeure, TypeService = @TypeService, Prix = @Prix, Statut = @Statut
                    WHERE IdPrestation = @IdPrestation;";
        var resultat = await connection.ExecuteAsync(sql, prestation);
        return resultat > 0;
    }

    public async Task<bool> UpdateStatutAsync(int idPrestation, string statut)
    {
        using var connection = _connectionFactory.CreateConnection();
        var sql = "UPDATE Prestation SET Statut = @Statut WHERE IdPrestation = @IdPrestation;";
        var resultat = await connection.ExecuteAsync(sql, new { IdPrestation = idPrestation, Statut = statut });
        return resultat > 0;
    }

    public async Task<bool> DeleteAsync(int idPrestation)
    {
        using var connection = _connectionFactory.CreateConnection();
        var sql = "DELETE FROM Prestation WHERE IdPrestation = @IdPrestation;";
        var resultat = await connection.ExecuteAsync(sql, new { IdPrestation = idPrestation });
        return resultat > 0;
    }
}
