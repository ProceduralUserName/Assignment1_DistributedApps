using Microsoft.EntityFrameworkCore;
using SB_Domain.Entities;
using SB_Domain.Enums;
using SB_Domain.ValueObjects;

namespace SB_Infrastructure.Persistence
{
    public class SB_InventoryDbContext : DbContext
    {
        public DbSet<SB_Vehicle> Vehicles => Set<SB_Vehicle>();

        public SB_InventoryDbContext(DbContextOptions<SB_InventoryDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SB_Vehicle>(entity =>
            {
                entity.HasKey(v => v.Id);
                entity.Property(v => v.VehicleCode)
                      .HasConversion(
                          vc => vc.Value,
                          s => new SB_VehicleCode(s))
                      .IsRequired();
                entity.Property(v => v.VehicleType)
                      .HasConversion(
                          vt => vt.Value,
                          s => new SB_VehicleType(s))
                      .IsRequired();
                entity.Property(v => v.Status)
                      .HasConversion<string>();
            });
        }
    }
}
