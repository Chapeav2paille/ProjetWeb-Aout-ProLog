using Microsoft.AspNetCore.Mvc;
using ProManLog.Core.DTOs;
using ProManLog.Core.Interfaces;

namespace ProManLog.Api.Controllers;

[ApiController]
[Route("api/[controller]")]

public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("connexion")]
    public async Task<ActionResult<UtilisateurConnecteDto>> ConnexionAsync(ConnexionDto dto)
    {
        var utilisateur = await _authService.ConnexionAsync(dto);
        if (utilisateur is null) return Unauthorized(new { message = "Identifiants invalides" });
        return Ok(utilisateur);
    }
}
