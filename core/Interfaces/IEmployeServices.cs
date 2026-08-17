using ProManLog.Core.DTOs;

namespace ProManLog.Core.Interfaces;
public interface IEmployeService
{
    Task<IEnumerable<EmployeDto>> GetEmployeAsync();
    Task<EmployeDto?> GetByIdAsync(int idEmploye);
    Task<EmployeDto> CreateAsync(CreateEmployeDto dto);
    Task<bool> UpdateAsync(int idEmploye, CreateEmployeDto dto);
    Task<bool> DeleteAsync(int idEmploye);
}
