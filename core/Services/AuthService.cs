using ProManLog.Core.DTOs;
using ProManLog.Core.Interfaces;

namespace ProManLog.Core.Services;

public class AuthService : IAuthService
{
    private readonly IUtilisateurRepository _utilisateurRepository;
    private readonly IHacheurMotDePasse _hacheurMotDePasse;
    private readonly IGenerateurToken _generateurToken;

    public AuthService(
        IUtilisateurRepository utilisateurRepository,
        IHacheurMotDePasse hacheurMotDePasse,
        IGenerateurToken generateurToken)
    {
        _utilisateurRepository = utilisateurRepository;
        _hacheurMotDePasse = hacheurMotDePasse;
        _generateurToken = generateurToken;
    }

    public async Task<UtilisateurConnecteDto?> ConnexionAsync(ConnexionDto dto)
    {
        var utilisateur = await _utilisateurRepository.GetParNomUtilisateurAsync(dto.NomUtilisateur);
        if (utilisateur is null) return null;

        if (!_hacheurMotDePasse.Verifier(dto.MotDePasse, utilisateur.MotDePasseHash)) return null;

        return new UtilisateurConnecteDto
        {
            Token = _generateurToken.Generer(utilisateur),
            NomUtilisateur = utilisateur.NomUtilisateur,
            NomComplet = utilisateur.NomComplet,
            Role = utilisateur.Role
        };
    }
}
