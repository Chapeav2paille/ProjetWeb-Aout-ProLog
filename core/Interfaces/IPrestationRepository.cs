using ProManLog.Core.DTOs;
using ProManLog.Core.Entities;

namespace ProManLog.Core.Interfaces;
public interface IPrestationRepository
{
    Task<IEnumerable<Prestation>> GetPrestationAsync(string? statut);
    Task<IEnumerable<Prestation>> GetByClientAsync(int idClient);
    Task<IEnumerable<Prestation>> GetDuMoisAsync(int annee, int mois);
    Task<Prestation?> GetByIdAsync(int idPrestation);
    Task<IEnumerable<StatutCompteDto>> CompterParStatutDuMoisAsync(int annee, int mois);
    Task<IEnumerable<ChiffreAffaireMois>> GetChiffreAffaireParMoisAsync(DateTime depuis);
    Task<Prestation> AddAsync(Prestation prestation);
    Task<bool> UpdateAsync(Prestation prestation);
    Task<bool> UpdateStatutAsync(int idPrestation, string statut);
    Task<bool> DeleteAsync(int idPrestation);
}
