using Microsoft.EntityFrameworkCore;
using SB_Application.Interfaces;
using SB_Domain.Entities;
using SB_Infrastructure.Persistence;

namespace SB_Infrastructure.Repositories
{
    public class SB_VehicleRepository : ISB_VehicleRepository
    {
        private readonly SB_InventoryDbContext _context;

        public SB_VehicleRepository(SB_InventoryDbContext context)
        {
            _context = context;
        }

        public async Task<SB_Vehicle?> GetByIdAsync(Guid id)
        {
            return await _context.Vehicles.FindAsync(id);
        }

        public async Task<IEnumerable<SB_Vehicle>> GetAllAsync()
        {
            return await _context.Vehicles.ToListAsync();
        }

        public async Task AddAsync(SB_Vehicle vehicle)
        {
            await _context.Vehicles.AddAsync(vehicle);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(SB_Vehicle vehicle)
        {
            _context.Vehicles.Update(vehicle);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var vehicle = await _context.Vehicles.FindAsync(id);
            if (vehicle != null)
            {
                _context.Vehicles.Remove(vehicle);
                await _context.SaveChangesAsync();
            }
        }
    }
}
