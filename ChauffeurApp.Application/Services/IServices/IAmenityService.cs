using ChauffeurApp.Application.DTOs;

namespace ChauffeurApp.Application.Services.IServices
{
    public interface IAmenityService
    {
        Task<Result<AmenityDTO>> CreateAmenity(AmenityCreateDTO amenityCreateDTO);
        Task<Result<List<AmenityDTO>>> ViewAllAmenities();
        Task<Result<AmenityDTO>> ViewAmenityByID(long id);
        Task<Result<bool>> DeleteAmenity(long id);
        Task<Result<AmenityDTO>> UpdateAmenity(AmenityCreateDTO updatedDTO, long ID);
        Task<Result<bool>> SoftDeleteAmenity(long id);
        Task<Result<VehicleAmenitiesDTO>> AddAmenitiesToVehicle(long vehicleID, List<long> amenityIds);
    }
}
