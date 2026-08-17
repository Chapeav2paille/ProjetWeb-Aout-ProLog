using ProManLog.Core.Entities;

namespace ProManLog.Core.Interfaces;
public interface IEmployeRepository
{
    Task<IEnumerable<Employe>> GetEmployeAsync();
    Task<Employe?> GetByIdAsync(int idEmploye);
    Task<IEnumerable<Employe>> GetPermisExpirantAvantAsync(DateTime dateLimite);
    Task<Employe> AddAsync(Employe employe);
    Task<bool> UpdateAsync(Employe employe);
    Task<bool> DeleteAsync(int idEmploye);
}
