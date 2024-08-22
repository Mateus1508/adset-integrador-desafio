using AdSetSolution.Domain.Enums;
using AdSetSolution.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace AdSetSolution.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
        {
        }

        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<Package> Packages { get; set; }
        public DbSet<Optional> Optional { get; set; }
        public DbSet<VehicleImg> VehicleImgs { get; set; }
        public DbSet<VehiclePackage> VehiclePackages { get; set; }
        public DbSet<VehicleOptional> VehicleOptional { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<VehiclePackage>()
                .HasKey(vp => new { vp.VehicleId, vp.PackageId, vp.PortalType });

            modelBuilder.Entity<Vehicle>()
                .HasMany(v => v.VehiclePackages)
                .WithOne(vp => vp.Vehicle)
                .HasForeignKey(vp => vp.VehicleId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Package>()
                .HasMany(p => p.VehiclePackages)
                .WithOne(vp => vp.Package)
                .HasForeignKey(vp => vp.PackageId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Package>()
               .Property(p => p.PortalType)
               .HasConversion(
                   value => (int)value,
                   value => (PortalType)value
               );

            modelBuilder.Entity<VehiclePackage>()
                .Property(vp => vp.PortalType)
                .HasConversion(
                    value => (int)value,
                    value => (PortalType)value
                );

            modelBuilder.Entity<VehicleOptional>()
                .HasKey(vo => new { vo.VehicleId, vo.OptionalId });

            modelBuilder.Entity<Vehicle>()
                .HasMany(v => v.VehicleOptional)
                .WithOne(vo => vo.Vehicle)
                .HasForeignKey(vo => vo.VehicleId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Optional>()
                .HasMany(o => o.VehicleOptional)
                .WithOne(vo => vo.Optional)
                .HasForeignKey(vo => vo.OptionalId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Vehicle>()
                .HasMany(v => v.VehicleImgs)
                .WithOne(vi => vi.Vehicle)
                .HasForeignKey(vi => vi.VehicleId)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
