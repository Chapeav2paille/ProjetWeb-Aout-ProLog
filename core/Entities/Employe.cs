namespace ProManLog.Core.Entities;
public class Employe
{
    public int IdEmploye { get; set; }
    public string Nom { get; set; } = string.Empty;
    public string Prenom { get; set; } = string.Empty;
    public string Poste { get; set; } = string.Empty;
    public DateTime DateEmbauche { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Telephone { get; set; } = string.Empty;
    public string Statut { get; set; } = string.Empty;
    public string Disponibilite { get; set; } = string.Empty;
    public string NumeroPermis { get; set; } = string.Empty;
    public string CategoriesPermis { get; set; } = string.Empty;
    public DateTime? ExpirationPermis { get; set; }
}
