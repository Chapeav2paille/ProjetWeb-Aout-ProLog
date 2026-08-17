using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProManLog.Core.DTOs;
using ProManLog.Core.Interfaces;

namespace ProManLog.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]

public class HistoriqueController : ControllerBase
{
    private readonly IHistoriqueService _historiqueService;
    public HistoriqueController(IHistoriqueService historiqueService)
    {
        _historiqueService = historiqueService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<HistoriqueDto>>> GetHistoriqueAsync(
        [FromQuery] DateTime? du, [FromQuery] DateTime? au, [FromQuery] string? typeAction)
    {
        var historique = await _historiqueService.GetHistoriqueAsync(du, au, typeAction);
        return Ok(historique);
    }
}
