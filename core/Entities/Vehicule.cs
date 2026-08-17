namespace ProManLog.Core.Entities;
public class Vehicule
{
    public int IdVehicule { get; set; }
    public string Immatriculation { get; set; } = string.Empty;
    public string TypeVehicule { get; set; } = string.Empty;
    public int CapaciteKg { get; set; }
    public int Kilometrage { get; set; }
    public string Statut { get; set; } = string.Empty;
    public DateTime? DernierEntretien { get; set; }
    public DateTime? ProchainEntretien { get; set; }
    public DateTime? ProchainControleTechnique { get; set; }
    public DateTime? FinAssurance { get; set; }
}
