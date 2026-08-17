using System.Globalization;
using ProManLog.Core.DTOs;
using ProManLog.Core.Interfaces;

namespace ProManLog.Core.Services;

public class TableauBordService : ITableauBordService
{
    private const int JoursAvantAlerte = 30;
    private const int NombreDernieresActivites = 5;
    private const int NombreMoisGraphique = 6;

    private readonly IPrestationRepository _prestationRepository;
    private readonly IVehiculeRepository _vehiculeRepository;
    private readonly IEmployeRepository _employeRepository;
    private readonly IHistoriqueRepository _historiqueRepository;

    public TableauBordService(
        IPrestationRepository prestationRepository,
        IVehiculeRepository vehiculeRepository,
        IEmployeRepository employeRepository,
        IHistoriqueRepository historiqueRepository)
    {
        _prestationRepository = prestationRepository;
        _vehiculeRepository = vehiculeRepository;
        _employeRepository = employeRepository;
        _historiqueRepository = historiqueRepository;
    }

    public async Task<TableauBordDto> GetTableauBordAsync()
    {
        var aujourdhui = DateTime.Today;
        var dateLimite = aujourdhui.AddDays(JoursAvantAlerte);

        var prestationsDuMois = (await _prestationRepository.GetDuMoisAsync(aujourdhui.Year, aujourdhui.Month)).ToList();
        var vehicules = (await _vehiculeRepository.GetVehiculeAsync()).ToList();
        var dernieres = await _historiqueRepository.GetDernieresAsync(NombreDernieresActivites);

        var tableauBord = new TableauBordDto
        {
            PrestationsEnCours = prestationsDuMois.Count(prestation => prestation.Statut == "EnCours"),
            PrestationsTerminees = prestationsDuMois.Count(prestation => prestation.Statut == "Terminee"),
            VehiculesDisponibles = vehicules.Count(vehicule => vehicule.Statut == "Disponible"),
            VehiculesEnMission = vehicules.Count(vehicule => vehicule.Statut == "EnMission"),
            VehiculesEnMaintenance = vehicules.Count(vehicule => vehicule.Statut == "EnMaintenance"),
            ChiffreAffaireDuMois = prestationsDuMois
                .Where(prestation => prestation.Statut == "Terminee")
                .Sum(prestation => prestation.Prix),
            PrestationsDuMois = prestationsDuMois.Select(PrestationService.VersDto).ToList(),
            Vehicules = vehicules.Select(VehiculeService.VersDto).ToList(),
            DernieresActivites = dernieres.Select(HistoriqueService.VersDto).ToList()
        };

        tableauBord.EvolutionChiffreAffaire = await ConstruireEvolutionAsync(aujourdhui);

        tableauBord.Alertes = (await ConstruireAlertesAsync(aujourdhui, dateLimite))
            .OrderBy(alerte => alerte.DateEcheance)
            .ToList();

        return tableauBord;
    }

    private async Task<List<PointChiffreAffaireDto>> ConstruireEvolutionAsync(DateTime aujourdhui)
    {
        var premierMois = new DateTime(aujourdhui.Year, aujourdhui.Month, 1).AddMonths(-(NombreMoisGraphique - 1));
        var chiffreAffaire = await _prestationRepository.GetChiffreAffaireParMoisAsync(premierMois);
        var culture = new CultureInfo("fr-FR");

        var evolution = new List<PointChiffreAffaireDto>();
        for (var rang = 0; rang < NombreMoisGraphique; rang++)
        {
            var mois = premierMois.AddMonths(rang);
            evolution.Add(new PointChiffreAffaireDto
            {
                Periode = mois.ToString("MMM yy", culture),
                Montant = chiffreAffaire
                    .Where(point => point.Annee == mois.Year && point.Mois == mois.Month)
                    .Sum(point => point.Montant)
            });
        }
        return evolution;
    }

    private async Task<List<AlerteDto>> ConstruireAlertesAsync(DateTime aujourdhui, DateTime dateLimite)
    {
        var alertes = new List<AlerteDto>();

        var vehicules = await _vehiculeRepository.GetEcheancesAvantAsync(dateLimite);
        foreach (var vehicule in vehicules)
        {
            AjouterAlerte(alertes, vehicule.ProchainEntretien, aujourdhui, dateLimite, "Entretien",
                $"Entretien du véhicule {vehicule.Immatriculation}");
            AjouterAlerte(alertes, vehicule.ProchainControleTechnique, aujourdhui, dateLimite, "ControleTechnique",
                $"Contrôle technique du véhicule {vehicule.Immatriculation}");
            AjouterAlerte(alertes, vehicule.FinAssurance, aujourdhui, dateLimite, "Assurance",
                $"Assurance du véhicule {vehicule.Immatriculation}");
        }

        var chauffeurs = await _employeRepository.GetPermisExpirantAvantAsync(dateLimite);
        foreach (var chauffeur in chauffeurs)
        {
            AjouterAlerte(alertes, chauffeur.ExpirationPermis, aujourdhui, dateLimite, "Permis",
                $"Permis de {chauffeur.Prenom} {chauffeur.Nom} ({chauffeur.NumeroPermis})");
        }

        return alertes;
    }

    private static void AjouterAlerte(
        List<AlerteDto> alertes, DateTime? dateEcheance, DateTime aujourdhui,
        DateTime dateLimite, string typeAlerte, string libelle)
    {
        if (dateEcheance is null || dateEcheance.Value.Date > dateLimite) return;

        var estExpiree = dateEcheance.Value.Date < aujourdhui;
        alertes.Add(new AlerteDto
        {
            TypeAlerte = typeAlerte,
            Message = estExpiree
                ? $"{libelle} : expiré le {dateEcheance:dd/MM/yyyy}"
                : $"{libelle} : échéance le {dateEcheance:dd/MM/yyyy}",
            DateEcheance = dateEcheance.Value,
            Gravite = estExpiree ? "Critique" : "Avertissement"
        });
    }
}
