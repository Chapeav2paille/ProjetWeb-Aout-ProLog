using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProManLog.Core.DTOs;
using ProManLog.Core.Interfaces;

namespace ProManLog.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]

public class VehiculeController : ControllerBase
{
    private readonly IVehiculeService _vehiculeService;
    public VehiculeController(IVehiculeService vehiculeService)
    {
        _vehiculeService = vehiculeService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<VehiculeDto>>> GetVehiculeAsync()
    {
        var vehicule = await _vehiculeService.GetVehiculeAsync();
        return Ok(vehicule);
    }

    [HttpGet("{id}", Name = "GetVehiculeById")]
    public async Task<ActionResult<VehiculeDto>> GetByIdAsync(int id)
    {
        var vehicule = await _vehiculeService.GetByIdAsync(id);
        if (vehicule is null) return NotFound();
        return Ok(vehicule);
    }

    [HttpPost]
    public async Task<ActionResult<VehiculeDto>> CreateAsync(CreateVehiculeDto dto)
    {
        var vehicule = await _vehiculeService.CreateAsync(dto);
        return CreatedAtRoute(("GetVehiculeById"), new { id = vehicule.IdVehicule }, vehicule);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<bool>> UpdateAsync(int id, CreateVehiculeDto dto)
    {
        var vehicule = await _vehiculeService.UpdateAsync(id, dto);
        if (vehicule is false) return NotFound();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<bool>> DeleteAsync(int id)
    {
        var vehicule = await _vehiculeService.DeleteAsync(id);
        if (vehicule is false) return Conflict(new { message = "Suppression impossible : ce véhicule est utilisé par des prestations." });
        return NoContent();
    }
}
