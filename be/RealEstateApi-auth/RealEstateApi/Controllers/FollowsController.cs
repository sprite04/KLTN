using Microsoft.AspNetCore.Mvc;
using RealEstate.Application.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RealEstate.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FollowsController : ControllerBase
    {
        private IFollowService _followService;
        public FollowsController(IFollowService followService)
        {
            _followService = followService;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Follow(string id)  //theo dõi 1 ai đó
        {
            try
            {
                var creatorID = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "id").Value;
                var result = await _followService.Follow(creatorID, id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> CheckFollow(string idUser1, string idUser2) // người 1 theo dõi người 2 ?? cần kiểm tra lại 1 theo dõi 2 hay ngược lại
        {
            var result = await _followService.CheckFollow(idUser1, idUser2);
            return Ok(result);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetFollow(string id) //lấy người đang theo dõi người này
        {
            var result = await _followService.GetFollow(id);
            return Ok(result);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetFollowed(string id) //lấy người được theo dõi bởi người này
        {
            var result = await _followService.GetFollowed(id);
            return Ok(result);
        }
    }
}
