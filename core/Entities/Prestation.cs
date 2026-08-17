namespace ProManLog.Core.Entities;
public class Prestation
{
    public int IdPrestation { get; set; }
    public int IdClient { get; set; }
    public int? IdVehicule { get; set; }
    public int? IdEmploye { get; set; }
    public string AdresseDepart { get; set; } = string.Empty;
    public string AdresseArrivee { get; set; } = string.Empty;
    public DateTime DateHeure { get; set; }
    public string TypeService { get; set; } = string.Empty;
    public decimal Prix { get; set; }
    public string Statut { get; set; } = string.Empty;
    public string NomClient { get; set; } = string.Empty;
    public string ImmatriculationVehicule { get; set; } = string.Empty;
    public string NomEmploye { get; set; } = string.Empty;
}
