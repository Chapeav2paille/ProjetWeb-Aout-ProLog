namespace ProManLog.Core.DTOs;
public class TableauBordDto
{
    public int PrestationsEnCours { get; set; }
    public int PrestationsTerminees { get; set; }
    public int VehiculesDisponibles { get; set; }
    public int VehiculesEnMission { get; set; }
    public int VehiculesEnMaintenance { get; set; }
    public decimal ChiffreAffaireDuMois { get; set; }
    public List<PrestationDto> PrestationsDuMois { get; set; } = new();
    public List<VehiculeDto> Vehicules { get; set; } = new();
    public List<PointChiffreAffaireDto> EvolutionChiffreAffaire { get; set; } = new();
    public List<AlerteDto> Alertes { get; set; } = new();
    public List<HistoriqueDto> DernieresActivites { get; set; } = new();
}

public class AlerteDto
{
    public string TypeAlerte { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public DateTime DateEcheance { get; set; }
    public string Gravite { get; set; } = string.Empty;
}

public class StatutCompteDto
{
    public string Statut { get; set; } = string.Empty;
    public int Nombre { get; set; }
}
