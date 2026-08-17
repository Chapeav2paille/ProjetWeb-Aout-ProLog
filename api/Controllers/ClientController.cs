using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProManLog.Core.DTOs;
using ProManLog.Core.Interfaces;

namespace ProManLog.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]

public class ClientController : ControllerBase
{
    private readonly IClientService _clientService;
    public ClientController(IClientService clientService)
    {
        _clientService = clientService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ClientDto>>> GetClientAsync()
    {
        var client = await _clientService.GetClientAsync();
        return Ok(client);
    }

    [HttpGet("{id}", Name = "GetClientById")]
    public async Task<ActionResult<ClientDto>> GetByIdAsync(int id)
    {
        var client = await _clientService.GetByIdAsync(id);
        if (client is null) return NotFound();
        return Ok(client);
    }

    [HttpGet("{id}/prestations")]
    public async Task<ActionResult<IEnumerable<PrestationDto>>> GetPrestationsAsync(int id)
    {
        var prestations = await _clientService.GetPrestationsAsync(id);
        return Ok(prestations);
    }

    [HttpPost]
    public async Task<ActionResult<ClientDto>> CreateAsync(CreateClientDto dto)
    {
        var client = await _clientService.CreateAsync(dto);
        return CreatedAtRoute(("GetClientById"), new { id = client.IdClient }, client);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<bool>> UpdateAsync(int id, CreateClientDto dto)
    {
        var client = await _clientService.UpdateAsync(id, dto);
        if (client is false) return NotFound();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<bool>> DeleteAsync(int id)
    {
        var client = await _clientService.DeleteAsync(id);
        if (client is false) return Conflict(new { message = "Suppression impossible : des prestations sont rattachées à ce client." });
        return NoContent();
    }
}
