using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RealEstate.Application.DTOs;
using RealEstate.Application.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace RealEstate.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostManagementsController : ControllerBase
    {
        private IPostService _postService;
        private IAuthService _authService;
        public PostManagementsController(IPostService postService, IAuthService authService)
        {
            _postService = postService;
            _authService = authService;
        }

        [HttpGet]
        [Authorize(Roles ="Admin,Staff")]
        public async Task<IActionResult> Get([FromQuery] FilterParams filterParams)
        {
            var result = await _postService.GetPostsByAdmin(filterParams);
            return Ok(result);
        }

        [HttpGet("[action]")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> Report1()
        {
            var result = await _postService.GetReport1();
            return Ok(result);
        }

        [HttpGet("[action]")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> Report2()
        {
            var result = await _postService.GetReport2();
            return Ok(result);
        }

        [HttpGet("[action]")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> Report3()
        {
            var result = await _postService.GetReport3();
            return Ok(result);
        }

        [HttpGet("[action]")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> Report4(int id) //theo tỉnh thành, mặc định truyền bên client lấy id=1
        {
            var result = await _postService.GetReport4(id);
            return Ok(result);
        }

        [HttpPut]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> ChangeStatus([FromBody] PostStatus postStatus)
        {
            var result = await _postService.ChangeStatus(postStatus);
            if (result == null)
                return NotFound();
            return Ok(result);
        }
    }
}
