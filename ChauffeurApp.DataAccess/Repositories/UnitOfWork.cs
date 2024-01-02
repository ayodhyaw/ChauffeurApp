using ChauffeurApp.DataAccess.Persistence;
using ChauffeurApp.DataAccess.Repositories.IRepositories;

namespace ChauffeurApp.DataAccess.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _db;
        public UnitOfWork(AppDbContext db)
        {
            _db = db;
            Brands = new BrandRepository(_db);
            Vehicles = new VehicleRepository(_db);
            Amenities = new AmenityRepository(_db);
        }
        public IBrandRepository Brands { get; private set; }
        public IVehicleRepository Vehicles { get; private set; }
        public IAmenityRepository Amenities { get; private set; }

        public void Dispose()
        {
            _db.Dispose();
        }

        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}
