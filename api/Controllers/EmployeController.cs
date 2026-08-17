using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProManLog.Core.DTOs;
using ProManLog.Core.Interfaces;

namespace ProManLog.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]

public class EmployeController : ControllerBase
{
    private readonly IEmployeService _employeService;
    public EmployeController(IEmployeService employeService)
    {
        _employeService = employeService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<EmployeDto>>> GetEmployeAsync()
    {
        var employe = await _employeService.GetEmployeAsync();
        return Ok(employe);
    }

    [HttpGet("{id}", Name = "GetEmployeById")]
    public async Task<ActionResult<EmployeDto>> GetByIdAsync(int id)
    {
        var employe = await _employeService.GetByIdAsync(id);
        if (employe is null) return NotFound();
        return Ok(employe);
    }

    [HttpPost]
    public async Task<ActionResult<EmployeDto>> CreateAsync(CreateEmployeDto dto)
    {
        var employe = await _employeService.CreateAsync(dto);
        return CreatedAtRoute(("GetEmployeById"), new { id = employe.IdEmploye }, employe);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<bool>> UpdateAsync(int id, CreateEmployeDto dto)
    {
        var employe = await _employeService.UpdateAsync(id, dto);
        if (employe is false) return NotFound();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<bool>> DeleteAsync(int id)
    {
        var employe = await _employeService.DeleteAsync(id);
        if (employe is false) return Conflict(new { message = "Suppression impossible : cet employé est assigné à des prestations." });
        return NoContent();
    }
}
