using SB_Domain.Entities;

namespace SB_Application.Interfaces
{
    public interface ISB_VehicleRepository
    {
        Task<SB_Vehicle?> GetByIdAsync(Guid id);
        Task<IEnumerable<SB_Vehicle>> GetAllAsync();
        Task AddAsync(SB_Vehicle vehicle);
        Task UpdateAsync(SB_Vehicle vehicle);
        Task DeleteAsync(Guid id);
    }
}
