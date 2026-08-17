using ProManLog.Core.DTOs;

namespace ProManLog.Core.Interfaces;
public interface IClientService
{
    Task<IEnumerable<ClientDto>> GetClientAsync();
    Task<ClientDto?> GetByIdAsync(int idClient);
    Task<IEnumerable<PrestationDto>> GetPrestationsAsync(int idClient);
    Task<ClientDto> CreateAsync(CreateClientDto dto);
    Task<bool> UpdateAsync(int idClient, CreateClientDto dto);
    Task<bool> DeleteAsync(int idClient);
}
