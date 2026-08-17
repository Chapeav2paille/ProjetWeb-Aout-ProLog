using ProManLog.Core.DTOs;
using ProManLog.Core.Entities;
using ProManLog.Core.Interfaces;

namespace ProManLog.Core.Services;

public class HistoriqueService : IHistoriqueService
{
    private readonly IHistoriqueRepository _historiqueRepository;

    public HistoriqueService(IHistoriqueRepository historiqueRepository)
    {
        _historiqueRepository = historiqueRepository;
    }

    public async Task<IEnumerable<HistoriqueDto>> GetHistoriqueAsync(DateTime? du, DateTime? au, string? typeAction)
    {
        var historique = await _historiqueRepository.GetHistoriqueAsync(du, au, typeAction);
        return historique.Select(VersDto).ToList();
    }

    public async Task EnregistrerAsync(string typeAction, string typeEntite, int? idEntite, string description)
    {
        var entree = new Historique
        {
            DateAction = DateTime.Now,
            TypeAction = typeAction,
            TypeEntite = typeEntite,
            IdEntite = idEntite,
            Description = description
        };
        await _historiqueRepository.AddAsync(entree);
    }

    public static HistoriqueDto VersDto(Historique historique) => new()
    {
        IdHistorique = historique.IdHistorique,
        DateAction = historique.DateAction,
        TypeAction = historique.TypeAction,
        TypeEntite = historique.TypeEntite,
        IdEntite = historique.IdEntite,
        Description = historique.Description
    };
}
