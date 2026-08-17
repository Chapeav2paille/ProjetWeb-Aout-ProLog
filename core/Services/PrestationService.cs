using ProManLog.Core.DTOs;
using ProManLog.Core.Entities;
using ProManLog.Core.Interfaces;

namespace ProManLog.Core.Services;

public class PrestationService : IPrestationService
{
    public static readonly string[] StatutsAutorises =
        ["Planifiee", "EnCours", "Terminee", "Annulee"];

    private readonly IPrestationRepository _prestationRepository;
    private readonly IHistoriqueService _historiqueService;

    public PrestationService(IPrestationRepository prestationRepository, IHistoriqueService historiqueService)
    {
        _prestationRepository = prestationRepository;
        _historiqueService = historiqueService;
    }

    public async Task<IEnumerable<PrestationDto>> GetPrestationAsync(string? statut)
    {
        var prestations = await _prestationRepository.GetPrestationAsync(statut);
        return prestations.Select(VersDto).ToList();
    }

    public async Task<PrestationDto?> GetByIdAsync(int idPrestation)
    {
        var prestation = await _prestationRepository.GetByIdAsync(idPrestation);
        return prestation is null ? null : VersDto(prestation);
    }

    public async Task<PrestationDto> CreateAsync(CreatePrestationDto dto)
    {
        var prestation = VersEntite(dto);
        var prestationCreee = await _prestationRepository.AddAsync(prestation);

        await _historiqueService.EnregistrerAsync("Creation", "Prestation", prestationCreee.IdPrestation,
            $"Création de la prestation numéro {prestationCreee.IdPrestation} ({prestationCreee.TypeService})");

        return VersDto(prestationCreee);
    }

    public async Task<bool> UpdateAsync(int idPrestation, CreatePrestationDto dto)
    {
        var ancienne = await _prestationRepository.GetByIdAsync(idPrestation);
        if (ancienne is null) return false;

        var prestation = VersEntite(dto);
        prestation.IdPrestation = idPrestation;

        var resultat = await _prestationRepository.UpdateAsync(prestation);
        if (!resultat) return false;

        if (ancienne.Statut != prestation.Statut)
        {
            await _historiqueService.EnregistrerAsync("ChangementStatut", "Prestation", idPrestation,
                $"Prestation numéro {idPrestation} : {ancienne.Statut} vers {prestation.Statut}");
        }
        else
        {
            await _historiqueService.EnregistrerAsync("Modification", "Prestation", idPrestation,
                $"Modification de la prestation numéro {idPrestation}");
        }
        return true;
    }

    public async Task<bool> ChangerStatutAsync(int idPrestation, string statut)
    {
        if (!StatutsAutorises.Contains(statut)) return false;

        var ancienne = await _prestationRepository.GetByIdAsync(idPrestation);
        if (ancienne is null) return false;

        var resultat = await _prestationRepository.UpdateStatutAsync(idPrestation, statut);
        if (resultat)
        {
            await _historiqueService.EnregistrerAsync("ChangementStatut", "Prestation", idPrestation,
                $"Prestation numéro {idPrestation} : {ancienne.Statut} vers {statut}");
        }
        return resultat;
    }

    public async Task<bool> DeleteAsync(int idPrestation)
    {
        var resultat = await _prestationRepository.DeleteAsync(idPrestation);
        if (resultat)
        {
            await _historiqueService.EnregistrerAsync("Suppression", "Prestation", idPrestation,
                $"Suppression de la prestation numéro {idPrestation}");
        }
        return resultat;
    }

    private static Prestation VersEntite(CreatePrestationDto dto) => new()
    {
        IdClient = dto.IdClient,
        IdVehicule = dto.IdVehicule,
        IdEmploye = dto.IdEmploye,
        AdresseDepart = dto.AdresseDepart,
        AdresseArrivee = dto.AdresseArrivee,
        DateHeure = dto.DateHeure,
        TypeService = dto.TypeService,
        Prix = dto.Prix,
        Statut = dto.Statut
    };

    public static PrestationDto VersDto(Prestation prestation) => new()
    {
        IdPrestation = prestation.IdPrestation,
        IdClient = prestation.IdClient,
        IdVehicule = prestation.IdVehicule,
        IdEmploye = prestation.IdEmploye,
        AdresseDepart = prestation.AdresseDepart,
        AdresseArrivee = prestation.AdresseArrivee,
        DateHeure = prestation.DateHeure,
        TypeService = prestation.TypeService,
        Prix = prestation.Prix,
        Statut = prestation.Statut,
        NomClient = prestation.NomClient,
        ImmatriculationVehicule = prestation.ImmatriculationVehicule,
        NomEmploye = prestation.NomEmploye
    };
}
