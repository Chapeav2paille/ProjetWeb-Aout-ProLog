using ProManLog.Core.DTOs;
using ProManLog.Core.Entities;
using ProManLog.Core.Interfaces;

namespace ProManLog.Core.Services;

public class VehiculeService : IVehiculeService
{
    private readonly IVehiculeRepository _vehiculeRepository;
    private readonly IHistoriqueService _historiqueService;

    public VehiculeService(IVehiculeRepository vehiculeRepository, IHistoriqueService historiqueService)
    {
        _vehiculeRepository = vehiculeRepository;
        _historiqueService = historiqueService;
    }

    public async Task<IEnumerable<VehiculeDto>> GetVehiculeAsync()
    {
        var vehicules = await _vehiculeRepository.GetVehiculeAsync();
        return vehicules.Select(VersDto).ToList();
    }

    public async Task<VehiculeDto?> GetByIdAsync(int idVehicule)
    {
        var vehicule = await _vehiculeRepository.GetByIdAsync(idVehicule);
        return vehicule is null ? null : VersDto(vehicule);
    }

    public async Task<VehiculeDto> CreateAsync(CreateVehiculeDto dto)
    {
        var vehicule = VersEntite(dto);
        var vehiculeCree = await _vehiculeRepository.AddAsync(vehicule);

        await _historiqueService.EnregistrerAsync("Creation", "Vehicule", vehiculeCree.IdVehicule,
            $"Ajout du véhicule {vehiculeCree.Immatriculation} ({vehiculeCree.TypeVehicule})");

        return VersDto(vehiculeCree);
    }

    public async Task<bool> UpdateAsync(int idVehicule, CreateVehiculeDto dto)
    {
        var ancien = await _vehiculeRepository.GetByIdAsync(idVehicule);
        if (ancien is null) return false;

        var vehicule = VersEntite(dto);
        vehicule.IdVehicule = idVehicule;

        var resultat = await _vehiculeRepository.UpdateAsync(vehicule);
        if (!resultat) return false;

        if (ancien.Statut != vehicule.Statut)
        {
            await _historiqueService.EnregistrerAsync("ChangementStatut", "Vehicule", idVehicule,
                $"Véhicule {vehicule.Immatriculation} : {ancien.Statut} vers {vehicule.Statut}");
        }
        else
        {
            await _historiqueService.EnregistrerAsync("Modification", "Vehicule", idVehicule,
                $"Modification du véhicule {vehicule.Immatriculation}");
        }
        return true;
    }

    public async Task<bool> DeleteAsync(int idVehicule)
    {
        var vehicule = await _vehiculeRepository.GetByIdAsync(idVehicule);
        if (vehicule is null) return false;

        var resultat = await _vehiculeRepository.DeleteAsync(idVehicule);
        if (resultat)
        {
            await _historiqueService.EnregistrerAsync("Suppression", "Vehicule", idVehicule,
                $"Suppression du véhicule {vehicule.Immatriculation}");
        }
        return resultat;
    }

    private static Vehicule VersEntite(CreateVehiculeDto dto) => new()
    {
        Immatriculation = dto.Immatriculation,
        TypeVehicule = dto.TypeVehicule,
        CapaciteKg = dto.CapaciteKg,
        Kilometrage = dto.Kilometrage,
        Statut = dto.Statut,
        DernierEntretien = dto.DernierEntretien,
        ProchainEntretien = dto.ProchainEntretien,
        ProchainControleTechnique = dto.ProchainControleTechnique,
        FinAssurance = dto.FinAssurance
    };

    public static VehiculeDto VersDto(Vehicule vehicule) => new()
    {
        IdVehicule = vehicule.IdVehicule,
        Immatriculation = vehicule.Immatriculation,
        TypeVehicule = vehicule.TypeVehicule,
        CapaciteKg = vehicule.CapaciteKg,
        Kilometrage = vehicule.Kilometrage,
        Statut = vehicule.Statut,
        DernierEntretien = vehicule.DernierEntretien,
        ProchainEntretien = vehicule.ProchainEntretien,
        ProchainControleTechnique = vehicule.ProchainControleTechnique,
        FinAssurance = vehicule.FinAssurance
    };
}
