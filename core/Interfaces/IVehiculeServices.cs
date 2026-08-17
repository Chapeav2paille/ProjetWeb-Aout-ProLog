using ProManLog.Core.DTOs;

namespace ProManLog.Core.Interfaces;
public interface IVehiculeService
{
    Task<IEnumerable<VehiculeDto>> GetVehiculeAsync();
    Task<VehiculeDto?> GetByIdAsync(int idVehicule);
    Task<VehiculeDto> CreateAsync(CreateVehiculeDto dto);
    Task<bool> UpdateAsync(int idVehicule, CreateVehiculeDto dto);
    Task<bool> DeleteAsync(int idVehicule);
}
