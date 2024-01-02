using AutoMapper;
using ChauffeurApp.Application.DTOs;
using ChauffeurApp.Application.Helper;
using ChauffeurApp.Application.Services.IServices;
using ChauffeurApp.Core.Entities;
using ChauffeurApp.DataAccess.Repositories.IRepositories;
using FluentValidation;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ChauffeurApp.Application.Services
{
    public class VehicleService : IVehicleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IValidator<ApplicationUser> _validator;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IEmailService _emailService;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly ITokenService _tokenService;

        public VehicleService(IUnitOfWork unitOfWork, IMapper mapper, ITokenService tokenService, IHttpContextAccessor httpContextAccessor, IValidator<ApplicationUser> validator, IEmailService emailService, IHostingEnvironment hostingEnvironment, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _emailService = emailService;
            _hostingEnvironment = hostingEnvironment;
            _tokenService = tokenService;
            _userManager = userManager;
            _validator = validator;


        }
        public async Task<Result<VehicleDTO>> CreateVehicle(VehicleCreateDTO vehicleCreateDTO)
        {
            if (vehicleCreateDTO == null) return Result<VehicleDTO>.Failure("Invalid input");
            Vehicle vehicle = _mapper.Map<Vehicle>(vehicleCreateDTO);
            await _unitOfWork.Vehicles.AddAsync(vehicle);
            await _unitOfWork.SaveAsync();

            VehicleDTO vehicleDTO = _mapper.Map<VehicleDTO>(vehicle);
            return Result<VehicleDTO>.Success(vehicleDTO);
        }

        public Task<Result<bool>> DeleteVehicle(long id)
        {
            throw new NotImplementedException();
        }


        public async Task<Result<List<VehicleDTO>>> FilterVehicles(long? typeId, int? seatingCapacity, long? brandId)
        {
            var vehicles = await _unitOfWork.Vehicles.FindAsync(v => v.AvailabilityStatus);
            var query = vehicles.AsQueryable();

            // Filter by vehicle type
            if (typeId.HasValue)
            {
                query = query.Where(v => v.TypeID == typeId);
            }

            // Filter by brand ID
            if (brandId.HasValue)
            {
                query = query.Where(v => v.BrandID == brandId);
            }

            // Filter by seating capacity
            if (seatingCapacity.HasValue)
            {
                query = query.Where(v => v.SeatingCapacity >= seatingCapacity.Value);
            }

            var listOfVehicles = await query.ToListAsync();
            List<VehicleDTO> vehicleDTOs = _mapper.Map<List<VehicleDTO>>(listOfVehicles);
            return Result<List<VehicleDTO>>.Success(vehicleDTOs);
        }

        public Task<Result<bool>> SoftDeleteVehicle(long id)
        {
            throw new NotImplementedException();
        }

        public Task<Result<VehicleDTO>> UpdateVehicle(VehicleCreateDTO updatedDTO, long ID)
        {
            throw new NotImplementedException();
        }

        public async Task<Result<List<string>>> UploadFiles(List<IFormFile> _IFormFiles)
        {
            string email = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email);

            if (string.IsNullOrEmpty(email))
            {
                return Result<List<string>>.Failure("User email not found.");
            }

            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Email == email);

            if (user == null)
            {
                return Result<List<string>>.Failure("User not found.");
            }

            List<string> fileNames = new List<string>();

            long fileSizeLimit = 5 * 1024 * 1024; // 5 MB
            string[] allowedExtensions = { ".jpg", ".jpeg", ".png", ".gif" };

            foreach (var file in _IFormFiles)
            {
                if (file == null || file.Length == 0)
                {
                    return Result<List<string>>.Failure("One or more files are empty or not provided.");
                }

                if (file.Length > fileSizeLimit)
                {
                    return Result<List<string>>.Failure($"File size exceeds the limit of {fileSizeLimit / (1024 * 1024)} MB.");
                }

                var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
                if (!allowedExtensions.Contains(fileExtension))
                {
                    return Result<List<string>>.Failure($"Invalid file format for file {file.FileName}. Allowed formats: " + string.Join(", ", allowedExtensions));
                }

                try
                {
                    FileInfo fileInfo = new FileInfo(file.FileName);
                    string fileName = $"{Guid.NewGuid().ToString()}{fileExtension}";
                    var filePath = Common.GetFilePath(fileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }

                    fileNames.Add(fileName);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    return Result<List<string>>.Failure($"An error occurred while uploading the file {file.FileName}.");
                }
            }
 
            foreach (var fileName in fileNames)
            {
                user.ProfilePicturePath = fileName;
                var result = await _userManager.UpdateAsync(user);

                if (!result.Succeeded)
                {
                    return Result<List<string>>.Failure("Error updating user profile.");
                }
            }

            return Result<List<string>>.Success(fileNames);
        }


        public Task<Result<List<VehicleDTO>>> ViewAllVehicle()
        {
            throw new NotImplementedException();
        }

        public Task<Result<VehicleDTO>> ViewVehicleByID(long id)
        {
            throw new NotImplementedException();
        }
    }
}
