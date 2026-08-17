using ProManLog.Core.DTOs;
using ProManLog.Core.Entities;

namespace ProManLog.Core.Interfaces;
public interface IVehiculeRepository
{
    Task<IEnumerable<Vehicule>> GetVehiculeAsync();
    Task<Vehicule?> GetByIdAsync(int idVehicule);
    Task<IEnumerable<Vehicule>> GetEcheancesAvantAsync(DateTime dateLimite);
    Task<IEnumerable<StatutCompteDto>> CompterParStatutAsync();
    Task<Vehicule> AddAsync(Vehicule vehicule);
    Task<bool> UpdateAsync(Vehicule vehicule);
    Task<bool> DeleteAsync(int idVehicule);
}
