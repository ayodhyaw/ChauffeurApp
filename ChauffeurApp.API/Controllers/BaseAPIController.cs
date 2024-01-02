using ChauffeurApp.Application;
using Microsoft.AspNetCore.Mvc;

namespace ChauffeurApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseAPIController : ControllerBase
    {
        protected ActionResult HandleResult<T>(Result<T> result)
        {
            if (result is null) return NotFound();
            if (result.IsSuccess && result.Value is not null)
                return Ok(result.Value);
            if (result.IsSuccess && result.Value is null)
                return NotFound();
            return BadRequest(result.Error);
        }
       
    }
}


    