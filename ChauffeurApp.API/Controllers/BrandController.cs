using ChauffeurApp.Application.DTOs;
using ChauffeurApp.Application.Services.IServices;
using ChauffeurApp.Core.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChauffeurApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandController : BaseAPIController
    {
        private readonly IBrandService _brandService;
        public BrandController(IBrandService brandService)
        {
            _brandService = brandService;
        }

        //[Authorize(Roles = "Admin")]
        [HttpPost("CreateBrand")]
        public async Task<IActionResult> CreateBrand(BrandCreateDTO createBrandDTO)
        {
            return HandleResult(await _brandService.CreateBrand(createBrandDTO));
        }

        //[Authorize(Roles = "Company")]
        [AllowAnonymous]
        [HttpGet("GetAllBrands")]
        public async Task<IActionResult> GetAllBrands()
        {
            return HandleResult(await _brandService.ViewAllBrands());
        }

        [Authorize(Roles = "Chauffeur")]
        [HttpGet("GetBrandByID")]
        public async Task<IActionResult> GetBrandByID(long ID)
        {
            return HandleResult(await _brandService.ViewBrandByID(ID));
        }

        [Authorize(Roles = "Passenger")]
        [HttpDelete("Deletebrand")]
        public async Task<IActionResult> DeleteBrand(long ID)
        {
            return HandleResult(await _brandService.DeleteBrand(ID));   
        }

        [HttpPut("UpdateBrand")]
        public async Task<IActionResult> UpdateBrand(BrandCreateDTO updateBrandDTO, long ID)
        {
            return HandleResult(await _brandService.UpdateBrand(updateBrandDTO, ID));
        }

        [HttpGet("SoftDelete")]
        public async Task<IActionResult> SoftDeleteBrand(long ID)
        {
            return HandleResult(await _brandService.SoftDeleteBrand(ID));
        }
    }
}
