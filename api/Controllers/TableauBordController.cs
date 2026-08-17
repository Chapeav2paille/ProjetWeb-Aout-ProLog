using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProManLog.Core.DTOs;
using ProManLog.Core.Interfaces;

namespace ProManLog.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]

public class TableauBordController : ControllerBase
{
    private readonly ITableauBordService _tableauBordService;
    public TableauBordController(ITableauBordService tableauBordService)
    {
        _tableauBordService = tableauBordService;
    }

    [HttpGet]
    public async Task<ActionResult<TableauBordDto>> GetTableauBordAsync()
    {
        var tableauBord = await _tableauBordService.GetTableauBordAsync();
        return Ok(tableauBord);
    }
}
