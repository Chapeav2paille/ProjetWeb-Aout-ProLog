using ProManLog.Core.DTOs;

namespace ProManLog.Core.Interfaces;
public interface IAuthService
{
    Task<UtilisateurConnecteDto?> ConnexionAsync(ConnexionDto dto);
}
