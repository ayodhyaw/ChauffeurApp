using ChauffeurApp.Core.Entities;
using ChauffeurApp.DataAccess.Persistence;
using ChauffeurApp.DataAccess.Repositories.IRepositories;

namespace ChauffeurApp.DataAccess.Repositories
{
    public class AmenityRepository : GenericRepository<Amenities>, IAmenityRepository
    {
        private readonly AppDbContext _context;
        public AmenityRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
