using Dapper;
using Microsoft.Data.Sqlite;
using ProManLog.Core.Entities;
using ProManLog.Core.Interfaces;
using ProManLog.Infrastructure.Data;

namespace ProManLog.Infrastructure.Repositories;

public class EmployeRepository : IEmployeRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public EmployeRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IEnumerable<Employe>> GetEmployeAsync()
    {
        using var connection = _connectionFactory.CreateConnection();
        var sql = "SELECT * FROM Employe ORDER BY Nom, Prenom";
        return await connection.QueryAsync<Employe>(sql);
    }

    public async Task<Employe?> GetByIdAsync(int idEmploye)
    {
        using var connection = _connectionFactory.CreateConnection();
        var sql = "SELECT * FROM Employe WHERE IdEmploye = @IdEmploye";
        return await connection.QueryFirstOrDefaultAsync<Employe>(sql, new { IdEmploye = idEmploye });
    }

    public async Task<IEnumerable<Employe>> GetPermisExpirantAvantAsync(DateTime dateLimite)
    {
        using var connection = _connectionFactory.CreateConnection();
        var sql = @"SELECT * FROM Employe
                    WHERE Statut = 'Actif'
                      AND ExpirationPermis IS NOT NULL
                      AND ExpirationPermis <= @DateLimite";
        return await connection.QueryAsync<Employe>(sql, new { DateLimite = dateLimite });
    }

    public async Task<Employe> AddAsync(Employe employe)
    {
        using var connection = _connectionFactory.CreateConnection();
        var sql = @"INSERT INTO Employe (Nom, Prenom, Poste, DateEmbauche, Email, Telephone, Statut,
                                         Disponibilite, NumeroPermis, CategoriesPermis, ExpirationPermis)
                    VALUES (@Nom, @Prenom, @Poste, @DateEmbauche, @Email, @Telephone, @Statut,
                            @Disponibilite, @NumeroPermis, @CategoriesPermis, @ExpirationPermis);
                    SELECT last_insert_rowid();";
        var nouvelId = await connection.ExecuteScalarAsync<int>(sql, employe);
        employe.IdEmploye = nouvelId;
        return employe;
    }

    public async Task<bool> UpdateAsync(Employe employe)
    {
        using var connection = _connectionFactory.CreateConnection();
        var sql = @"UPDATE Employe
                    SET Nom = @Nom, Prenom = @Prenom, Poste = @Poste, DateEmbauche = @DateEmbauche,
                        Email = @Email, Telephone = @Telephone, Statut = @Statut,
                        Disponibilite = @Disponibilite, NumeroPermis = @NumeroPermis,
                        CategoriesPermis = @CategoriesPermis, ExpirationPermis = @ExpirationPermis
                    WHERE IdEmploye = @IdEmploye;";
        var resultat = await connection.ExecuteAsync(sql, employe);
        return resultat > 0;
    }

    public async Task<bool> DeleteAsync(int idEmploye)
    {
        using var connection = _connectionFactory.CreateConnection();
        var sql = "DELETE FROM Employe WHERE IdEmploye = @IdEmploye;";
        try
        {
            var resultat = await connection.ExecuteAsync(sql, new { IdEmploye = idEmploye });
            return resultat > 0;
        }
        catch (SqliteException exception) when (exception.SqliteErrorCode == 19)
        {
            return false;
        }
    }
}
