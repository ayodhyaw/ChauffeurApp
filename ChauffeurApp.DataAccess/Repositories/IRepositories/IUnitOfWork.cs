namespace ChauffeurApp.DataAccess.Repositories.IRepositories
{
    public interface IUnitOfWork : IDisposable
    {
       IBrandRepository Brands { get; }
       IVehicleRepository Vehicles { get; }
       IAmenityRepository Amenities { get; }
       Task SaveAsync();
    }
}
