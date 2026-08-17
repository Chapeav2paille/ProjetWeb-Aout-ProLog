using ProManLog.Core.DTOs;

namespace ProManLog.Core.Interfaces;
public interface IChiffresService
{
    Task<ChiffresDto> GetChiffresAsync();
}
