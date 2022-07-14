using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RealEstate.Application.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RealEstate.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostTypesController : ControllerBase
    {
        private IPostTypeService _postTypeService;
        public PostTypesController(IPostTypeService postTypeService)
        {
            _postTypeService = postTypeService;
        }
        

        [HttpGet]
        public async Task<IActionResult> GetPostTypes()
        {
            try
            {
                var result = await _postTypeService.GetPostTypes();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }
    }
}
