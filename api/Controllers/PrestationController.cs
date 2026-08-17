using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProManLog.Core.DTOs;
using ProManLog.Core.Interfaces;
using ProManLog.Core.Services;

namespace ProManLog.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]

public class PrestationController : ControllerBase
{
    private readonly IPrestationService _prestationService;
    public PrestationController(IPrestationService prestationService)
    {
        _prestationService = prestationService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PrestationDto>>> GetPrestationAsync([FromQuery] string? statut)
    {
        var prestation = await _prestationService.GetPrestationAsync(statut);
        return Ok(prestation);
    }

    [HttpGet("{id}", Name = "GetPrestationById")]
    public async Task<ActionResult<PrestationDto>> GetByIdAsync(int id)
    {
        var prestation = await _prestationService.GetByIdAsync(id);
        if (prestation is null) return NotFound();
        return Ok(prestation);
    }

    [HttpPost]
    public async Task<ActionResult<PrestationDto>> CreateAsync(CreatePrestationDto dto)
    {
        var prestation = await _prestationService.CreateAsync(dto);
        return CreatedAtRoute(("GetPrestationById"), new { id = prestation.IdPrestation }, prestation);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<bool>> UpdateAsync(int id, CreatePrestationDto dto)
    {
        var prestation = await _prestationService.UpdateAsync(id, dto);
        if (prestation is false) return NotFound();
        return NoContent();
    }

    [HttpPatch("{id}/statut")]
    public async Task<ActionResult<bool>> ChangerStatutAsync(int id, ChangerStatutDto dto)
    {
        if (!PrestationService.StatutsAutorises.Contains(dto.Statut))
            return BadRequest(new { message = "Statut invalide." });

        var prestation = await _prestationService.ChangerStatutAsync(id, dto.Statut);
        if (prestation is false) return NotFound();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<bool>> DeleteAsync(int id)
    {
        var prestation = await _prestationService.DeleteAsync(id);
        if (prestation is false) return NotFound();
        return NoContent();
    }
}
