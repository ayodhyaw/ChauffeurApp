using ChauffeurApp.Application.DTOs;
using ChauffeurApp.Application.Services;
using ChauffeurApp.Application.Services.IServices;
using Microsoft.AspNetCore.Mvc;

namespace ChauffeurApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehicleController : BaseAPIController
    {
        private readonly IVehicleService _vehicleService;
        public VehicleController(IVehicleService vehicleService)
        {
            _vehicleService = vehicleService;
        }

        [HttpPost("CreateVehicle")]
        public async Task<IActionResult> CreateVehicle(VehicleCreateDTO vehicleCreateDTO)
        {
            return HandleResult(await _vehicleService.CreateVehicle(vehicleCreateDTO));
        }

        [HttpGet("FilterVehicle")]
        public async Task<IActionResult> FilterVehicle(long? typeId, int? seatingCapacity, long? brandId)
        {
            return HandleResult(await _vehicleService.FilterVehicles(typeId, seatingCapacity, brandId));  
        }
        [HttpPost]
        [Route("uploadfiles")]
        public async Task<IActionResult> UploadFiles(List<IFormFile> _IFormFiles)
        {
            return HandleResult(await _vehicleService.UploadFiles(_IFormFiles));
        }


    }
}
