using ChauffeurApp.Application.DTOs;

namespace ChauffeurApp.Application.Services.IServices
{
    public interface IBrandService
    {
        Task<Result<BrandDTO>> CreateBrand(BrandCreateDTO brandDTO);
        Task<Result<List<BrandDTO>>> ViewAllBrands();
        Task<Result<BrandDTO>> ViewBrandByID (long id);
        Task<Result<bool>> DeleteBrand(long id);
        Task<Result<BrandDTO>> UpdateBrand(BrandCreateDTO updatedDTO, long ID);
        Task<Result<bool>> SoftDeleteBrand(long id);
        
    }
}
