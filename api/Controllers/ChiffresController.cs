using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProManLog.Core.DTOs;
using ProManLog.Core.Interfaces;

namespace ProManLog.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]

public class ChiffresController : ControllerBase
{
    private readonly IChiffresService _chiffresService;
    public ChiffresController(IChiffresService chiffresService)
    {
        _chiffresService = chiffresService;
    }

    [HttpGet]
    public async Task<ActionResult<ChiffresDto>> GetChiffresAsync()
    {
        var chiffres = await _chiffresService.GetChiffresAsync();
        return Ok(chiffres);
    }
}
