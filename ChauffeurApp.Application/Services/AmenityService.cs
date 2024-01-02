using AutoMapper;
using ChauffeurApp.Application.DTOs;
using ChauffeurApp.Application.Services.IServices;
using ChauffeurApp.Core.Entities;
using ChauffeurApp.DataAccess.Repositories.IRepositories;

namespace ChauffeurApp.Application.Services
{
    public class AmenityService : IAmenityService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public AmenityService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public Task<Result<VehicleAmenitiesDTO>> AddAmenitiesToVehicle(long vehicleID, List<long> amenityIds)
        {
            throw new NotImplementedException();
        }

        //public async Task<Result<VehicleAmenitiesDTO>> AddAmenitiesToVehicle(long vehicleId, List<long> amenityIds)
        //{
        //    var vehicle = await _unitOfWork.Vehicles.GetByIdAsync(vehicleId);

        //    if (vehicle == null)
        //    {
        //        return Result<VehicleAmenitiesDTO>.Failure($"Vehicle with ID {vehicleId} not found.");
        //    }


        //    var amenities = await _unitOfWork.Amenities.FindAsync(a => amenityIds.Contains(a.Id));


        //    foreach (var amenity in amenities)
        //    {
        //        var vehicleAmenity = new VehicleAmenities
        //        {
        //            Vehicle = vehicle,
        //            Amenities = amenity
        //        };

        //        vehicle.VehicleAmenities ??= new List<VehicleAmenities>();
        //        vehicle.VehicleAmenities.Add(vehicleAmenity);
        //    }


        //    _unitOfWork.Vehicles.Update(vehicle);
        //    await _unitOfWork.SaveAsync();

        //   // return Result<VehicleAmenitiesDTO>.Success(true);
        //}

        public async Task<Result<AmenityDTO>> CreateAmenity(AmenityCreateDTO amenityCreateDTO)
        {
            if (amenityCreateDTO == null) return Result<AmenityDTO>.Failure("Invalid input");
            Amenities amenity = _mapper.Map<Amenities>(amenityCreateDTO);

            await _unitOfWork.Amenities.AddAsync(amenity);
            await _unitOfWork.SaveAsync();

            AmenityDTO amenityDTO = _mapper.Map<AmenityDTO>(amenity);

            return Result<AmenityDTO>.Success(amenityDTO);
        }

        public Task<Result<bool>> DeleteAmenity(long id)
        {
            throw new NotImplementedException();
        }

        public Task<Result<bool>> SoftDeleteAmenity(long id)
        {
            throw new NotImplementedException();
        }

        public Task<Result<AmenityDTO>> UpdateAmenity(AmenityCreateDTO updatedDTO, long ID)
        {
            throw new NotImplementedException();
        }

        public Task<Result<List<AmenityDTO>>> ViewAllAmenities()
        {
            throw new NotImplementedException();
        }

        public Task<Result<AmenityDTO>> ViewAmenityByID(long id)
        {
            throw new NotImplementedException();
        }
    }
}
