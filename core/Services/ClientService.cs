using ProManLog.Core.DTOs;
using ProManLog.Core.Entities;
using ProManLog.Core.Interfaces;

namespace ProManLog.Core.Services;

public class ClientService : IClientService
{
    private readonly IClientRepository _clientRepository;
    private readonly IPrestationRepository _prestationRepository;
    private readonly IHistoriqueService _historiqueService;

    public ClientService(
        IClientRepository clientRepository,
        IPrestationRepository prestationRepository,
        IHistoriqueService historiqueService)
    {
        _clientRepository = clientRepository;
        _prestationRepository = prestationRepository;
        _historiqueService = historiqueService;
    }

    public async Task<IEnumerable<ClientDto>> GetClientAsync()
    {
        var clients = await _clientRepository.GetClientAsync();
        return clients.Select(VersDto).ToList();
    }

    public async Task<ClientDto?> GetByIdAsync(int idClient)
    {
        var client = await _clientRepository.GetByIdAsync(idClient);
        return client is null ? null : VersDto(client);
    }

    public async Task<IEnumerable<PrestationDto>> GetPrestationsAsync(int idClient)
    {
        var prestations = await _prestationRepository.GetByClientAsync(idClient);
        return prestations.Select(PrestationService.VersDto).ToList();
    }

    public async Task<ClientDto> CreateAsync(CreateClientDto dto)
    {
        var client = new Client
        {
            TypeClient = dto.TypeClient,
            Nom = dto.Nom,
            Contact = dto.Contact,
            Email = dto.Email,
            Telephone = dto.Telephone,
            Adresse = dto.Adresse
        };

        var clientCree = await _clientRepository.AddAsync(client);
        await _historiqueService.EnregistrerAsync("Creation", "Client", clientCree.IdClient,
            $"Création du client {clientCree.Nom}");

        return VersDto(clientCree);
    }

    public async Task<bool> UpdateAsync(int idClient, CreateClientDto dto)
    {
        var client = new Client
        {
            IdClient = idClient,
            TypeClient = dto.TypeClient,
            Nom = dto.Nom,
            Contact = dto.Contact,
            Email = dto.Email,
            Telephone = dto.Telephone,
            Adresse = dto.Adresse
        };

        var resultat = await _clientRepository.UpdateAsync(client);
        if (resultat)
        {
            await _historiqueService.EnregistrerAsync("Modification", "Client", idClient,
                $"Modification du client {client.Nom}");
        }
        return resultat;
    }

    public async Task<bool> DeleteAsync(int idClient)
    {
        var client = await _clientRepository.GetByIdAsync(idClient);
        if (client is null) return false;

        var resultat = await _clientRepository.DeleteAsync(idClient);
        if (resultat)
        {
            await _historiqueService.EnregistrerAsync("Suppression", "Client", idClient,
                $"Suppression du client {client.Nom}");
        }
        return resultat;
    }

    public static ClientDto VersDto(Client client) => new()
    {
        IdClient = client.IdClient,
        TypeClient = client.TypeClient,
        Nom = client.Nom,
        Contact = client.Contact,
        Email = client.Email,
        Telephone = client.Telephone,
        Adresse = client.Adresse
    };
}
