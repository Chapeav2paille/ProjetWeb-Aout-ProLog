using ProManLog.Core.Entities;

namespace ProManLog.Core.Interfaces;
public interface IUtilisateurRepository
{
    Task<Utilisateur?> GetParNomUtilisateurAsync(string nomUtilisateur);
}
