using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RealEstate.Application.DTOs;
using RealEstate.Application.IServices;
using RealEstate.Domain.Entities;
using System.Linq;
using System.Threading.Tasks;
namespace RealEstate.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsController : ControllerBase
    {
        private INewsService _newService;
        private IAuthService _authService;
        public NewsController(INewsService newService, IAuthService authService)
        {
            _newService = newService;
            _authService = authService;
        }

        [HttpGet]
        public async Task<IActionResult> GetNews(int? page, int? size, bool? display, string ? search) 
        {
            var result = await _newService.GetNews(page, size, display, search);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetNewsById(int id) 
        {
            var result = await _newService.GetNewById(id);

            return Ok(result);
        }


        [HttpPost]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> CreateNews([FromForm]NewsDto newsDto)
        {
            var result = await _newService.AddNews(newsDto);
            return Ok(result);
        }

        [HttpPut]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> UpdateNews([FromForm] NewsDto newsDto)
        {
            var result = await _newService.UpdateNews(newsDto);
            return Ok(result);
        }

        [HttpPut("[action]")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> UpdateStatusNews(int id, bool display)
        {
            var result = await _newService.UpdateStatusNews(id, display);
            return Ok(result);
        }

        [HttpDelete]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> DeleteNews(int id)
        {
            var result = await _newService.DeleteNews(id);
            return Ok(result);
        }
    }
}
