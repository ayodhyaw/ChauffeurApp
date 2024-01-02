using ChauffeurApp.Core.Entities;
using ChauffeurApp.DataAccess.Persistence;
using ChauffeurApp.DataAccess.Repositories.IRepositories;

namespace ChauffeurApp.DataAccess.Repositories
{
    public class VehicleRepository : GenericRepository<Vehicle>, IVehicleRepository
    {
        private readonly AppDbContext _context;
        public VehicleRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
