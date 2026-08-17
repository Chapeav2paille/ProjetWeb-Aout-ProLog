using ProManLog.Core.Entities;

namespace ProManLog.Core.Interfaces;
public interface IChargeRepository
{
    Task<IEnumerable<Charge>> GetDepuisAsync(DateTime periodeDebut);
}
