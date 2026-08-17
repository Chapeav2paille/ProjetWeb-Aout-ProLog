using ProManLog.Core.DTOs;
using ProManLog.Core.Entities;
using ProManLog.Core.Interfaces;

namespace ProManLog.Core.Services;

public class EmployeService : IEmployeService
{
    private const string PosteChauffeur = "Chauffeur";

    private readonly IEmployeRepository _employeRepository;
    private readonly IHistoriqueService _historiqueService;

    public EmployeService(IEmployeRepository employeRepository, IHistoriqueService historiqueService)
    {
        _employeRepository = employeRepository;
        _historiqueService = historiqueService;
    }

    public async Task<IEnumerable<EmployeDto>> GetEmployeAsync()
    {
        var employes = await _employeRepository.GetEmployeAsync();
        return employes.Select(VersDto).ToList();
    }

    public async Task<EmployeDto?> GetByIdAsync(int idEmploye)
    {
        var employe = await _employeRepository.GetByIdAsync(idEmploye);
        return employe is null ? null : VersDto(employe);
    }

    public async Task<EmployeDto> CreateAsync(CreateEmployeDto dto)
    {
        var employe = VersEntite(dto);
        var employeCree = await _employeRepository.AddAsync(employe);

        await _historiqueService.EnregistrerAsync("Creation", "Employe", employeCree.IdEmploye,
            $"Ajout de l'employé {employeCree.Prenom} {employeCree.Nom} ({employeCree.Poste})");

        return VersDto(employeCree);
    }

    public async Task<bool> UpdateAsync(int idEmploye, CreateEmployeDto dto)
    {
        var employe = VersEntite(dto);
        employe.IdEmploye = idEmploye;

        var resultat = await _employeRepository.UpdateAsync(employe);
        if (resultat)
        {
            await _historiqueService.EnregistrerAsync("Modification", "Employe", idEmploye,
                $"Modification de l'employé {employe.Prenom} {employe.Nom}");
        }
        return resultat;
    }

    public async Task<bool> DeleteAsync(int idEmploye)
    {
        var employe = await _employeRepository.GetByIdAsync(idEmploye);
        if (employe is null) return false;

        var resultat = await _employeRepository.DeleteAsync(idEmploye);
        if (resultat)
        {
            await _historiqueService.EnregistrerAsync("Suppression", "Employe", idEmploye,
                $"Suppression de l'employé {employe.Prenom} {employe.Nom}");
        }
        return resultat;
    }

    private static Employe VersEntite(CreateEmployeDto dto)
    {
        var estChauffeur = dto.Poste == PosteChauffeur;
        return new Employe
        {
            Nom = dto.Nom,
            Prenom = dto.Prenom,
            Poste = dto.Poste,
            DateEmbauche = dto.DateEmbauche,
            Email = dto.Email,
            Telephone = dto.Telephone,
            Statut = dto.Statut,
            Disponibilite = dto.Disponibilite,
            NumeroPermis = estChauffeur ? dto.NumeroPermis : string.Empty,
            CategoriesPermis = estChauffeur ? dto.CategoriesPermis : string.Empty,
            ExpirationPermis = estChauffeur ? dto.ExpirationPermis : null
        };
    }

    public static EmployeDto VersDto(Employe employe) => new()
    {
        IdEmploye = employe.IdEmploye,
        Nom = employe.Nom,
        Prenom = employe.Prenom,
        Poste = employe.Poste,
        DateEmbauche = employe.DateEmbauche,
        Email = employe.Email,
        Telephone = employe.Telephone,
        Statut = employe.Statut,
        Disponibilite = employe.Disponibilite,
        NumeroPermis = employe.NumeroPermis,
        CategoriesPermis = employe.CategoriesPermis,
        ExpirationPermis = employe.ExpirationPermis
    };
}
