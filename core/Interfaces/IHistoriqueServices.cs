using ProManLog.Core.DTOs;

namespace ProManLog.Core.Interfaces;
public interface IHistoriqueService
{
    Task<IEnumerable<HistoriqueDto>> GetHistoriqueAsync(DateTime? du, DateTime? au, string? typeAction);
    Task EnregistrerAsync(string typeAction, string typeEntite, int? idEntite, string description);
}
