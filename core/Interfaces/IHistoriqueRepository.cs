using ProManLog.Core.Entities;

namespace ProManLog.Core.Interfaces;
public interface IHistoriqueRepository
{
    Task<IEnumerable<Historique>> GetHistoriqueAsync(DateTime? du, DateTime? au, string? typeAction);
    Task<IEnumerable<Historique>> GetDernieresAsync(int nombre);
    Task AddAsync(Historique historique);
}
