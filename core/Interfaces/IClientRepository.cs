using ProManLog.Core.Entities;

namespace ProManLog.Core.Interfaces;
public interface IClientRepository
{
    Task<IEnumerable<Client>> GetClientAsync();
    Task<Client?> GetByIdAsync(int idClient);
    Task<Client> AddAsync(Client client);
    Task<bool> UpdateAsync(Client client);
    Task<bool> DeleteAsync(int idClient);
}
