using ProManLog.Core.DTOs;

namespace ProManLog.Core.Interfaces;
public interface IPrestationService
{
    Task<IEnumerable<PrestationDto>> GetPrestationAsync(string? statut);
    Task<PrestationDto?> GetByIdAsync(int idPrestation);
    Task<PrestationDto> CreateAsync(CreatePrestationDto dto);
    Task<bool> UpdateAsync(int idPrestation, CreatePrestationDto dto);
    Task<bool> ChangerStatutAsync(int idPrestation, string statut);
    Task<bool> DeleteAsync(int idPrestation);
}
