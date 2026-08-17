namespace ProManLog.Core.Entities;
public class Utilisateur
{
    public int IdUtilisateur { get; set; }
    public string NomUtilisateur { get; set; } = string.Empty;
    public string MotDePasseHash { get; set; } = string.Empty;
    public string NomComplet { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
}
