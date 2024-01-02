using ChauffeurApp.Core.Common;
using ChauffeurApp.Core.Entities;
using ChauffeurApp.Shared.Services;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ChauffeurApp.DataAccess.Persistence
{
    public class AppDbContext : IdentityDbContext<ApplicationUser, ApplicationUserRole, long>
    {
        private readonly IClaimService _claimService;
        public AppDbContext(DbContextOptions options, IClaimService claimService) : base(options)
        {
            _claimService = claimService;
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Brand>()
               .HasMany(e => e.Vehicles)
               .WithOne(e => e.Brand)
               .HasForeignKey(e => e.BrandID)
               .IsRequired(false);

            builder.Entity<VehicleType>()
               .HasMany(e => e.Vehicles)
               .WithOne(e => e.VehicleType)
               .HasForeignKey(e => e.TypeID)
               .IsRequired(false);

            builder.Entity<VehicleAmenities>()
               .HasKey(pc => new { pc.VehicleId, pc.AmenityId });
            builder.Entity<VehicleAmenities>()
                .HasOne(p => p.Vehicle)
                .WithMany(pc => pc.VehicleAmenities)
                .HasForeignKey(p => p.VehicleId);
            builder.Entity<VehicleAmenities>()
                .HasOne(p => p.Amenities)
                .WithMany(pc => pc.VehicleAmenities)
                .HasForeignKey(p => p.AmenityId);

            builder.Entity<Vehicle>()
                .HasMany(v => v.vehicleImages)
                .WithOne(v => v.Vehicle)
                .HasForeignKey(v => v.VehicleID)
                .IsRequired(false);
                


        }
        public DbSet<VehicleImages> Vimages { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<VehicleType> VehicleTypes { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<ApplicationUser> Users { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<Amenities> Amenities { get; set; }
        public DbSet<VehicleAmenities> VehicleAmenities { get; set; }

        public new async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
        {
            foreach (var entry in ChangeTracker.Entries<IAuditedEntity>())
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedById = ConvertToNullableLong(_claimService.GetUserId());
                        entry.Entity.CreatedAt = DateTime.Now;
                        break;
                    case EntityState.Modified:
                        entry.Entity.UpdatedById = ConvertToNullableLong(_claimService.GetUserId());
                        entry.Entity.UpdatedAt = DateTime.Now;
                        break;
                    case EntityState.Deleted:
                        entry.Entity.DeletedById = ConvertToNullableLong(_claimService.GetUserId());
                        entry.Entity.DeletedAt = DateTime.Now;
                        break;
                }

            return await base.SaveChangesAsync(cancellationToken);
        }

        private long? ConvertToNullableLong(string value)
        {
            if (long.TryParse(value, out var result))
            {
                return result;
            }
            return null;
        }
    }
}
