using ProManLog.Core.DTOs;

namespace ProManLog.Core.Interfaces;
public interface ITableauBordService
{
    Task<TableauBordDto> GetTableauBordAsync();
}
