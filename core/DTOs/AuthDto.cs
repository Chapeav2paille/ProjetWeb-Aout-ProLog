namespace ProManLog.Core.DTOs;
public class ConnexionDto
{
    public string NomUtilisateur { get; set; } = string.Empty;
    public string MotDePasse { get; set; } = string.Empty;
}

public class UtilisateurConnecteDto
{
    public string Token { get; set; } = string.Empty;
    public string NomUtilisateur { get; set; } = string.Empty;
    public string NomComplet { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
}
