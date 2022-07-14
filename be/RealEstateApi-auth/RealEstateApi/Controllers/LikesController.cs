using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
    public class LikesController : ControllerBase
    {
        private ILikeService _likeService;
        public LikesController(ILikeService likeService)
        {
            _likeService = likeService;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Like(int id)  //theo dõi 1 bài đăng
        {
            try
            {
                var creatorID = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "id").Value;
                var result = await _likeService.Like(creatorID, id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }

        
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetLike() //lấy bài đăng đang theo dõi
        {
            try
            {
                var creatorID = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "id").Value;
                var result = await _likeService.GetLike(creatorID);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }

        
    }
}
